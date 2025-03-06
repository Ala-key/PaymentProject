using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Identity;
using Core.Models;
using DataLayer.Models;
using PaymentProject.Dto;
using PaymentProject.Response;

namespace PaymentProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController(Serilog.ILogger logger) : ControllerBase
    {
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpGet("all")]
        public IQueryable<User> GetUsers([FromServices] UserManager<User> userManager) =>
             userManager.Users;
        
        [HttpPost("create-user")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateUser([FromServices] IUserCreator userCreator, [FromBody] UserRequest model)
        {
            try
            {
                var user = await userCreator.AddNewUserAsync(model.UserName, model.Password);
                return Ok(new UserResponse(user));
            }
            catch(InvalidOperationException)
            {
                return BadRequest();
            }
        }
        
        [HttpGet("get-balance")]
        public async Task<IActionResult> GetBalance([FromServices] IUserService service)
        {
            try
            {
                var balance = await service.GetBalance(User?.Identity?.Name);
                return Ok(new BalanceResponse(balance));
            }
            catch (InvalidOperationException ex)
            {
                logger.Error(ex, ex.Message);
                return BadRequest(new { ex.Message });
            }
        }
        
        [HttpGet("my-role")]
        public async Task<IActionResult> GetMyRole([FromServices]UserManager<User>  manager)
        {
            try
            {
                var user = await manager.FindByNameAsync(User.Identity?.Name);
                var roles = await manager.GetRolesAsync(user);
                return Ok(roles);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
