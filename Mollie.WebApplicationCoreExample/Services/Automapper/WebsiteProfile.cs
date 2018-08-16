using System.Globalization;
using AutoMapper;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.List.Specific;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Services.Automapper {
    public class WebsiteProfile : Profile {
        public WebsiteProfile() {
            this.CreateMap<CreatePaymentModel, PaymentRequest>()
                .ForMember(x => x.Amount, m => m.MapFrom(x => new Amount(x.Currency, x.Amount.ToString(CultureInfo.InvariantCulture))));

            this.CreateMap<ListResponse<PaymentListData>, OverviewModel<PaymentResponse>>()
                .ForMember(x => x.Items, m => m.MapFrom(x => x.Embedded.Items))
                .ForMember(x => x.Navigation, m => m.MapFrom(x => new OverviewNavigationLinks(x.Links.Previous, x.Links.Next)));
        }
    }
}