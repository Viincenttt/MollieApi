using Mollie.Api.Models.Payment.Response;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Payment; 

public interface IPaymentOverviewClient {
    Task<OverviewModel<PaymentResponse>> GetList();
    Task<OverviewModel<PaymentResponse>> GetListByUrl(string url);
}