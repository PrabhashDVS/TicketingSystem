/*
   File: LoginController.cs
   Description: This file contains the implementation of the LoginController class, which handles user authentication.
   Author: Prabhash D.V.S.
   Creation Date: 2023/10/03
   Last Modified Date: 2023/10/10  
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TicketingSystem.Model;
using TicketingSystem.Repository;

namespace TicketingSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly JwtTokenService _tokenService;
        private readonly LoginService _loginService;

        public LoginController(JwtTokenService tokenService, LoginService loginService)
        {
            _tokenService = tokenService;
            _loginService = loginService;
        }


        /// <summary>
        /// Handles the HTTP POST request for user login.
        /// </summary>
        /// <param name="user">A User object containing the NIC and password for login.</param>
        /// <returns>Returns an HTTP response containing an authentication token upon successful login,
        /// or an unauthorized response if the login fails.</returns>
        [HttpPost("login")]
        public IActionResult Login(User user)
        {
            try
            {
                string nic = user.NIC;                
                string password = user.Password;    
                
                var userRes = _loginService.Login(nic, password);

                string id = userRes.Id;
                string role = userRes.Role;

                string token = _tokenService.GenerateToken(id, role);

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new BaseResponseService().GetErrorResponse(ex));
            }
           ;
        }

    }
}
