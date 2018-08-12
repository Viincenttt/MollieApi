namespace Mollie.Api.Client.Abstract {
    public interface IMollieClient : ICustomerClient, IMandateClient, IPaymentClient, IPaymentMethodClient, IRefundClient, ISubscriptionClient {
    }
}