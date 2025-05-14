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
        [HttpPost("createClient")]
        [ProducesResponseType(typeof(ResponseClientJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorManagerJson), StatusCodes.Status400BadRequest)]
        public IActionResult Register([FromBody] RequestClientJson request)
        {
            var useCase = new RegisterClientUserCase();

            var response = useCase.Execute(request);

            var repository = new ClientRepository();

            var success = repository.Add(request);

            return Created(string.Empty, success);
        }

        [HttpPut]
        public IActionResult Update()
        {
            return Ok();
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

            var client = repository.GetByid(id);

            return Ok(client);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var repository = new ClientRepository();

            var client = repository.GetByid(id);

            return Ok(client);
        }
    }
}
