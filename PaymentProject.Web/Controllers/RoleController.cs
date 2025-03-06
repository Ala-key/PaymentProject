using Core.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Core.Models;

namespace PaymentProject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RoleController : Controller
    {
        [HttpGet("all")]
        public IQueryable<IdentityRole> GetRoles([FromServices] RoleManager<IdentityRole> roleManager)
        {
            return roleManager.Roles;
        }

        [HttpPost("add-role")]
        public async Task<IActionResult> AddRole([FromServices]RoleManager<IdentityRole> roleManager, string roleName)
        {
            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            return result == IdentityResult.Success ? Ok() : BadRequest(); 
        }
        
        [HttpPost("set-admin")]
        public async Task<IActionResult> SetAdmin([FromServices] IRoleChanger roleChanger, string login)
        {
            var result = await roleChanger.ChangeAsync(login, DefaultRoles.Admin);
            return result == IdentityResult.Success ? Ok() : BadRequest();
        }
        
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpPost("set-user")]
        public async Task<IActionResult> SetUser([FromServices] IRoleChanger roleChanger, string login)
        {
            var result = await roleChanger.ChangeAsync(login, DefaultRoles.User);
            return result == IdentityResult.Success ? Ok() : BadRequest();
        }
    }
}
