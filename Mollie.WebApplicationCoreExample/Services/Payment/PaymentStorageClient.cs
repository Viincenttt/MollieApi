using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Payment.Request;
using Mollie.WebApplicationCoreExample.Models;

namespace Mollie.WebApplicationCoreExample.Services.Payment {
    public interface IPaymentStorageClient {
        Task Create(CreatePaymentModel model);
    }

    public class PaymentStorageClient : IPaymentStorageClient {
        private readonly IPaymentClient _paymentClient;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public PaymentStorageClient(IPaymentClient paymentClient, IMapper mapper, IConfiguration configuration) {
            this._paymentClient = paymentClient;
            this._mapper = mapper;
            this._configuration = configuration;
        }

        public async Task Create(CreatePaymentModel model) {
            PaymentRequest paymentRequest = this._mapper.Map<PaymentRequest>(model);
            paymentRequest.RedirectUrl = this._configuration["DefaultRedirectUrl"];

            await this._paymentClient.CreatePaymentAsync(paymentRequest);
        }
    }
}