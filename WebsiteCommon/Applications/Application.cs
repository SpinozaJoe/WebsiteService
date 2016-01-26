using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteCommon.Client;

namespace WebsiteCommon.Applications
{
    public class Application
    {
        public int Id { get; set; }
        public ApplicationType ApplicationType { get; set; }
        public string Name { get; set; }
        public int CustomerId { get; set; }
        public string OriginUrls { get; set; }
        public string IPAccessRestrictions { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
