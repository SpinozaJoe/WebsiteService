[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(WebsiteService.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(WebsiteService.App_Start.NinjectWebCommon), "Stop")]

namespace WebsiteService.App_Start
{
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;

    using Ninject;
    using Ninject.Web.Common;
    using WebsiteCommon.Data;
    using System.Web.Http;
    using WebApiContrib.IoC.Ninject;
    using WebsiteService.Models;
    using WebsiteService.Services.Email;
    using WebsiteCommon.Logging;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);

                // This is required to allow web api to use the registered services
                GlobalConfiguration.Configuration.DependencyResolver = new NinjectResolver(kernel);
                
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        
        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<WebsiteDataContext>().To<WebsiteDataContext>().InRequestScope();
            kernel.Bind<IWebsiteRepository>().To<WebsiteRepository>().InRequestScope();

            kernel.Bind<IEmailServiceSettings>().To<SmtpEmailServiceSettings>().InSingletonScope();
            kernel.Bind<IEmailService>().To<SmtpEmailService>().InSingletonScope();

            kernel.Bind<LogDataContext>().To<LogDataContext>().InRequestScope();
            kernel.Bind<ILogRepository>().To<LogRepository>().InRequestScope();

            kernel.Bind<IActionLogger>().To<DatabaseActionLogger>().InRequestScope();
        }        
    }
}
