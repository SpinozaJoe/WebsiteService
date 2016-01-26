using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebsiteCommon.Client
{
    public class ContactDetails
    {
// To keep Annotations out of this class they are defined in the DataContext instead
//        [Key, ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public string EmailAddress { get; set; }
        public string HomePhoneNumber { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
