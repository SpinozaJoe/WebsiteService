using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteService.Services.Email
{
    public interface IEmailService
    {
        void sendEmail(string toAddress, string subject, string body);
    }
}
