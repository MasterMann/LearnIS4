using Is4Server.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Is4Server.Controllers
{
    [Route("api/external-login")]
    [AllowAnonymous]
    public class ExternalLoginController : Controller
    {
        #region Methods

        [HttpPost, HttpGet]
        [Consumes("application/x-www-form-urlencoded")]
        public IActionResult Login([FromForm] ExternalLoginViewModel model)
        {
            return Ok();
        }

        #endregion
    }
}