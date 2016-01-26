using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebsiteService.Services.Email
{
    public interface IEmailServiceSettings
    {
        string SmtpUrl { get; }
        string UserName { get; }
        string Password { get; }
    }
}
