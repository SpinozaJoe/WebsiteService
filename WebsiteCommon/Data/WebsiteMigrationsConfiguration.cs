using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteCommon.Applications;
using WebsiteCommon.Client;

namespace WebsiteCommon.Data
{
    public class WebsiteMigrationsConfiguration
        : DbMigrationsConfiguration<WebsiteDataContext>
    {
        public WebsiteMigrationsConfiguration()
        {
#if DEBUG
            this.AutomaticMigrationDataLossAllowed = true;
#endif

            this.AutomaticMigrationsEnabled = true;
        }

        protected override void Seed(WebsiteDataContext context)
        {
            // Called every time a new app domain is created
            base.Seed(context);

            // Can create dummy data for debug purposes
#if DEBUG
            if (context.Customers.Count() == 0)
            {
                // Add some for test
                Customer customer = new Customer()
                {
                    FirstName = "Joe",
                    LastName = "Bloggs",
                    ContactDetails = new ContactDetails() {
                        EmailAddress = "fakeemail@thing",
                        HomePhoneNumber = "01245343434"
                    }
                };

                context.Customers.AddOrUpdate(c => new {c.FirstName, c.LastName}, customer);

                context.SaveChanges();
            }

#endif
        }

    }
}
