# MollieApi

This project allows you to easily add the [Mollie payment provider](https://www.mollie.com) to your application. Mollie has excellent [documentation](https://www.mollie.com/nl/docs/overzicht) which I highly recommend you read before using this library. Please keep in mind that this is a 3rd party library and I am in no way associated with Mollie. 

If you have encounter any issues while using this library or have any feature requests, feel free to open an issue on GitHub. 

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

### Creating a API client object
Every API has it's own API client class. These are: PaymentClient, PaymentMethodClient, CustomerClient, MandateClient, SubscriptionClient, IssuerClient and RefundClient classes. 

These classes allow you to send and receive requests to the Mollie REST webservice. To create a API client class, you simple instantiate a new object for the API you require. For example, if you want to create new payments, you can use the PaymentClient class. 
```c#
PaymentClient paymentClient = new PaymentClient("{your_api_key}");
```
### Payments
#### Creating a payment
```c#
PaymentClient paymentClient = new PaymentClient("{your_api_key}");
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = 100,
    Description = "Test payment of the example project",
    RedirectUrl = "http://google.com"
};

PaymentResponse paymentResponse = paymentClient.CreatePaymentAsync(paymentRequest).Result;
```

If you want to create a payment with a specific paymentmethod, there are seperate classes that allow you to set paymentmethod specific parameters. For example, a bank transfer payment allows you to set the billing e-mail and due date. Have a look at the [Mollie create payment documentation](https://www.mollie.com/nl/docs/reference/payments/create) for more information. 

The full list of payment specific request classes is:
- BankTransferPaymentRequest
- CreditCardPaymentRequest
- IDealPaymentRequest
- PayPalPaymentRequest
- SepaDirectDebitRequest
- KbcPaymentRequest

#### Retrieving a payment by id
```c#
PaymentClient paymentClient = new PaymentClient("{your_api_key}");
PaymentResponse result = paymentClient.GetPaymentAsync(paymentResponse.Id).Result;
```

Keep in mind that some payment methods have specific payment detail values. For example: PayPal payments have reference and customer reference properties. In order to access these properties you have to cast the PaymentResponse to the PayPalPaymentResponse and access the Detail property. 

Take a look at the [Mollie payment response documentation](https://www.mollie.com/nl/docs/reference/payments/get) for a full list of payment methods that have extra detail fields.

The full list of payment specific response classes is:
- BankTransferPaymentResponse
- BelfiusPaymentResponse
- BitcoinPaymentResponse
- CreditCardPaymentResponse
- IdealPaymentResponse
- KbcPaymentResponse
- MisterCashPaymentResponse
- PayPalPaymentResponse
- PaySafeCardPaymentResponse
- PodiumCadeauKaartPaymentResponse
- SepaDirectDebitResponse
- SofortPaymentResponse

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
ListResponse<PaymentMethodResponse> paymentMethodList = paymentMethodClient.GetPaymentMethodListAsync(offset, count).Result;
foreach (PaymentMethodResponse paymentMethod in paymentMethodList.Data) {
  // Your code here
}
```
#### Retrieving a single payment method
```c#
PaymentMethodClient _paymentMethodClient = new PaymentMethodClient("{your_api_key}");
PaymentMethodResponse paymentMethodResponse = paymentMethodClient.GetPaymentMethodAsync(paymentMethod).Result;
```

### Issuer methods
#### Retrieve issuer list
```c#
IssuerClient issuerClient = new IssuerClient("{your_api_key}");
ListResponse<IssuerResponse> issuerList = this.issuerClient.GetIssuerListAsync().Result;
foreach (IssuerResponse issuer in issuerList.Data) {
    // Your code here
}
```

#### Retrieve a single issuer by id
```c#
IssuerClient issuerClient = new IssuerClient("{your_api_key}");
issuerClient.GetIssuerAsync(issuerId).Result;
```

### Refund methods
#### Create a new refund
```c#
RefundClient refundClient = new RefundClient("{your_api_key}");
RefundResponse refundResponse = refundClient.CreateRefund(payment.Id).Result;
```

#### Create a partial refund
```c#
RefundClient refundClient = new RefundClient("{your_api_key}");
RefundRequest refundRequest = new RefundRequest() {
    Amount = 50
};
RefundResponse refundResponse = this.refundClient.CreateRefund(payment.Id, refundRequest).Result;
```

#### Retrieve a refund by payment and refund id
```c#
RefundClient refundClient = new RefundClient("{your_api_key}");
RefundResponse refundResponse = refundClient.GetRefund(payment.Id, refundResponse.Id).Result;
```

#### Retrieve refund list
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
RefundClient refundClient = new RefundClient("{your_api_key}");
ListResponse<RefundResponse> refundList = refundClient.GetRefundList(payment.Id, offset, count).Result;
```

#### Cancel a refund
```c#
RefundClient refundClient = new RefundClient("{your_api_key}");
refundClient.CancelRefund(paymentId, refundId);
```
