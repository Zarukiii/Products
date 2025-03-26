namespace Application.Services.Jwt
{
    public interface IJwtService
    {
        Task<IDictionary<string, object>> DecodeJwtAsync(string jwtToken);
    }
}
