namespace Mollie.Api.Client {
    public interface IMollieClient : ICustomerClient, IIssuerClient, IMandateClient, IPaymentClient, IPaymentMethodClient, IRefundClient, ISubscriptionClient {
        
    }
}