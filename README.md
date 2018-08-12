# MollieApi

This project allows you to easily add the [Mollie payment provider](https://www.mollie.com) to your application. Mollie has excellent [documentation](https://www.mollie.com/nl/docs/overzicht) which I highly recommend you read before using this library. Please keep in mind that this is a 3rd party library and I am in no way associated with Mollie. 

If you have encounter any issues while using this library or have any feature requests, feel free to open an issue on GitHub. 

## Mollie API v1 and v2
The API client uses version 2 of the Mollie API. If you want to use version 1 of the Mollie API, you can use version 1.5.2 of the Mollie API Nuget Package. Mollie API client 2.0.0+ is not backwards compatible with version 1.5.2 or lower. Please take a look at the [Mollie migration guide](https://docs.mollie.com/payments/migrating-v1-to-v2) for assistence.

## Getting started

### Installing the library
The easiest way to install the Mollie Api library is to use the [Nuget Package](https://www.nuget.org/packages/Mollie.Api).
```
Install-Package Mollie.Api
```

### Example projects
An example web application project that allows you to view, create, pay and refund payments is included. In order to use this project you have to set your Mollie API key in the web.config file. 

### Supported API's
This library currently supports the following API's:
- Payments API
- PaymentMethod
- Customers API
- Mandates API
- Subscriptions API
- Issuer API
- Refund API
- Connect API
- Chargebacks API
- Invoices API
- Permissions API
- Profiles API
- Organisations API

### Creating a API client object
Every API has it's own API client class. For example: PaymentClient, PaymentMethodClient, CustomerClient, MandateClient, SubscriptionClient, IssuerClient and RefundClient classes. All of these API client classes also have their own interface. 

These client API classes allow you to send and receive requests to the Mollie REST webservice. To create a API client class, you simple instantiate a new object for the API you require. For example, if you want to create new payments, you can use the PaymentClient class. 
```c#
IPaymentClient paymentClient = new PaymentClient("{your_api_key}");
```
### Payments
#### Creating a payment
```c#
IPaymentClient paymentClient = new PaymentClient("{your_api_key}");
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = new Amount(Currency.EUR, "100.00"),
    Description = "Test payment of the example project",
    RedirectUrl = "http://google.com"
};

PaymentResponse paymentResponse = paymentClient.CreatePaymentAsync(paymentRequest).Result;
```

If you want to create a payment with a specific paymentmethod, there are seperate classes that allow you to set paymentmethod specific parameters. For example, a bank transfer payment allows you to set the billing e-mail and due date. Have a look at the [Mollie create payment documentation](https://www.mollie.com/nl/docs/reference/payments/create) for more information. 

The full list of payment specific request classes is:
- BankTransferPaymentRequest
- BitcoinPaymentRequest
- CreditCardPaymentRequest
- GiftcardPaymentRequest
- IDealPaymentRequest
- KbcPaymentRequest
- PayPalPaymentRequest
- PaySafeCardPaymentRequest
- SepaDirectDebitRequest


#### Retrieving a payment by id
```c#
PaymentClient paymentClient = new PaymentClient("{your_api_key}");
PaymentResponse result = paymentClient.GetPaymentAsync(paymentResponse.Id).Result;
```

Keep in mind that some payment methods have specific payment detail values. For example: PayPal payments have reference and customer reference properties. In order to access these properties you have to cast the PaymentResponse to the PayPalPaymentResponse and access the Detail property. 

Take a look at the [Mollie payment response documentation](https://www.mollie.com/nl/docs/reference/payments/get) for a full list of payment methods that have extra detail fields.

The full list of payment specific response classes is:
- BancontactPaymentResponse
- BankTransferPaymentResponse
- BelfiusPaymentResponse
- BitcoinPaymentResponse
- CreditCardPaymentResponse
- GiftcardPaymentResponse
- IdealPaymentResponse
- IngHomePayPaymentResponse
- KbcPaymentResponse
- PayPalPaymentResponse
- PaySafeCardPaymentResponse
- SepaDirectDebitResponse
- SofortPaymentResponse

#### Setting metadata
Mollie allows you to send any metadata you like in JSON notation and will save the data alongside the payment. When you fetch the payment with the API, Mollie will include the metadata. The library allows you to set the metadata JSON string manually, by setting the Metadata property of the PaymentRequest class, but the recommended way of setting/getting the metadata is to use the SetMetadata/Getmetadata methods. 

For example: 
```c#
// Custom metadata class that contains the data you want to include in the metadata class. 
CustomMetadataClass metadataRequest = new CustomMetadataClass() {
    OrderId = 1,
    Description = "Custom description"
};

// Create a new payment
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = new Amount(Currency.EUR, "100.00"),
    Description = "Description",
    RedirectUrl = this.DefaultRedirectUrl,
};

// Set the metadata
paymentRequest.SetMetadata(metadataRequest);

// When we retrieve the payment response, we can convert our metadata back to our custom class
PaymentResponse result = await this._paymentClient.CreatePaymentAsync(paymentRequest);
CustomMetadataClass metadataResponse = result.GetMetadata<CustomMetadataClass>();
```

#### Retrieving a list off payments
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional. The maximum number of payments you can request in a single roundtrip is 250. 
```c#
PaymentClient paymentClient = new PaymentClient("{your_api_key}");
ListResponse<PaymentResponse> response = paymentClient.GetPaymentListAsync(offset, count).Result;
```

### Payment methods
#### Retrieving a list of all payment methods
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
PaymentMethodClient _paymentMethodClient = new PaymentMethodClient("{your_api_key}");
ListResponse<PaymentMethodListData> paymentMethodList = await this._paymentMethodClient.GetPaymentMethodListAsync();
foreach (PaymentMethodResponse paymentMethod in paymentMethodList.Embedded.Methods) {
	// Your code here
}
```
#### Retrieving a single payment method
```c#
PaymentMethodClient _paymentMethodClient = new PaymentMethodClient("{your_api_key}");
PaymentMethodResponse paymentMethodResponse = paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Ideal).Result;
```

### Refund methods
#### Create a new refund
```c#
RefundClient refundClient = new RefundClient("{your_api_key}");
RefundResponse refundResponse = await this._refundClient.CreateRefundAsync("test", new RefundRequest() {
	Amount = new Amount(Currency.EUR, "100"),
	Description = "Refund description"
});
```

#### Retrieve a refund by payment and refund id
```c#
RefundClient refundClient = new RefundClient("{your_api_key}");
RefundResponse refundResponse = await this._refundClient.GetRefundAsync(paymentId, refundId);
```

#### Retrieve refund list
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
RefundClient refundClient = new RefundClient("{your_api_key}");
ListResponse<RefundListData> refundList = await this._refundClient.GetRefundListAsync(payment.Id, offset, count);
```

#### Cancel a refund
```c#
RefundClient refundClient = new RefundClient("{your_api_key}");
await refundClient.CancelRefundAsync(paymentId, refundId);
```
