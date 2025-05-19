using Microsoft.AspNetCore.Mvc;
using ProductClientHub.API.Infraestructure;
using ProductClientHub.API.UseCases.Auths.Register;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ProductClientHub.API.Helpers;


namespace ProductClientHub.API.Controllers
{
    [Route("api/Auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorManagerJson), StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody] AuthRequest request)
        {
            var repository = new ClientRepository();

            var client = repository.GetByEmail(request.Email);

            var passwordHash = client.passwordHash;

            Guid Id = client.Id; 

            var user = client.user;


            if (passwordHash == null || !BCrypt.Net.BCrypt.Verify(request.Password, passwordHash))
            {
                return Unauthorized("Credenciais inválidas");
            }

            var token = JwtTokenGenerator.Generate(Id, user.Email);

            return Ok(new AuthResponse { Token = token });
        }

        [HttpPost("logout")]
        public IActionResult Logout() 
        { 
            return BadRequest(); 
        }
    }
}
