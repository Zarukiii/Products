using Application.Dto.Response.UserResponse;
using Application.Dto.UserDto;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserDto registerUserDto);
        Task<LoginUserResponse> LoginUserAsync(LoginUserDto loginUserDto);
    }
}
