using System.Threading.Tasks;
using Mollie.Api.Models.Mandate;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Services.Mandate {
    public interface IMandateOverviewClient {
        Task<OverviewModel<MandateResponse>> GetList(string customerId);
        Task<OverviewModel<MandateResponse>> GetListByUrl(string url);
    }
}