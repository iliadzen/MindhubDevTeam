using System;
using System.Security.Claims;
using ItHappened.App.Authentication;
using ItHappened.App.Model;
using ItHappened.Application;
using ItHappened.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ItHappened.App.Controller
{
    [Route("users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtIssuer _jwtIssuer;
        public UserController(IUserService userService, IJwtIssuer jwtIssuer)
        {
            _userService = userService;
            _jwtIssuer = jwtIssuer;
        }

        [Authorize]
        [HttpGet]
        [Route("self")]
        public IActionResult GetUser()
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var optionUser = _userService.GetUserById(actorId,actorId);
            return optionUser.Match(
                Some: user =>
                {
                    var userGetResponse = new UserGetResponse(user);
                    return Ok(userGetResponse);
                }, 
                None: Ok("Error! :("));
        }
        
        [HttpPost]
        [Route("signin")]
        public IActionResult LoginUser([FromBody] LoginRequest loginRequest)
        {
            var optionUser = _userService.LogInByCredentials(loginRequest.Username, loginRequest.Password);
            return optionUser.Match(
                Some: user =>
                {
                    var token = _jwtIssuer.GenerateToken(user);
                    var userCreateResponse = new UserCreateResponse(token);
                    return Ok(userCreateResponse);
                }, 
                None: Ok("Error! :("));
        }
        
        [HttpPost]
        [Route("signup")]
        public IActionResult CreateUser([FromBody] UserCreateRequest userCreateRequest)
        {
            var userForm = new UserForm(userCreateRequest.Username, userCreateRequest.Password, new License());
            _userService.CreateUser(userForm);
            var optionUser = _userService.LogInByCredentials(userForm.Username, userForm.Password);

            return optionUser.Match(
                Some: user =>
                {
                    var token = _jwtIssuer.GenerateToken(user);
                    var userCreateResponse = new UserCreateResponse(token);
                    return Ok(userCreateResponse);
                }, 
                None: Ok("Error! :("));
        }
        
        [Authorize]
        [HttpPut]
        [Route("self")]
        public IActionResult UpdateUser([FromBody] UserUpdateRequest userUpdateRequest)
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var optionUser = _userService.GetUserById(actorId,actorId);
            return optionUser.Match(
                Some: user =>
                {
                    var userForm = new UserForm(userUpdateRequest.Username, userUpdateRequest.Password, user.License);
                    _userService.EditUser(actorId, actorId, userForm);
                    return Ok("Success!");
                }, 
                None: Ok("Error! :("));
        }
        
        [Authorize]
        [HttpDelete]
        [Route("self")]
        public IActionResult UserDelete()
        {
            var actorId = Guid.Parse(User.FindFirstValue(JwtClaimTypes.Id));
            var optionUser = _userService.GetUserById(actorId,actorId);
            return optionUser.Match(
                Some: user =>
                {
                    _userService.DeleteUser(actorId, actorId);
                    return Ok("Success!");
                }, 
                None: Ok("Error! :("));
        }
        
    }
}