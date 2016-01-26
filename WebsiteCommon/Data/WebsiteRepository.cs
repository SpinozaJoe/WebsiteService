using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebsiteCommon.Applications;
using WebsiteCommon.Client;

namespace WebsiteCommon.Data
{
    public class WebsiteRepository : IWebsiteRepository
    {
        private WebsiteDataContext m_context;

        public WebsiteRepository(WebsiteDataContext context)
        {
            m_context = context;
        }

        public IQueryable<Customer> GetCustomers()
        {
            return m_context.Customers.Include("ContactDetails").Include("Applications");
        }

        public IQueryable<Customer> GetSortedCustomers()
        {
            return m_context.Customers.OrderBy(c => c.LastName)
                .ThenByDescending(c=> c.FirstName);
        }

        public Customer GetCustomer(int id)
        {
            Customer result = null;

            result = m_context.Customers.Include("ContactDetails").Include("Applications")
                .FirstOrDefault(c => c.Id == id);

            return result;
        }

        public IQueryable<Application> GetAllApplications()
        {
            return m_context.Applications;
        }

        public IQueryable<Application> GetApplicationsForCustomer(int customerId)
        {
            return m_context.Applications.Where(a => a.CustomerId == customerId);
        }

        public Application GetApplication(int applicationId)
        {
            return m_context.Applications.FirstOrDefault(a => a.Id == applicationId);
        }


        public bool AddCustomer(Customer customer)
        {
            try
            {
                m_context.Customers.Add(customer);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteCustomer(int customerId)
        {
            try
            {
                Customer customer = GetCustomer(customerId);

                if (customer != null)
                {
                    m_context.Customers.Remove(customer);
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Save()
        {
            try
            {
                return m_context.SaveChanges() > 0;
            }
            catch (Exception)
            {
                // TODO: log this
                return false;
            }
        }

    }
}
