using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mollie.WebApplicationCoreExample.Framework.Middleware;
using Mollie.WebApplicationCoreExample.Services.Customer;
using Mollie.WebApplicationCoreExample.Services.Mandate;
using Mollie.WebApplicationCoreExample.Services.Payment;
using Mollie.WebApplicationCoreExample.Services.Payment.Refund;
using Mollie.WebApplicationCoreExample.Services.PaymentMethod;
using Mollie.WebApplicationCoreExample.Services.Subscription;

namespace Mollie.WebApplicationCoreExample {
    public class Startup {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration) {
            this._configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services) {
            services.AddMollieApi(this._configuration);
            services.AddScoped<IPaymentOverviewClient, PaymentOverviewClient>();
            services.AddScoped<ICustomerOverviewClient, CustomerOverviewClient>();
            services.AddScoped<ISubscriptionOverviewClient, SubscriptionOverviewClient>();
            services.AddScoped<IMandateOverviewClient, MandateOverviewClient>();
            services.AddScoped<IPaymentMethodOverviewClient, PaymentMethodOverviewClient>();
            services.AddScoped<IPaymentStorageClient, PaymentStorageClient>();
            services.AddScoped<ICustomerStorageClient, CustomerStorageClient>();
            services.AddScoped<ISubscriptionStorageClient, SubscriptionStorageClient>();
            services.AddScoped<IMandateStorageClient, MandateStorageClient>();
            services.AddScoped<IRefundPaymentClient, RefundPaymentClient>();

            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            app.UseDeveloperExceptionPage();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
