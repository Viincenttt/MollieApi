using Mollie.Api.Models.Mandate;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Mandate; 

public interface IMandateOverviewClient {
    Task<OverviewModel<MandateResponse>> GetList(string customerId);
    Task<OverviewModel<MandateResponse>> GetListByUrl(string url);
}