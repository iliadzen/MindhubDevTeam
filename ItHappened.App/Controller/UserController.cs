using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    [Route("users")]
    public class UserController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        [Route("{userId}")]
        IActionResult GetUser([FromRoute] Guid userId)
        {
            return Ok(userId);
        }
        
        [HttpPost]
        [Route("")]
        IActionResult CreateUser()
        {
            return Ok();
        }
        
        [Authorize]
        [HttpPut]
        [Route("{userId}")]
        IActionResult UpdateUser([FromRoute] Guid userId)
        {
            return Ok();
        }
        
        [Authorize]
        [HttpDelete]
        [Route("{userId}")]
        IActionResult UserDelete([FromRoute] Guid userId)
        {
            return Ok();
        }
        
    }
}