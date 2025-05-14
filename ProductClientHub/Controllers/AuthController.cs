using Microsoft.AspNetCore.Mvc;
<<<<<<< Updated upstream
using ProductClientHub.API.UseCases.Auths.Register;
=======
>>>>>>> Stashed changes
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.Controllers
{
<<<<<<< Updated upstream
    [Route("api/Auth")]
=======
    [Route("api/auth")]
>>>>>>> Stashed changes
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
<<<<<<< Updated upstream
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
=======
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status200OK)]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            var response = new AuthResponse
            {
                Token = "fake-jwt-token",
                Expiration = DateTime.UtcNow.AddHours(1),
                UserId = Guid.NewGuid(),
                Email = request.Email
            };

            return Ok(response);
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return NoContent();
>>>>>>> Stashed changes
        }
    }
}
