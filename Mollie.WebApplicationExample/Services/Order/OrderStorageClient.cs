using AutoMapper;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Order;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Order;

public class OrderStorageClient : IOrderStorageClient {
    private readonly IOrderClient _orderClient;
    private readonly IMapper _mapper;

    public OrderStorageClient(IOrderClient orderClient, IMapper mapper) {
        this._orderClient = orderClient;
        this._mapper = mapper;
    }

    public async Task Create(CreateOrderModel model) {
        OrderRequest orderRequest = this._mapper.Map<OrderRequest>(model);

        await this._orderClient.CreateOrderAsync(orderRequest);
    }
}