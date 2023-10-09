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

                var resObject = new { userData = userRes, tokenData = token };

                return Ok(resObject);
            }
            catch (Exception ex)
            {
                return Unauthorized(new BaseResponseService().GetErrorResponse(ex));
            }
           ;
        }

    }
}
