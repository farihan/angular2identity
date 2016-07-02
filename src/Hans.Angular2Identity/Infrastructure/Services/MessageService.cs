using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.Angular2Identity.Infrastructure.Services
{
    public class MessageService : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }
}
