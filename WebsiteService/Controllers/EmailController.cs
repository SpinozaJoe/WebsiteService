using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.ServiceModel.Channels;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using WebsiteCommon.Applications;
using WebsiteCommon.Client;
using WebsiteCommon.Data;
using WebsiteCommon.Logging;
using WebsiteService.Models;
using WebsiteService.Services.Email;

namespace WebsiteService.Controllers
{
    public enum A
    {
        B,C
    }
    public class EmailController : ApiController
    {
        private readonly IWebsiteRepository m_repository;
        private readonly IEmailService m_emailService;
        private readonly IActionLogger m_actionLogger;

        public EmailController(IWebsiteRepository repository, IEmailService emailService, IActionLogger actionLogger)
        {
            m_repository = repository;
            m_emailService = emailService;
            m_actionLogger = actionLogger;
        }

        private void logMessage(string message, string tag)
        {
            m_actionLogger.writeLog("WebsiteApi", GetOrigin(Request),
                GetClientIp(Request), ActionType.SendEmail, message, new List<string> { tag });
        }

        private string GetOrigin(HttpRequestMessage request)
        {
            return request.Headers.GetValues("Origin").First<string>();
        }

        private string GetClientIp(HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop;
                prop = (RemoteEndpointMessageProperty)this.Request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else
            {
                return null;
            }
        }

        // GET api/email
        public bool Get()
        {
            return false;
        }

        private bool isAccessAllowed(string restrictionStrings, string requestString)
        {
            bool allowed = true;

            if (!string.IsNullOrEmpty(restrictionStrings))
            {
                string[] allowedStrings = restrictionStrings.Split(';');

                allowed = allowedStrings.Contains(requestString);
            }

            return allowed;
        }

        // POST api/email
        public HttpResponseMessage Post([FromBody]EmailParameters emailParams)
        {
            HttpResponseMessage response;
            Customer customer = null;
            EmailApplication app = null;

            if (emailParams != null)
            {
                customer = m_repository.GetCustomer(emailParams.CustomerId);
            }

            if (customer == null)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Unrecognised customer");
            }
            else if (!string.IsNullOrEmpty(emailParams.ApplicationName))
            {
                app = customer.Applications.FirstOrDefault(a => a.Name.Equals(emailParams.ApplicationName, StringComparison.InvariantCultureIgnoreCase)) as EmailApplication;
//                app = m_repository.GetAllApplications().FirstOrDefault(a => a.Name.Equals(emailParams.ApplicationName, StringComparison.InvariantCultureIgnoreCase)) as EmailApplication;
            }

            if (app == null)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Unrecognised application");
            }
            else if (app.ApplicationType != ApplicationType.Email)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid application type");
            }
            else if (string.IsNullOrEmpty(app.ToAddresses))
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, "No To email address specified");
            }
            else
            {
                string origin = GetOrigin(Request);
                bool accessAllowed = isAccessAllowed(app.OriginUrls, origin);

                if (accessAllowed)
                {
                    string hostAddress = GetClientIp(Request);

                    accessAllowed = isAccessAllowed(app.IPAccessRestrictions, hostAddress);
                }

                if (accessAllowed)
                {
                    try
                    {
                        m_emailService.sendEmail(app.ToAddresses, emailParams.Subject, emailParams.Body);

                        // Log details of the message sent request
                        logMessage(string.Format("Email sent for application {0} with subject {1}", app.Name, emailParams.Subject), "EmailSent");

                        response = Request.CreateResponse(HttpStatusCode.OK, true);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);

                        response = Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
                    }
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.BadRequest, "Access denied");
                }
            }

            return response;
        }

    }
}
