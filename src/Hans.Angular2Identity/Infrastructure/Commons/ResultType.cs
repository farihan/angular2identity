using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.Angular2Identity.Infrastructure.Commons
{
    public class ResultType
    {
        public const string LoginSucceed = "Login succeed";
        public const string LoginFailed = "Login failed";
        public const string LoginAccountLockedOut = "Login account locked out";
        public const string InvalidLoginAttempt = "Invalid login attempt";
        public const string InvalidLoginField = "Invalid login field"; 
        public const string RegistrationSucceeded = "Registration succeeded";
    }
}
