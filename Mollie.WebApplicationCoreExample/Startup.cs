using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mollie.WebApplicationCoreExample.Middleware;

namespace Mollie.WebApplicationCoreExample {
    public class Startup {
        private readonly IConfiguration _configuration;
        public Startup(IConfiguration configuration) {
            this._configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMollieApi(this._configuration["MollieApiKey"]);

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            // TODO: Add middleware to validate Mollie API key and display an error if a live/invalid api key is passed

            // TODO: Add middleware to catch all mollie API exceptions

            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}