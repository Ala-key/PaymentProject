using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Core.Authorization
{
    public interface IJwtTokenGenerator
    {
        string GenerateTokens(string username, Claim[] claims, DateTime now);
    }

    public class JwtTokenGenerator : IJwtTokenGenerator
    { 
        private readonly JwtSettings _jwtSettings;
        private readonly byte[] _secret;

        public JwtTokenGenerator(IOptions<JwtSettings> optionsJwtSettings)
        {
            _jwtSettings = optionsJwtSettings.Value;
            _secret = Encoding.UTF8.GetBytes(_jwtSettings.Secret);
        }

        public string GenerateTokens(string username, Claim[] claims, DateTime now)
        {
            var jwtToken = CreateJwtSecureToken(claims, now);
            var accessToken = GetAccessToken(jwtToken);
            return accessToken;
        }

        private bool HasAudienenceClaims(Claim[] claims)
        {
            return string.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
        }

        private JwtSecurityToken CreateJwtSecureToken(Claim[] claims, DateTime date)
        {
            return new JwtSecurityToken(
                _jwtSettings.Issuer,
                HasAudienenceClaims(claims) ? _jwtSettings.Audience : string.Empty,
                claims,
                notBefore: DateTime.Now,
                expires: date.AddMinutes(_jwtSettings.AccessTokenExpiration),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));
        }

        private string GetAccessToken(JwtSecurityToken jwtToken)
        {
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }
    }
}
