using Serilog;
using System.Security.Claims;
using Core.Identity;

namespace Core.Authorization
{
    public interface IAuthorizeService
    {
        Task<JwtAuthResult> LoginAsync(string username, string password);
    }

    public class AuthorizeService : IAuthorizeService
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenGenerator _jwtAuthManager;
        private readonly ILogger _logger;

        public AuthorizeService(IUserService userService, IJwtTokenGenerator jwtAuthManager, ILogger logger)
        {     
            _userService = userService;
            _jwtAuthManager = jwtAuthManager;
            _logger = logger;
        }

        public async Task<JwtAuthResult> LoginAsync(string username, string password)
        {
            if (await _userService.IsValidUserCredentialsAsync(username, password))
            {
                var role = await _userService.GetUserRoleAsync(username);
                var claims = GetUserClaims(username, role);
                var token = _jwtAuthManager.GenerateTokens(username, claims, DateTime.Now);
                _logger.Information($"Пользователь [{username}] вошел в систему.");

                return new JwtAuthResult()
                {
                    UserName = username,
                    Role = role,
                    AccessToken = token
                };
            }

            throw new ArgumentException("Неверное имя пользователя или пароль");
        }


        private Claim[] GetUserClaims(string username, string role) =>
            [
                new(ClaimsIdentity.DefaultNameClaimType, username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
            ];
        
    }
}
