using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using TicketingSystem.Model;
using TicketingSystem.Model.ViewModels;
using TicketingSystem.Repository;

namespace TicketingSystem.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("AddUser")]
        public IActionResult AddUser(User User)
        {
            try
            {
                var res = _userService.InsertUser(User);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpGet("ActiveDeactiveUser/{id}")]
        public IActionResult ActiveDeactiveUser(string id)
        {
            try
            {
                var res = _userService.ActiveDeactiveUser(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        //Authorize(Policy = "AdminOnly")]
        [HttpGet("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            try
            {
                var UserList = _userService.GetAllUsers();
                return Ok(UserList);
            }
            catch (Exception ex)
            {

                return BadRequest($"Error: {ex.Message}");
            }

        }

        //[Authorize]
        [HttpGet("GetUser/{id}")]
        public IActionResult GetUserById(string id)
        {
            try
            {
                var res = _userService.GetUserById(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        //[Authorize]
        [HttpPut("UpdateUser/{id}")]
        public IActionResult UpdateUser(string id, [FromBody] UserVM updatedUser)
        {
            try
            {                               
                _userService.UpdateUser(id, updatedUser);
                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }

        //[Authorize]
        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser(string id)
        {
            try
            {
                var res = _userService.DeleteUser(id);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }

        }

    }
}
