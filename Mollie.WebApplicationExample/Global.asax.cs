using System.Web.Configuration;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Mollie.Api.Client;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Response;
using Mollie.WebApplicationExample.Infrastructure;

namespace Mollie.WebApplicationExample
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AppSettings.Initialize(WebConfigurationManager.AppSettings);
        }
    }
}
