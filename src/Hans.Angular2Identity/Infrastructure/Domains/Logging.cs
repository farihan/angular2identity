using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hans.Angular2Identity.Infrastructure.Domains
{
    public class Logging
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public DateTimeOffset DateCreated { get; set; }
    }
}
