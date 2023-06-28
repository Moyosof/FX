using FX.API.Helpers;
using FX.Application.Contracts;
using FX.Application.Contracts.Auth;
using FX.Application.DataContext;
using FX.DTO.WriteOnly.AuthDTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace FX.API.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : BaseController
    {
        private readonly IUserAuth _userAuth;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(
            IUserAuth userAuth,
            ITokenGenerator tokenGenerator,
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager
            )
        {
            _userAuth = userAuth;
            _tokenGenerator = tokenGenerator;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /// <summary>
        /// This is to register a new user
        /// </summary>
        /// <param name="registerUserDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create_user")]
        [ProducesResponseType(typeof(JsonMessage<string>), 200)]
        public async Task<IActionResult> CreateUser([FromBody] RegisterUserDTO registerUserDTO)
        {
            #region Register User
            var result = await _userAuth.RegisterUser(registerUserDTO);

            if (string.IsNullOrWhiteSpace(result))
            {
                // send email to verify email

                return Ok(new JsonMessage<string>()
                {
                    status = true,
                    success_message = "Confirm Email to complete authentication"
                });
            }

            return Ok(new JsonMessage<string>()
            {
                error_message = result,
                status = false,
                status_code = (int)HttpStatusCode.BadRequest
            });
            #endregion
        }

        [HttpPost]
        [Route("register_admin")]
        [ProducesResponseType(typeof(JsonMessage<string>), 200)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserDTO registerUserDTO)
        {
            #region register admin
            var result = await _userAuth.RegisterFXAdmin(registerUserDTO);
            if (string.IsNullOrWhiteSpace(result))
            {
                // send email to verify email

                return Ok(new JsonMessage<string>()
                {
                    status = true,
                    success_message = "Confirm Email to complete authentication"
                });
            }
            return Ok(new JsonMessage<string>()
            {
                error_message = result,
                status = false,
                status_code = (int)HttpStatusCode.BadRequest
            });
            #endregion
        }
    }

}
