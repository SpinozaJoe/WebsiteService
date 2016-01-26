using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebsiteService.Models
{
    public class EmailParameters
    {
        public int CustomerId { get; set; }
        public string ApplicationName { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}