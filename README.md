# MollieApi

This project allows you to easily add the [Mollie payment provider](https://www.mollie.com) to your application. Mollie has excellent [documentation](https://www.mollie.com/nl/docs/overzicht) which I highly recommend you read before using this library. Please keep in mind that this is a 3rd party library and I am in no way associated with Mollie. 

## Getting started

### Installing the library
The easiest way to install the Mollie Api library is to use the [Nuget Package](https://www.nuget.org/packages/Mollie.Api).
```
Install-Package Mollie.Api
```

### Example projects
Two example projects are included. A simple console application and a web application that allows you to view, create, pay and refund payments. 

### Creating a MollieClient object
The MollieClient object allows you to send and receive requests to the Mollie REST webservice. 
```c#
MollieClient mollieClient = new MollieClient("{your_api_key}");
```
### Payments
#### Creating a payment
```c#
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = 100,
    Description = "Test payment of the example project",
    RedirectUrl = "http://google.com"
};

PaymentResponse paymentResponse = mollieClient.CreatePaymentAsync(paymentRequest).Result;
```

If you want to create a payment with a specific paymentmethod, there are seperate classes that allow you to set paymentmethod specific parameters. For example, a bank transfer payment allows you to set the billing e-mail and due date. Have a look at the [Mollie create payment documentation](https://www.mollie.com/nl/docs/reference/payments/create) for more information. 

The full list of payment specific request classes is:
- BankTransferPaymentRequest
- CreditCardPaymentRequest
- IDealPaymentRequest
- PayPalPaymentRequest
- SepaDirectDebitRequest

#### Retrieving a payment by id
```c#
PaymentResponse result = this._mollieClient.GetPaymentAsync(paymentResponse.Id).Result;
```

Keep in mind that some payment methods have specific payment detail values. For example: PayPal payments have reference and customer reference properties. In order to access these properties you have to cast the PaymentResponse to the PayPalPaymentResponse and access the Detail property. 

Take a look at the [Mollie payment response documentation](https://www.mollie.com/nl/docs/reference/payments/get) for a full list of payment methods that have extra detail fields.

The full list of payment specific response classes is:
- BankTransferPaymentResponse
- BitcoinPaymentResponse
- CreditCardPaymentResponse
- IdealPaymentResponse
- MisterCashPaymentResponse
- PayPalPaymentResponse
- PaySafeCardPaymentResponse
- PodiumCadeauKaartPaymentResponse
- SofortPaymentResponse

#### Retrieving a list off payments
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional. The maximum number of payments you can request in a single roundtrip is 250. 
```c#
ListResponse<PaymentResponse> response = this._mollieClient.GetPaymentListAsync(offset, count).Result;
```

### Payment methods
#### Retrieving a list of all payment methods
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
ListResponse<PaymentMethodResponse> paymentMethodList = this._mollieClient.GetPaymentMethodListAsync(offset, count).Result;
foreach (PaymentMethodResponse paymentMethod in paymentMethodList.Data) {
  // Your code here
}
```
#### Retrieving a single payment method
```c#
PaymentMethodResponse paymentMethodResponse = this._mollieClient.GetPaymentMethodAsync(paymentMethod).Result;
```

### Issuer methods
#### Retrieve issuer list
```c#
ListResponse<IssuerResponse> issuerList = this._mollieClient.GetIssuerListAsync().Result;
foreach (IssuerResponse issuer in issuerList.Data) {
    // Your code here
}
```

#### Retrieve a single issuer by id
```c#
this._mollieClient.GetIssuerAsync(issuerId).Result;
```

### Refund methods
#### Create a new refund
```c#
RefundResponse refundResponse = this._mollieClient.CreateRefund(payment.Id, amount).Result;
```

#### Retrieve a refund by payment and refund id
```c#
RefundResponse refundResponse = this._mollieClient.GetRefund(payment.Id, refundResponse.Id).Result;
```

#### Retrieve refund list
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
ListResponse<RefundResponse> refundList = this._mollieClient.GetRefundList(payment.Id, offset, count).Result;
```

#### Cancel a refund
```c#
this._mollieClient.CancelRefund(paymentId, refundId);
```
