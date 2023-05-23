using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Order; 

public interface IOrderStorageClient {
    Task Create(CreateOrderModel model);
}