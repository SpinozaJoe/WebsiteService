using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteService.Configuration
{
    public abstract class AbstractGlobalAppSettings
    {
        protected string getSetting(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
    }
}