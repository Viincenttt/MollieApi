# MollieApi

This project allows you to easily add the Mollie payment provider to your application. Mollie has excellent [documentation](https://www.mollie.com/nl/docs/overzicht) which I highly recommend you read before using this library. 

Keep in mind that I am in no way associated with Mollie. 

## Getting started

### Installing the library
The easiest way to install the Mollie Api library is to use the [Nuget Package](https://www.nuget.org/packages/Mollie.Api/1.0.0).
```
Install-Package Mollie.Api
```

### Creating a MollieApi object
The MollieApi object allows you to send and receive requests to the Mollie REST webservice. 
```c#
MollieApi mollieApi = new MollieApi("test_gwiqLFcMzjBSzKZuxAADm6taxtAUgF");
```
### Payments
#### Creating a payment
```c#
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = 100,
    Description = "Test payment of the example project",
    RedirectUrl = "http://google.com"
};

PaymentResponse paymentResponse = mollieApi.CreatePayment(paymentRequest).Result;
```

#### Retrieving a payment by id
```c#
PaymentResponse result = this._mollieClient.GetPayment(paymentResponse.Id).Result;
```

#### Retrieving a list off payments
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional. The maximum number of payments you can request in a single roundtrip is 250. 
```c#
ListResponse<PaymentResponse> response = this._mollieClient.GetPaymentList(offset, count).Result;
```

### Payment methods
#### Retrieving a list of all payment methods
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
ListResponse<PaymentMethodResponse> paymentMethodList = this._mollieClient.GetPaymentMethodList(offset, count).Result;
foreach (PaymentMethodResponse paymentMethod in paymentMethodList.Data) {
  // Your code here
}
```
#### Retrieving a single payment method
```c#
PaymentMethodResponse paymentMethodResponse = this._mollieClient.GetPaymentMethod(paymentMethod).Result;
```

### Issuer methods
#### Retrieve issuer list
```c#
ListResponse<IssuerResponse> issuerList = this._mollieClient.GetIssuerList().Result;
foreach (IssuerResponse issuer in issuerList.Data) {
    // Your code here
}
```

#### Retrieve a single issuer by id
```c#
this._mollieClient.GetIssuer(issuerId).Result;
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
