using Core.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PaymentProject.Dto;

namespace PaymentProject.Controllers
{
    [ApiController]
    [Route("api/")]
    public class AccountController(Serilog.ILogger logger) : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromServices] IAuthorizeService authorizationService, [FromBody] UserRequest request)
        {
            try
            {
                var result = await authorizationService.LoginAsync(request.UserName, request.Password);
                return Ok(result);
            }
            catch(ArgumentException ex)
            {
                logger.Error(ex, ex.Message);
                return Unauthorized(ex.Message);
            }
        }
        
        [Authorize]
        [HttpPost("logout")]
        public async Task<ActionResult> Logout()
        {
            HttpContext.Session.Clear();
            return Ok(result);
        }
    }
}
