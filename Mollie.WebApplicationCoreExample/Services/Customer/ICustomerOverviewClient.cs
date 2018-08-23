using System.Threading.Tasks;
using Mollie.Api.Models.Customer;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Services.Customer {
    public interface ICustomerOverviewClient {
        Task<OverviewModel<CustomerResponse>> GetList();
        Task<OverviewModel<CustomerResponse>> GetListByUrl(string url);
    }
}