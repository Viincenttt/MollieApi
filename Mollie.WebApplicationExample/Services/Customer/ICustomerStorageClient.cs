using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Customer; 

public interface ICustomerStorageClient {
    Task Create(CreateCustomerModel model);
}