using Microsoft.Extensions.Options;

namespace Core.Authorization
{
    public class JwtSettingsConfiguration : IConfigureOptions<JwtSettings>
    {
        public void Configure(JwtSettings options)
        {
            string secret = Environment.GetEnvironmentVariable("JWT_SECRET");

            if (!string.IsNullOrEmpty(secret))
            {
                options.Secret = secret;
            }
        }
    }
}
