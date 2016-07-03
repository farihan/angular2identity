using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Hans.Angular2Identity.Infrastructure.Domains;
using Hans.Angular2Identity.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Hans.Angular2Identity.Models.AccountViewModels;
using Hans.Angular2Identity.Infrastructure.Commons;
using Hans.Angular2Identity.Infrastructure.Repositories;

namespace Hans.Angular2Identity.Controllers
{
    [Produces("application/json")]
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly SignInManager<ApplicationUser> SignInManager;
        private readonly IEmailSender EmailSender;
        private readonly IRepository<Logging> LoggingRepository;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IEmailSender emailSender,
            IRepository<Logging> loggingRepository)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            EmailSender = emailSender;
            LoggingRepository = loggingRepository;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel user)
        {
            GenericResult actionResult = null;

            try
            {
                if (ModelState.IsValid)
                {
                    var result = await SignInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        actionResult = new GenericResult() { Succeeded = true, Message = ResultType.LoginSucceed };
                    }
                    if (result.IsLockedOut)
                    {
                        actionResult = new GenericResult() { Succeeded = false, Message = ResultType.LoginAccountLockedOut };
                    }
                    else
                    {
                        actionResult = new GenericResult() { Succeeded = false, Message = ResultType.LoginFailed };
                    }
                }
                else
                {
                    actionResult = new GenericResult() { Succeeded = false, Message = ResultType.InvalidLoginAttempt };
                }
            }
            catch (Exception ex)
            {
                actionResult = new GenericResult() { Succeeded = false, Message = ex.Message };

                LoggingRepository.Add(new Logging() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                LoggingRepository.Commit();
            }

            return new ObjectResult(actionResult);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await SignInManager.SignOutAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                LoggingRepository.Add(new Logging() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                LoggingRepository.Commit();

                return BadRequest();
            }
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
        {
            GenericResult actionResult = null;

            try
            {
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser { UserName = model.Email, Email = model.Email };

                    var result = await UserManager.CreateAsync(user, model.Password);

                    if (result.Succeeded)
                    {
                        // Send an email with this link
                        //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                        //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                        //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                        await SignInManager.SignInAsync(user, isPersistent: false);

                        actionResult = new GenericResult() { Succeeded = true, Message = ResultType.RegistrationSucceeded };
                    }
                }
                else
                {
                    actionResult = new GenericResult() { Succeeded = false, Message = ResultType.InvalidLoginAttempt };
                }
            }
            catch (Exception ex)
            {
                actionResult = new GenericResult() { Succeeded = false, Message = ex.Message };

                LoggingRepository.Add(new Logging() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                LoggingRepository.Commit();
            }

            return new ObjectResult(actionResult);
        }
    }
}