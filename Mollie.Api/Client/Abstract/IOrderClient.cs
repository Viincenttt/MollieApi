using System.Threading.Tasks;
using Mollie.Api.Models.Order;

namespace Mollie.Api.Client.Abstract {
    public interface IOrderClient {
        Task<OrderResponse> CreateOrderAsync(OrderRequest orderRequest);
    }
}