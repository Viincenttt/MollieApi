namespace Mollie.Api.Client.Abstract {
    public interface IMollieClient : ICustomerClient, IIssuerClient, IMandateClient, IPaymentClient, IPaymentMethodClient, IRefundClient, ISubscriptionClient {
        
    }
}