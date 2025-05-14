<<<<<<< Updated upstream
﻿using Microsoft.AspNetCore.DataProtection.Repositories;
using Microsoft.AspNetCore.Mvc;
=======
﻿using Microsoft.AspNetCore.Mvc;
>>>>>>> Stashed changes
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

<<<<<<< Updated upstream
                var response = useCase.Execute(request);                
=======
            var response = useCase.Execute(request);
>>>>>>> Stashed changes

            return Created(string.Empty, response);
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

<<<<<<< Updated upstream
            var users = repository.Get();

            return Ok(users);
=======
            var clients = repository.Get();
            return Ok(clients);
>>>>>>> Stashed changes
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            return Ok();
        }
    }
}
