using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    [Authorize]
    [Route("statistics")]
    public class StatisticsController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        IActionResult METHOD_TEMPLATE_GET()
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
        [Route("")]
        IActionResult METHOD_TEMPLATE_PUT()
        {
            return Ok();
        }
        
        [HttpDelete]
        [Route("")]
        IActionResult METHOD_TEMPLATE_DELETE()
        {
            return Ok();
        }

    }
}