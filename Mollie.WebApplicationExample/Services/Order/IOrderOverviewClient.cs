using Mollie.Api.Models.Order;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Order; 

public interface IOrderOverviewClient {
    Task<OverviewModel<OrderResponse>> GetList();
    Task<OverviewModel<OrderResponse>> GetListByUrl(string url);
}