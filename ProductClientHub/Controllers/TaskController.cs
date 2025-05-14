using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductClientHub.Communication.Requests;

namespace ProductClientHub.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskRequest request)
        {
            return Created(string.Empty, request);
        }

        [HttpGet("{id}")]
        public IActionResult GetTaskById([FromRoute] Guid id)
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetTasksByUser([FromQuery] Guid assignedTo)
        {
            return Ok();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask([FromRoute] Guid id, [FromBody] TaskRequest request)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask([FromRoute] Guid id)
        {
            return NoContent();
        }
    }
}
