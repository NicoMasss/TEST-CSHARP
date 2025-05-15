using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductClientHub.API.Infraestructure;
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

            return Created(string.Empty, response);
        }

        [HttpGet]
        [Route("{taskId}")]
        public IActionResult GetById([FromRoute] Guid taskId)
        {
            var repository = new TaskRepository();

            var task = repository.GetById(taskId);

            return Ok(task);
        }

        [HttpGet]
        public IActionResult GetTaskByUser([FromQuery] Guid AssignedTo) 
        {
            var repository = new TaskRepository();

            var task = repository.GetByAssignedTo(AssignedTo);

            return Ok(task);
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
            var repository = new TaskRepository();

            var success = repository.Delete(taskId);

            return Ok(success);
        }
    }
}
