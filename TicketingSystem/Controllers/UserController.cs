/*
   File: UserController.cs
   Description: This file contains the implementation of the UserController class, which handles user-related operations.
   Author: Prabhash D.V.S.
   Creation Date: 2023/10/03
   Last Modified Date: 2023/10/08  
*/
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

        /// <summary>
        /// Handles the HTTP POST request to add a new user.
        /// </summary>
        /// <param name="user">A User object containing details of the user to be added.</param>
        /// <returns>Returns an HTTP response with the added user information if successful, 
        /// or a bad request response with an error message if an exception occurs.</returns>
        [HttpPost("AddUser")]
        public IActionResult AddUser(User user)
        {
            try
            {
                var res = _userService.InsertUser(user);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Handles the HTTP GET request to activate or deactivate a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to activate or deactivate.</param>
        /// <returns>Returns an HTTP response with the activation status if successful,
        /// or a bad request response with an error message if an exception occurs or the user is not found.</returns>
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

        /// <summary>
        /// Handles the HTTP GET request to retrieve a list of all users.
        /// </summary>
        /// <returns>Returns an HTTP response with a list of all users if successful,
        /// or a bad request response with an error message if an exception occurs.</returns>
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

        /// <summary>
        /// Handles the HTTP GET request to retrieve a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to retrieve.</param>
        /// <returns>Returns an HTTP response with the user information if found,
        /// or a bad request response with an error message if an exception occurs or the user is not found.</returns>
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

        /// <summary>
        /// Handles the HTTP PUT request to update a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to update.</param>
        /// <param name="updatedUser">A UserVM (User View Model) object containing the updated user details.</param>
        /// <returns>Returns an HTTP response with the updated user information if successful,
        /// or a bad request response with an error message if an exception occurs or the user is not found.</returns>
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

        /// <summary>
        /// Handles the HTTP DELETE request to delete a user by their unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the user to be deleted.</param>
        /// <returns>Returns an HTTP response with a success message if the deletion is successful, 
        /// or a bad request response with an error message if an exception occurs or the user is not found.</returns>
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
