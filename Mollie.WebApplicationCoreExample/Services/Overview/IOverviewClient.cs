using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Services.Overview {
    public interface IOverviewClient<T> where T : IResponseObject {
        Task<OverviewModel<T>> GetList();
        Task<OverviewModel<T>> GetList(string url);
    }
}