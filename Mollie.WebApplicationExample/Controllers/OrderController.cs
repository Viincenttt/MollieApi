using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models.Order;
using Mollie.WebApplicationExample.Models;
using Mollie.WebApplicationExample.Services.Order;

namespace Mollie.WebApplicationExample.Controllers; 

public class OrderController : Controller {
    private readonly IOrderOverviewClient _orderOverviewClient;

    public OrderController(IOrderOverviewClient orderOverviewClient) {
        _orderOverviewClient = orderOverviewClient;
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
}