using Microsoft.AspNetCore.Mvc;
using ProductClientHub.API.Infraestructure;
using ProductClientHub.API.UseCases.Auths.Register;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using ProductClientHub.API.Helpers;
using ProductClientHub.API.UseCases.Clients.Register;


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
        public IActionResult Login([FromBody] AuthRequestLogin request)
        {
            var repository = new UserAdminRepository();

            var client = repository.GetByEmail(request.Email);

            var passwordHash = client.passwordHash;

            Guid Id = client.Id; 

            var user = client.user;

            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(client.passwordHash);

            if (passwordHash == null || !BCrypt.Net.BCrypt.Verify(request.Password, passwordHash))
            {
                return Unauthorized("Credenciais inválidas");
            }

            var token = JwtTokenGenerator.Generate(Id, user.Email);

            return Ok(new AuthResponse { Token = token });
        }

        [AllowAnonymous]
        [HttpPost("RegisterAdm")] 
        public IActionResult Register([FromBody] AuthRequestRegister request) 
        {
            var repository = new UserAdminRepository();

            var success = repository.Add(request);

            return Created(string.Empty, success);
        }

        [HttpPost("logout")]
        public IActionResult Logout() 
        {
            return Ok(new { message = "Logout realizado com sucesso." });
        }
    }
}
