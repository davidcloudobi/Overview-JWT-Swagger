using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.DTO;
using Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Overview.Controllers
{

    /// <summary>
    /// Version 1, User
    /// </summary>
    /// 
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "User")]
    [Route("api/v1.0/[controller]")]
    //[Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserServices _userServices;
        
        /// <summary>
        /// user services
        /// </summary>
        /// <param name="userServices"></param>
        public UserController(IUserServices userServices )
        {
            _userServices = userServices;
            
        }


        // POST: api/User/login
        /// <summary>
        ///  Return a login user
        /// </summary>
        /// <param name="user"></param>
        /// <remarks>
        /// Sample request (this request return  a login user)
        /// </remarks>
        /// <response code="200">Returns  a login user </response>

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Login(UserLoginDTO user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.Login(user);

                return Ok(result);
            }

            return BadRequest("Invalid details");
        }




        // POST: api/User/register
        /// <summary>
        ///  Return a register user
        /// </summary>
        /// <param name="user"></param>
        /// <remarks>
        /// Sample request (this request return  a register user)
        /// </remarks>
        /// <response code="200">Returns  a register user </response>
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> Register(UserRegDTO user)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.Register(user);

                return Ok(result);
            }

            return BadRequest("Invalid details");
        }

        // GET: api/User/current
        /// <summary>
        ///  Return a current user
        /// </summary>
        /// <remarks>
        /// Sample request (this request return  a current user)
        /// </remarks>
        /// <response code="200">Returns  a current user </response>
        [Authorize(Roles = "User")]
        [HttpGet("current")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> CurrentUser()
        {

            var result = await _userServices.CurrentUser();
            if (result != null)
            {
              

                return Ok(result);
            }

            return BadRequest("Invalid details");
        }


        // POST: api/User/addPhone
        /// <summary>
        ///  Return a new phone
        /// </summary>
        /// <param name="phone"></param>
        /// <remarks>
        /// Sample request (this request return a new phone)
        /// </remarks>
        /// <response code="200">Returns  a new phone </response>
      
        [HttpPost("addPhone")]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> AddPhone(PhoneDTO phone)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.AddPhone(phone);

                return Ok(result);
            }

            return BadRequest("Invalid details");
        }



        // POST: api/User/changePassword
        /// <summary>
        ///  Return a new password
        /// </summary>
        /// <param name="details"></param>
        /// <remarks>
        /// Sample request (this request return a new password)
        /// </remarks>
        /// <response code="200">Returns  a new password </response>

        [HttpPost("changePassword")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ChangePassword(PasswordDTO details)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.ChangePassword(details);

                 return Ok(result);
            }

            return BadRequest("Invalid details");
        }


        // POST: api/User/addPhone
        /// <summary>
        ///  Return a new password
        /// </summary>
        /// <param name="details"></param>
        /// <remarks>
        /// Sample request (this request reset password)
        /// </remarks>
        /// <response code="200">Returns  a new password </response>

        [HttpPost("resetPassword")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
        public async Task<IActionResult> ResetPassword(DisplayPasswordDTO details)
        {
            if (ModelState.IsValid)
            {
                var result = await _userServices.ResetPassword(details);

                return Ok(result);
            }

            return BadRequest("Invalid details");
        }
    }
}