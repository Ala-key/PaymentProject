using Core.Models;
using Core.Services;
using DataLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace PaymentProject.Controllers
{
    [ApiController]
    [Route("api/payments")]
    [Authorize]
    public class PaymentController(UserManager<User> manager, Serilog.ILogger logger): ControllerBase
    {
        [HttpGet("pay")]
        public async Task<IActionResult> Pay([FromServices] IPaymentService paymentService)
        {
            try
            {
                var user = await manager.FindByNameAsync(User?.Identity?.Name);
                
                if (user is null) 
                    return Unauthorized();
                
                await paymentService.PayAsync(user);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                logger.Error(ex, ex.Message);
                return BadRequest(new { ex.Message });
            }
        }
        
        [Authorize(Roles = DefaultRoles.Admin)]
        [HttpGet("user-payments")]
        public async Task<IActionResult> GetUserPayments([FromServices] IPaymentService paymentService)
        {
            try
            {
                var user = await manager.FindByNameAsync(User?.Identity?.Name);
                
                if (user is null) 
                    return Unauthorized();
                
                var payments = await paymentService.GetUserPaymentsAsync(user);
                return Ok(payments);
            }
            catch (InvalidOperationException ex)
            {
                logger.Error(ex, ex.Message);
                return BadRequest(new { ex.Message });
            }
        }
    }
}
