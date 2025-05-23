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
        [Authorize]
        [HttpPost]
        [ProducesResponseType(typeof(ResponseClientJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorManagerJson), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RequestClientJson request)
        {
                var useCase = new RegisterClientUserCase();

                var response = useCase.Execute(request);                

                return Created(string.Empty, response);
        }

        [HttpPut]
        public IActionResult Update([FromBody] RequestClientJsonUpdate request)
        {
            var repository = new ClientRepository();

            var users = repository.Update(request);

            if (!users)
                return BadRequest("Email ou senha incorretos.");

            return Ok(users);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var repository = new ClientRepository();

            var users = repository.Get();

            return Ok(users);
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            var repository = new ClientRepository();

            var user = repository.GetById(id);

            return Ok(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var repository = new ClientRepository();

            var found = repository.Delete(id);

            return Ok(found);
        }
    }
}
