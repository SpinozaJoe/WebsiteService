using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteCommon.Applications;
using WebsiteCommon.Client;

namespace WebsiteCommon.Data
{
    public interface IWebsiteRepository
    {
        IQueryable<Customer> GetCustomers();
        IQueryable<Customer> GetSortedCustomers();
        Customer GetCustomer(int id);
        bool AddCustomer(Customer customer);
        bool DeleteCustomer(int customerId);

        IQueryable<Application> GetAllApplications();
        IQueryable<Application> GetApplicationsForCustomer(int customerId);
        Application GetApplication(int applicationId);

        bool Save();
    }
}
