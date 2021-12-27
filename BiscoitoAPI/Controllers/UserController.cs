using Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Model.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace BiscoitoAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly IEmailSender _emailSender;
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<BiscoitoAPIUser> _userManager;
        private readonly SignInManager<BiscoitoAPIUser> _signInManager;

        public UserController(IEmailSender emailSender, ILogger<UserController> logger, UserManager<BiscoitoAPIUser> userManager, SignInManager<BiscoitoAPIUser> signInManager)
        {
            _emailSender = emailSender;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("/login")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<bool> Login([FromBody] UserLoginDTO login)
        {
            try
            {
                var isLogged = await _signInManager.PasswordSignInAsync(login.Email, login.Password, false, lockoutOnFailure: false);
                return isLogged.Succeeded;
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR: {e.Message} | {DateTime.Now:F}");
                return false;
            }
        }


        //[HttpPost]
        //[Route("/forgotpassword")]
        //[ProducesResponseType(typeof(bool), 200)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public async Task<bool> ForgotPassword([FromBody] UserLoginDTO login)
        //{
        //    try
        //    {
        //        var user = await _userManager.FindByEmailAsync(Input.Email);
        //        if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
        //        {
        //            // Don't reveal that the user does not exist or is not confirmed
        //            return RedirectToPage("./ForgotPasswordConfirmation");
        //        }

        //        // For more information on how to enable account confirmation and password reset please 
        //        // visit https://go.microsoft.com/fwlink/?LinkID=532713
        //        var code = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
        //        var callbackUrl = Url.Page(
        //            "/Account/ResetPassword",
        //            pageHandler: null,
        //            values: new { area = "Identity", code },
        //            protocol: Request.Scheme);

        //        await _emailSender.SendEmailAsync(
        //            Input.Email,
        //            "Reset Password",
        //            $"Please reset your password by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

        //        return RedirectToPage("./ForgotPasswordConfirmation");

        //    }
        //    catch (Exception e)
        //    {
        //        _logger.LogError($"ERROR: {e.Message} | {DateTime.Now:F}");
        //        return false;
        //    }
        //}

        [HttpPost]
        [Route("/signin")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<bool> Signin([FromBody] UserSignInDTO userDTO)
        {
            try
            {
                var user = new BiscoitoAPIUser()
                {
                    UserName = userDTO.Email, 
                    Email = userDTO.Email 
                };

                var result = await _userManager.CreateAsync(user, userDTO.Password);
                if (result.Succeeded)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code, returnUrl = "" },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(userDTO.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError($"ERROR: {e.Message} | {DateTime.Now:F}");
                return false;
            }

            return false;
        }




    }
}
