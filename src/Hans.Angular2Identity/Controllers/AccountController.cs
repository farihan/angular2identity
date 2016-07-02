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
            GenericResult loginResult = null;

            try
            {
                if (ModelState.IsValid)
                {
                    var login = await SignInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, lockoutOnFailure: false);

                    if (login.Succeeded)
                    {
                        loginResult = new GenericResult() { Succeeded = true, Message = ResultType.LoginSucceed };
                    }
                    if (login.IsLockedOut)
                    {
                        loginResult = new GenericResult() { Succeeded = false, Message = ResultType.LoginAccountLockedOut };
                    }
                    else
                    {
                        loginResult = new GenericResult() { Succeeded = false, Message = ResultType.LoginFailed };
                    }
                }
                else
                {
                    loginResult = new GenericResult() { Succeeded = false, Message = ResultType.InvalidLoginAttempt };
                }
            }
            catch (Exception ex)
            {
                loginResult = new GenericResult() { Succeeded = false, Message = ex.Message };

                LoggingRepository.Add(new Logging() { Message = ex.Message, StackTrace = ex.StackTrace, DateCreated = DateTime.Now });
                LoggingRepository.Commit();
            }

            return new ObjectResult(loginResult);
        }
    }
}