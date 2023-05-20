using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models.PaymentMethod;
using Mollie.WebApplicationExample.Models;
using Mollie.WebApplicationExample.Services.PaymentMethod;

namespace Mollie.WebApplicationExample.Controllers; 

public class PaymentMethodController : Controller {
    private readonly IPaymentMethodOverviewClient _paymentMethodOverviewClient;

    public PaymentMethodController(IPaymentMethodOverviewClient paymentMethodOverviewClient) {
        this._paymentMethodOverviewClient = paymentMethodOverviewClient;
    }

    public async Task<ViewResult> Index() {
        OverviewModel<PaymentMethodResponse> model = await this._paymentMethodOverviewClient.GetList();
        return this.View(model);
    }
}