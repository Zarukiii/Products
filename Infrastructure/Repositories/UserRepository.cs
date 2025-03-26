using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Dto.Response.UserResponse;
using Application.Dto.UserDto;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public UserRepository(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<LoginUserResponse> LoginUserAsync(LoginUserDto loginUserDto)
        {
            var getUser = await FindUserByUserName(loginUserDto.UserName);
            if (getUser == null)
            {
                return new LoginUserResponse(false, "User Not Found");
            }

            bool verifyPassword = BCrypt.Net.BCrypt.Verify(loginUserDto.Password, getUser.Password);

            if (verifyPassword)
            {
                return new LoginUserResponse(true, "Login Successful", GenerateJWTToken(getUser));
            }
            else
            {
                return new LoginUserResponse(false, "Invalid Credentials");
            }
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDto registerUserDto)
        {
            var getUser = await FindUserByUserName(registerUserDto.UserName);
            if (getUser != null)
            {
                return new RegisterUserResponse(true, "User Already Exists");
            }

            _context.Users.Add(new User()
            {
                UserName = registerUserDto.UserName,
                Email = registerUserDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerUserDto.Password)
            });
            await _context.SaveChangesAsync();

            return new RegisterUserResponse(true, "Registration Successful");
        }

        private async Task<User> FindUserByUserName(string userName)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName);
            return user;
        }

        private string GenerateJWTToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName!),
                new Claim(ClaimTypes.Email, user.Email!)
            };
            var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: userClaims,
                    expires: DateTime.Now.AddDays(5),
                    signingCredentials: credentials
                );

            Console.WriteLine("Token generation key: " + _config["Jwt:Key"]);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
