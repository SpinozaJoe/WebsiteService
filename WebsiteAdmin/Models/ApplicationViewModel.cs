using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using WebsiteCommon.Applications;

namespace WebsiteAdmin.Models
{
    public class ApplicationViewModel
    {
        public ApplicationViewModel()
        {
            initDropDown();

            SelectedApplicationType = ApplicationType.Email;
        }

        public ApplicationViewModel(Application app)
        {
            initDropDown();

            SelectedApplicationType = app.ApplicationType;
            Name = app.Name;
            CustomerId = app.CustomerId;
            OriginUrls = app.OriginUrls;
            IPAccessRestrictions = app.IPAccessRestrictions;

            if (app is EmailApplication)
            {
                EmailApplication webApp = app as EmailApplication;

                ToAddresses = webApp.ToAddresses;
            }
        }

        private void initDropDown()
        {
            ApplicationTypes = new SelectList(new List<SelectListItem>()
            {
                new SelectListItem() { Value = ApplicationType.Email.ToString(), Text = ApplicationType.Email.ToString() },
                new SelectListItem() { Value = ApplicationType.Blog.ToString(), Text = ApplicationType.Blog.ToString() }
            }, "Value", "Text");
        }

        public IEnumerable<SelectListItem> ApplicationTypes { get; set; }

        [Required]
        [Display(Name = "Application Type")]
        public ApplicationType SelectedApplicationType { get; set; }

        [Required]
        [Display(Name = "Application Name")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 5)]
        public string Name { get; set; }

        public int CustomerId { get; set; }

        [Display(Name = "URL List of Authorised Websites (semi-colon separated)")]
        public string OriginUrls { get; set; }

        [Display(Name = "IP Access Restrictions (semi-colon separated)")]
        public string IPAccessRestrictions { get; set; }

        [Display(Name = "Authorised Email To Addresses")]
        public string ToAddresses { get; set; }
    }
}