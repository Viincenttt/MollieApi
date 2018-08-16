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

            this.CreateMap<CreatePaymentModel, Amount>()
                .ForMember(x => x.Value, m => m.MapFrom(x => x.Amount));

            this.CreateMap<ListResponse<PaymentListData>, OverviewModel<PaymentResponse>>()
                .ForMember(x => x.Items, m => m.MapFrom(x => x.Embedded.Payments))
                .ForMember(x => x.Navigation, m => m.MapFrom(x => x));

            this.CreateMap<ListResponse<PaymentListData>, OverviewNavigationLinks>()
                .ForMember(x => x.Previous, m => m.MapFrom(x => x.Links.Previous))
                .ForMember(x => x.Next, m => m.MapFrom(x => x.Links.Next));
        }
    }
}