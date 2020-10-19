using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    [Route("users")]
    public class UserController : ControllerBase
    {
        [HttpGet]
        [Route("{userId}")]
        IActionResult GetUser()
        {
            return Ok();
        }
        
        [HttpPost]
        [Route("")]
        IActionResult METHOD_TEMPLATE_POST()
        {
            return Ok();
        }
        
        [HttpPut]
        [Route("{userId}")]
        IActionResult UpdateUser()
        {
            return Ok();
        }
        
        [HttpDelete]
        [Route("{userId}")]
        IActionResult UserDelete()
        {
            return Ok();
        }
        
    }
}