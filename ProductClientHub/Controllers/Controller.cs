using Microsoft.AspNetCore.Mvc;

namespace ProductClientHub.Controllers
{
    [Route("api/Clientes")]
    [ApiController]

    public class Controller : ControllerBase
    {
        [HttpPost]
        public IActionResult Register()
        {
            return Ok();
        }

        [HttpPut]
        public IActionResult Update()
        {
            return Ok();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult GetById(Guid id)
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
