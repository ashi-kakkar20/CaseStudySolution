using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TopUp_Beneficiary.Interfaces;
using TopUp_Beneficiary.Models;

namespace TopUp_Beneficiary.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthenticationService _authenticationService;

        public UserController(IUserService userService, IAuthenticationService authenticationService)
        {
            _userService = userService;
           
            _authenticationService = authenticationService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate(UsersAuthenticate model)
        {
            try
            {
                if (!string.IsNullOrEmpty(model.Username) && !(string.IsNullOrEmpty(model.Password)))
                
                {
                    var user = _userService.Authenticate(model.Username, model.Password);

                    if (user == null)
                        return BadRequest(new { message = "Username or password is incorrect" });

                    var token = _authenticationService.GenerateJwtToken(model.Username);
                    // Set JWT token as cookie
                    Response.Cookies.Append("access_token", token, new Microsoft.AspNetCore.Http.CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTime.UtcNow.AddHours(1) // Set expiration time as needed
                    });


                    return Ok(new { token }); // message = "Authentication successful" 
                }
                else
                {
                    return BadRequest(new { message = "Please enter the  Username & password " });
                }
            }

            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during Authenticate - " + ex.Message);

            }
        }

        [Authorize]
        [HttpGet("GetUserId")]
        public async Task<IActionResult> GetUserId(string username)
        {
            try
            {
                var beneficiaries = _userService.GetUserId(username);
                return Ok(beneficiaries);
            }

            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during GetUserId - " + ex.Message);

            }

        }


    }


}

