using System.Globalization;
using AutoMapper;
using Mollie.Api.Models;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Mandate;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Subscription;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Automapper; 

public class MollieApiProfile : Profile {
    public MollieApiProfile() {
        this.CreateMap<CreatePaymentModel, PaymentRequest>()
            .ForMember(x => x.Amount, m => m.MapFrom(x => new Amount(x.Currency, x.Amount.ToString(CultureInfo.InvariantCulture))));

        this.CreateMap<CreateSubscriptionModel, SubscriptionRequest>()
            .ForMember(x => x.Amount, m => m.MapFrom(x => new Amount(x.Currency, x.Amount.ToString(CultureInfo.InvariantCulture))))
            .ForMember(x => x.Interval, m => m.MapFrom(x => $"{x.IntervalAmount} {x.IntervalPeriod.ToString().ToLower()}"));

        this.CreateMap<CreateCustomerModel, CustomerRequest>();

        this.CreateOverviewMap<PaymentResponse>();
        this.CreateOverviewMap<CustomerResponse>();
        this.CreateOverviewMap<SubscriptionResponse>();
        this.CreateOverviewMap<MandateResponse>();
        this.CreateOverviewMap<PaymentMethodResponse>();
    }

    private void CreateOverviewMap<TResponseType>() where TResponseType : IResponseObject {
        this.CreateMap<ListResponse<TResponseType>, OverviewModel<TResponseType>>()
            .ForMember(x => x.Items, m => m.MapFrom(x => x.Items))
            .ForMember(x => x.Navigation, m => m.MapFrom(x => new OverviewNavigationLinksModel(x.Links.Previous, x.Links.Next)));
    }
}