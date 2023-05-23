using Mollie.Api.Models.PaymentMethod;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.PaymentMethod; 

public interface IPaymentMethodOverviewClient {
    Task<OverviewModel<PaymentMethodResponse>> GetList();
}