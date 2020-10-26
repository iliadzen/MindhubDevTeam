using System;
using System.Security.Claims;
using ItHappened.App.Authentication;
using ItHappened.App.Model;
using ItHappened.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    /// <summary>
    /// Example of controller, that can auth user
    /// </summary>
    public class DummyController : ControllerBase
    {
        private readonly IJwtIssuer _jwtIssuer;

        public DummyController(IJwtIssuer jwtIssuer)
        {
            _jwtIssuer = jwtIssuer ?? throw new ArgumentNullException(nameof(jwtIssuer));
        }
        
        [HttpPost]
        [Route("")]
        public IActionResult TestPost([FromBody] LoginRequest request)
        {
            var user = new User(
                request.UserId, 
                "silkslime", 
                "lalala",
                new License(LicenseType.Premium, DateTime.Now),
                DateTime.Now,
                DateTime.Now
            );
            var token = _jwtIssuer.GenerateToken(user);
            return Ok(token);
        }

        [Authorize]
        [HttpGet]
        [Route("")]
        public IActionResult TestGet()
        {
            var userId = User.FindFirstValue(JwtClaimTypes.Id);
            
            return Ok(new
            {
                id = userId
            });
        }
    }
}