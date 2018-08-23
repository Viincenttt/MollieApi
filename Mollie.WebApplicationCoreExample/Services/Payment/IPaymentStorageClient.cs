using System.Threading.Tasks;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Services.Payment {
    public interface IPaymentStorageClient {
        Task Create(CreatePaymentModel model);
    }
}