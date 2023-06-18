using Microsoft.AspNetCore.Mvc;
using HandyCook.Communication.RequestModels;
using HandyCook.Communication.ResponseModels;
using HandyCook.API.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;

namespace HandyCook.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<ActionResult<LoginResponse>> Login(LoginRequest loginRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _signInManager.PasswordSignInAsync(loginRequest.Email,
                           loginRequest.Password, loginRequest.RememberMe, lockoutOnFailure: true);
            if (!result.Succeeded)
            {
                return Unauthorized(result);
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<User>> Register(RegisterRequest registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = new User() { Email = registerRequest.Email };
            var result = await _userManager.CreateAsync(user, registerRequest.Password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok(result);
        }

        [HttpPost("Logout")]
        public async Task<ActionResult<User>> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
