using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using WebsiteCommon.Client;

namespace WebsiteAdmin.Models
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {
        }

        public CustomerViewModel(Customer customer)
        {
            Id = customer.Id;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            EmailAddress = customer.ContactDetails.EmailAddress;
            HomePhoneNumber = customer.ContactDetails.HomePhoneNumber;
        }

        public int Id { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Email Address")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        [Required]
        [Display(Name = "Home Phone Number")]
        [Phone]
        public string HomePhoneNumber { get; set; }
    }
}