using System.Threading.Tasks;
using AutoMapper;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Url;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Services {
    public abstract class OverviewClientBase<T> where T : IResponseObject {
        private readonly IMapper _mapper;

        protected OverviewClientBase(IMapper mapper) {
            this._mapper = mapper;
        }

        protected OverviewModel<T> Map(ListResponse<T> list) {
            return this._mapper.Map<OverviewModel<T>>(list);
        }

        protected UrlObjectLink<ListResponse<T>> CreateUrlObject(string url) {
            return new UrlObjectLink<ListResponse<T>> {
                Href = url
            };
        }
    }
}