using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
using ProductClientHub.API.Infraestructure;
using ProductClientHub.API.UseCases.Clients.Register;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.Controllers
{
    [Route("api/Clientes")]
    [ApiController]

    public class ControllerClients : ControllerBase
    {
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseClientJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorManagerJson), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RequestClientJson request)
        {
                var useCase = new RegisterClientUserCase();

                var response = useCase.Execute(request);                

                return Created(string.Empty, response);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPut]
        public IActionResult Update([FromBody] RequestClientJsonUpdate request)
        {
            var repository = new ClientRepository();

            var users = repository.Update(request);

            if (!users)
                return BadRequest("Email ou senha incorretos.");

            return Ok(users);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var repository = new ClientRepository();

            var users = repository.Get();

            return Ok(users);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var repository = new ClientRepository();

            var user = repository.GetById(id);

            return Ok(user);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var repository = new ClientRepository();

            var found = repository.Delete(id);

            return Ok(found);
        }
    }
}
