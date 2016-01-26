using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebsiteService.Configuration;

namespace WebsiteService.Services.Email
{
    public class SmtpEmailServiceSettings : AbstractGlobalAppSettings, IEmailServiceSettings
    {
        private const string APP_KEY_SMTP_URL = "smtpUrl";
        private const string APP_KEY_SMTP_USER_NAME = "smtpUserName";
        private const string APP_KEY_SMTP_PASSWORD = "smtpPassword";

        public string SmtpUrl
        {
            get { return getSetting(APP_KEY_SMTP_URL); }
        }

        public string UserName
        {
            get { return getSetting(APP_KEY_SMTP_USER_NAME); }
        }

        public string Password
        {
            get { return getSetting(APP_KEY_SMTP_PASSWORD); }
        }
    }
}