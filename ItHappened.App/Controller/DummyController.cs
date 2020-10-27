using System;
using System.Security.Claims;
using ItHappened.App.Authentication;
using ItHappened.App.Model;
using ItHappened.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    [Route("dummy")]
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
                Guid.NewGuid(), 
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