using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework;

namespace Mollie.WebApplicationCoreExample.Framework.Middleware {
    public static class MollieApiClientServiceExtensions {
        public static IServiceCollection AddMollieApi(this IServiceCollection services, IConfiguration configuration) {
            MollieConfiguration mollieConfiguration = configuration.GetSection("Mollie").Get<MollieConfiguration>();

            services.AddScoped<IPaymentClient, PaymentClient>();
            services.AddScoped<IPaymentClient, PaymentClient>(x => new PaymentClient(mollieConfiguration.ApiKey));
            services.AddScoped<ICustomerClient, CustomerClient>(x => new CustomerClient(mollieConfiguration.ApiKey));
            services.AddScoped<IRefundClient, RefundClient>(x => new RefundClient(mollieConfiguration.ApiKey));
            services.AddScoped<IPaymentMethodClient, PaymentMethodClient>(x => new PaymentMethodClient(mollieConfiguration.ApiKey));
            services.AddScoped<ISubscriptionClient, SubscriptionClient>(x => new SubscriptionClient(mollieConfiguration.ApiKey));
            services.AddScoped<IMandateClient, MandateClient>(x => new MandateClient(mollieConfiguration.ApiKey));
            services.AddScoped<IInvoicesClient, InvoicesClient>(x => new InvoicesClient(mollieConfiguration.ApiKey));

            return services;
        }
    }
}