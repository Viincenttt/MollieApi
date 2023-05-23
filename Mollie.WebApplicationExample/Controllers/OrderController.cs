using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models;
using Mollie.Api.Models.Order;
using Mollie.Api.Models.Payment;
using Mollie.WebApplicationExample.Models;
using Mollie.WebApplicationExample.Services.Order;

namespace Mollie.WebApplicationExample.Controllers; 

public class OrderController : Controller {
    private readonly IOrderOverviewClient _orderOverviewClient;
    private readonly IOrderStorageClient _orderStorageClient;

    public OrderController(IOrderOverviewClient orderOverviewClient, IOrderStorageClient orderStorageClient) {
        _orderOverviewClient = orderOverviewClient;
        _orderStorageClient = orderStorageClient;
    }
    
    [HttpGet]
    public async Task<ViewResult> Index() {
        OverviewModel<OrderResponse> model = await this._orderOverviewClient.GetList();
        return this.View(model);
    }
    
    [HttpGet]
    public async Task<ViewResult> ApiUrl([FromQuery]string url) {
        OverviewModel<OrderResponse> model = await this._orderOverviewClient.GetListByUrl(url);
        return this.View(nameof(this.Index), model);
    }
    
    [HttpGet]
    public ViewResult Create() {
        CreateOrderModel model = new CreateOrderModel() {
            Amount = 100,
            Locale = Locale.nl_NL,
            Currency = Currency.EUR,
            RedirectUrl = "https://www.google.com",
            OrderNumber = Guid.NewGuid().ToString(),
            Lines = new List<CreateOrderLineModel> {
                new CreateOrderLineModel {
                    Name = "A box of chocolates",
                    Quantity = 1,
                    UnitPrice = 100,
                    VatRate = "21.00",
                    VatAmount = 17.36m,
                    TotalAmount = 100
                }
            },
            BillingAddress = new CreateOrderBillingAddressModel {
                GivenName = "John",
                FamilyName = "Smit",
                Email = "johnsmit@gmail.com",
                City = "Rotterdam",
                Country = "NL",
                StreetAndNumber = "Coolsingel 1",
                PostalCode = "0000XX"
            }
        };

        return this.View(model);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateOrderModel model) {
        if (!this.ModelState.IsValid) {
            return this.View();
        }

        await this._orderStorageClient.Create(model);
        return this.RedirectToAction(nameof(this.Index));
    }
}