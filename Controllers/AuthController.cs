using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Tasks_Handler.Data;
using Tasks_Handler.Dtos.User;
using Tasks_Handler.Models;
using Tasks_Handler.Services.TokenService;

namespace Tasks_Handler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly ITokenManager _tokenManager;
        public AuthController(IAuthRepository authRepo, ITokenManager tokenManager)
        {
            _tokenManager = tokenManager;
            _authRepo = authRepo;

        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto request)
        {
            ServiceResponse<int> response = new ServiceResponse<int>();
            if (request.Password != request.ConfirmPassword)
            {
                response.Success = false;
                response.Message = "Password and Confirm Password are not the same.";
                return BadRequest(response);
            }

            response = await _authRepo.Register(
               new User { FirstName = request.FirstName, LastName = request.LastName, PhoneNumber = request.PhoneNumber, Email = request.Email }, request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLoginDto request)
        {
            ServiceResponse<string> response = await _authRepo.Login(
               request.Email, request.Password
            );
            if (!response.Success)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            await _tokenManager.DeactivateCurrentAsync();

            return NoContent();
        }
    }
}