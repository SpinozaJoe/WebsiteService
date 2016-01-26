using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebsiteAdmin.Models;
using WebsiteCommon.Applications;
using WebsiteCommon.Client;
using WebsiteCommon.Data;
using WebsiteCommon.Logging;

namespace WebsiteAdmin.Controllers
{
    public class HomeController : Controller
    {
        private IWebsiteRepository m_repository;
        private ILogRepository m_logRepository;
        private IActionLogger m_actionLogger;

        public HomeController(IWebsiteRepository repository, ILogRepository logRepository, IActionLogger actionLogger)
        {
            m_repository = repository;
            m_actionLogger = actionLogger;
            m_logRepository = logRepository;
        }

        private void logMessage(string message, string tag)
        {
            m_actionLogger.writeLog("WebsiteAdmin", Request.Headers.GetValues("Origin").First<string>(),
                Request.UserHostAddress, ActionType.WebAction, message, new List<string> { tag });
        }

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Dashboard()
        {
            ViewBag.NumCustomers = m_repository.GetCustomers().Count();
            ViewBag.NumNewActions = m_logRepository.GetLogsFromDate(DateTime.Today).Count();

            return View();
        }

        public ActionResult ActionLogs()
        {
            return View(m_logRepository.GetLogsFromDate(DateTime.Today));
        }

        public ActionResult Customers()
        {
            return View(m_repository.GetCustomers());
        }

        [HttpGet]
        public ActionResult NewCustomer()
        {
            return View(new CustomerViewModel());
        }

        [HttpPost]
        public ActionResult NewCustomer(CustomerViewModel customerModel)
        {
            if (ModelState.IsValid)
            {
                Customer customer = new Customer()
                {
                    FirstName = customerModel.FirstName,
                    LastName = customerModel.LastName,
                    ContactDetails = new ContactDetails()
                    {
                        EmailAddress = customerModel.EmailAddress,
                        HomePhoneNumber = customerModel.HomePhoneNumber
                    }
                };

                m_repository.AddCustomer(customer);
                m_repository.Save();

                return Redirect("~/Home/Customers");
            }
            else
            {
                return View(customerModel);
            }
        }

        [HttpGet]
        public ActionResult EditCustomer(int id)
        {
            return View(new CustomerViewModel(m_repository.GetCustomer(id)));
        }

        [HttpPost]
        public ActionResult EditCustomer(int id, CustomerViewModel customerModel)
        {
            if (ModelState.IsValid)
            {
                Customer customer = m_repository.GetCustomer(id);

                if (customer != null)
                {
                    customer.FirstName = customerModel.FirstName;
                    customer.LastName = customerModel.LastName;
                    customer.ContactDetails.EmailAddress = customerModel.EmailAddress;
                    customer.ContactDetails.HomePhoneNumber = customerModel.HomePhoneNumber;

                    m_repository.Save();

                    logMessage("Edited user " + customer.FullName, "EditUser");

                    return Redirect("~/Home/Customers");
                }
                else
                {
                    ModelState.AddModelError("NoCustomer", "Could not find the customer being edited.");
                }
            }

            return View(customerModel);
        }

        [HttpPost]
        public ActionResult DeleteCustomer(int customerId)
        {
            m_repository.DeleteCustomer(customerId);
            m_repository.Save();

            return View();
        }

        private Customer setCustomerViewBag(int customerId)
        {
            Customer customer = m_repository.GetCustomer(customerId);

            if (customer != null)
            {
                ViewBag.CustomerName = customer.FullName;
                ViewBag.CustomerId = customer.Id;
            }
            else
            {
                ViewBag.CustomerName = "Unknown";
            }

            return customer;
        }

        [HttpGet]
        public ActionResult CustomerApps(int id)
        {
            setCustomerViewBag(id);

            return View(m_repository.GetApplicationsForCustomer(id));
        }

        [HttpGet]
        public ActionResult NewApplication(int id)
        {
            setCustomerViewBag(id);

            return View(new ApplicationViewModel());
        }

        [HttpPost]
        public ActionResult NewApplication(int id, ApplicationViewModel appModel)
        {
            Customer customer = setCustomerViewBag(id);

            if (ModelState.IsValid)
            {
                // TODO: Verify the fields have valid data
                if (customer != null)
                {
                    if (appModel.SelectedApplicationType == ApplicationType.Email)
                    {
                        // Add the Web Application
                        customer.Applications.Add(new EmailApplication()
                        {
                            ApplicationType = appModel.SelectedApplicationType,
                            Name = appModel.Name,
                            OriginUrls = appModel.OriginUrls,
                            IPAccessRestrictions = appModel.IPAccessRestrictions,
                            ToAddresses = appModel.IPAccessRestrictions
                        });

                        m_repository.Save();

                        return Redirect("~/Home/CustomerApps/" + customer.Id);
                    }
                }
            }

            return View(appModel);
        }

        [HttpGet]
        public ActionResult EditApplication(int id)
        {
            Application app = m_repository.GetApplication(id);

            setCustomerViewBag(app.CustomerId);

            return View(new ApplicationViewModel(app));
        }

        [HttpPost]
        public ActionResult EditApplication(int id, ApplicationViewModel appModel)
        {
            Application app = m_repository.GetApplication(id);
            Customer customer = setCustomerViewBag(app.CustomerId);

            if (ModelState.IsValid)
            {
                // TODO: Verify the fields have valid data
                if (customer != null)
                {
                    app.ApplicationType = appModel.SelectedApplicationType;
                    app.Name = appModel.Name;
                    app.OriginUrls = appModel.OriginUrls;
                    app.IPAccessRestrictions = appModel.IPAccessRestrictions;

                    if (app.ApplicationType == ApplicationType.Email)
                    {
                        EmailApplication webApp = (EmailApplication)app;

                        webApp.ToAddresses = appModel.ToAddresses;
                    }

                    m_repository.Save();

                    return Redirect("~/Home/CustomerApps/" + customer.Id);
                }
            }

            return View(appModel);
        }

        public ActionResult EmailSettings()
        {
            ViewBag.Message = "Email settings.";

            return View(m_repository.GetAllApplications());
        }
    }
}
