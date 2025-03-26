using System.Text;
using System.Text.Json;

namespace Application.Services.Jwt
{
    public class JwtService : IJwtService
    {
        public async Task<IDictionary<string, object>> DecodeJwtAsync(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
            {
                throw new ArgumentException("JWT token cannot be null or empty");
            }

            var parts = jwtToken.Split('.');

            if (parts.Length < 2)
            {
                throw new ArgumentException("Invalid JWT token");
            }

            var payload = parts[1];

            await Task.Delay(1);

            var jsonPayload = Encoding.UTF8.GetString(Base64UrlDecode(payload));

            return JsonSerializer.Deserialize<Dictionary<string, object>>(jsonPayload);
        }

        private static byte[] Base64UrlDecode(string input)
        {
            input = input.Replace('-', '+').Replace('_', '/');
            switch (input.Length % 4)
            {
                case 2: input += "=="; break;
                case 3: input += "="; break;
            }
            return Convert.FromBase64String(input);
        }
    }
}
