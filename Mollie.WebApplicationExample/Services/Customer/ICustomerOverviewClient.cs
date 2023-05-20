using Mollie.Api.Models.Customer;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Customer; 

public interface ICustomerOverviewClient {
    Task<OverviewModel<CustomerResponse>> GetList();
    Task<OverviewModel<CustomerResponse>> GetListByUrl(string url);
}