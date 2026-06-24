using Microsoft.AspNetCore.Mvc;
using Shop.App.Filters;
using ShopDomain.Models;

namespace ShopApp.Controllers
{
    [ApiController]
    [Route("api/user")]
    [UserCheckFilter]
    public class UserController : ControllerBase
    {
        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            return Ok(user);
        }
    }
}




