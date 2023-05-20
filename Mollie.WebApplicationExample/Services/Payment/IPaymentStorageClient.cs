using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Payment; 

public interface IPaymentStorageClient {
    Task Create(CreatePaymentModel model);
}