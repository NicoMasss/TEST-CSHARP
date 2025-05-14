using Microsoft.AspNetCore.Mvc;
using ProductClientHub.API.UseCases.Auths.Register;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorManagerJson), StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            var useCase = new RegisterAuthsUserCase();
            var response = useCase.Execute(request);

            var token = "fake=jwt";

            return Created(string.Empty, response);
        }

        [HttpPost("logout")]
        public IActionResult Logout() 
        { 
            return BadRequest(); 
        }
    }
}
