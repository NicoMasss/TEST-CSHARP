using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductClientHub.API.UseCases.Clients.Register;
using ProductClientHub.API.UseCases.Tasks.Register;
using ProductClientHub.Communication.Requests;
using ProductClientHub.Communication.Responses;

namespace ProductClientHub.API.Controllers
{
    [Route("api/Tasks")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        [HttpPost("createTask")]
        [ProducesResponseType(typeof(AuthResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorManagerJson), StatusCodes.Status400BadRequest)]
        public IActionResult CreateTask([FromBody] TaskRequest request)
        {
            var useCase = new RegisterTasksUserCase();

            var response = useCase.Execute(request);

            return Created(string.Empty, new TaskResponse());
        }

        [HttpGet]
        [Route("{taskId}")]
        public IActionResult GetById([FromRoute] Guid taskId)
        {
            //var UseCase = new RegisterTasksUserCase();

            //var response = UseCase.Execute(Use);

            return Ok();
        }

        [HttpGet]
        public IActionResult GetTaskByUser([FromQuery] Guid AssignedTo) 
        {
            return Ok();
        }

        [HttpPut]
        [Route("{taskId}")]
        public IActionResult UptadeTask([FromRoute] Guid taskId, [FromBody] TaskRequest request) 
        {
            return NoContent();
        }

        [HttpDelete]
        [Route("{taskId}")]
        public IActionResult DeleteTask(Guid taskId) 
        {
            return NoContent();
        }
    }
}
