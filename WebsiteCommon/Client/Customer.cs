using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteCommon.Applications;

namespace WebsiteCommon.Client
{
    public class Customer
    {
        public Customer()
        {
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ContactDetails ContactDetails { get; set; }

        public List<Application> Applications { get; set; }

        public string FullName
        {
            get { return FirstName + " " + LastName;  }
        }
    }
}
