using AutoMapper;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Payment.Request;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Payment; 

public class PaymentStorageClient : IPaymentStorageClient {
    private readonly IPaymentClient _paymentClient;
    private readonly IMapper _mapper;

    public PaymentStorageClient(IPaymentClient paymentClient, IMapper mapper) {
        this._paymentClient = paymentClient;
        this._mapper = mapper;
    }

    public async Task Create(CreatePaymentModel model) {
        PaymentRequest paymentRequest = this._mapper.Map<PaymentRequest>(model);

        await this._paymentClient.CreatePaymentAsync(paymentRequest);
    }
}