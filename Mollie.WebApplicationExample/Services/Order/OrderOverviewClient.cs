using AutoMapper;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Order;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Order; 

public class OrderOverviewClient : OverviewClientBase<OrderResponse>, IOrderOverviewClient {
    private readonly IOrderClient _orderClient;

    public OrderOverviewClient(IMapper mapper, IOrderClient orderClient) : base(mapper) {
        this._orderClient = orderClient;
    }

    public async Task<OverviewModel<OrderResponse>> GetList() {
        return this.Map(await this._orderClient.GetOrderListAsync());
    }

    public async Task<OverviewModel<OrderResponse>> GetListByUrl(string url) {
        return this.Map(await this._orderClient.GetOrderListAsync(this.CreateUrlObject(url)));
    }
    
}