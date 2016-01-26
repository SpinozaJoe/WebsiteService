using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using WebsiteCommon.Client;
using WebsiteCommon.Applications;
using System.Data.Entity.ModelConfiguration;

namespace WebsiteCommon.Data
{
    public class WebsiteDataContext : DbContext
    {
        public WebsiteDataContext()
            : base("DefaultConnection")
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;

            Database.SetInitializer(
                new MigrateDatabaseToLatestVersion<WebsiteDataContext, WebsiteMigrationsConfiguration>());
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ContactDetailsMappings());
            modelBuilder.Configurations.Add(new CustomerMappings());

            base.OnModelCreating(modelBuilder);
        }
    }

    /// <summary>
    /// Controls entity configurations for the Customer class, e.g. foreign keys, column names etc
    /// </summary>
    public class CustomerMappings : EntityTypeConfiguration<Customer>
    {
        public CustomerMappings()
        {
            // Primary Key
            this.HasKey(c => c.Id);

            // Non null columns
            this.Property(c => c.FirstName).IsRequired();
            this.Property(c => c.LastName).IsRequired();

            // Max length properties
            this.Property(c => c.FirstName).HasMaxLength(256);
            this.Property(c => c.LastName).HasMaxLength(256);

            // Don't add this to the database
            this.Ignore(c => c.FullName);
        }
    }

    /// <summary>
    /// Controls entity configurations for the ContactDetails class, e.g. foreign keys, column names etc
    /// </summary>
    public class ContactDetailsMappings : EntityTypeConfiguration<ContactDetails>
    {
        public ContactDetailsMappings()
        {
            // Primary Key
            this.HasKey(c => c.CustomerId);

            // Foreign Key to Customer
            this.HasRequired(c => c.Customer).WithOptional(cu => cu.ContactDetails);
        }
    }
}
