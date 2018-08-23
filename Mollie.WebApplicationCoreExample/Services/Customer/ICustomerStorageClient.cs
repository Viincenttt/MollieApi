using System.Threading.Tasks;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Services.Customer {
    public interface ICustomerStorageClient {
        Task Create(CreateCustomerModel model);
    }
}