# MollieApi

This project allows you to easily add the [Mollie payment provider](https://www.mollie.com) to your application. Mollie has excellent [documentation](https://www.mollie.com/nl/docs/overzicht) which I highly recommend you read before using this library. 

## Support
If you have encounter any issues while using this library or have any feature requests, feel free to open an issue on GitHub. If you need fast support or need help integrating the Mollie API into your .NET application, please contact me on [LinkedIn](https://www.linkedin.com/in/vincent-kok-4aa44211/). 

## Contributions

Have you spotted a bug or want to add a missing feature? All pull requests are welcome! Please provide a description of the bug or feature you have fixed/added. Make sure to target the latest development branch. 

## Table of contents
[1. Mollie API v1 and V2](#1-mollie-api-v1-and-v2)  
[2. Getting started](#2-getting-started)  
[3. Payment API](#3-payment-api)  
[4. Payment method API](#4-payment-method-api)  
[5. Refund API](#5-refund-api)  
[6. Customer API](#6-customer-api)  
[7. Mandate API](#7-mandate-api)  
[8. Subscription API](#8-subscription-api)  
[9. Order API](#9-order-api)

## 1. Mollie API v1 and v2
In May 2018, Mollie launched version 2 of their API. Version 2 offers support for multicurrency, improved error messages and much more.  The current version of the Mollie API client supports all API version 2 features. If you want to keep using version 1, you can use version 1.5.2 of the Mollie API Nuget package. Version 2.0.0+ of the Mollie API client supports version 2 of the API.  

Mollie API version 2 is not backwards compatible with version 1. This means some of the Mollie API client code has been changed and you will need to update your project if you want to use Mollie API client version 2.0.0+. Please take a look at the [Mollie migration guide](https://docs.mollie.com/payments/migrating-v1-to-v2) for assistence.

## 2. Getting started

### Installing the library
The easiest way to install the Mollie Api library is to use the [Nuget Package](https://www.nuget.org/packages/Mollie.Api).
```
Install-Package Mollie.Api
```

### Example projects
An example ASP.NET Core web application project is included. In order to use this project you have to set your Mollie API key in the appsettings.json file. The example project demonstrates the Payment API, Mandate API, Customer API and Subscription API. 

### Supported API's
This library currently supports the following API's:
- Payments API
- PaymentMethod
- Customers API
- Mandates API
- Subscriptions API
- Refund API
- Connect API
- Chargebacks API
- Invoices API
- Permissions API
- Profiles API
- Organisations API
- Order API

### Creating a API client object
Every API has it's own API client class. For example: PaymentClient, PaymentMethodClient, CustomerClient, MandateClient, SubscriptionClient, IssuerClient and RefundClient classes. All of these API client classes also have their own interface. 

These client API classes allow you to send and receive requests to the Mollie REST webservice. To create a API client class, you simple instantiate a new object for the API you require. For example, if you want to create new payments, you can use the PaymentClient class. 
```c#
IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
```

### List of constant value strings
In the past, this library used enums that were decorated with the EnumMemberAttribute for static values that were defined in the Mollie documentation. We are now moving away from this idea and using constant strings where possible. The reason for this is that enum values often broke when Mollie added new values to their API. This means that I had to release a new version every time when Mollie added a new value and all library consumers had to update their version. The following static classes are available with const string values that you can use to set and compare values in your code:
- Mollie.Api.Models.Payment.PaymentMethod
- Mollie.Api.Models.Payment.PaymentStatus
- Mollie.Api.Models.Payment.SequenceType
- Mollie.Api.Models.Payment.Request.KbcIssuer
- Mollie.Api.Models.Payment.Response.CreditCardAudience
- Mollie.Api.Models.Payment.Response.CreditCardSecurity
- Mollie.Api.Models.Payment.Response.CreditCardFailureReason
- Mollie.Api.Models.Payment.Response.CreditCardLabel
- Mollie.Api.Models.Payment.Response.CreditCardFeeRegion
- Mollie.Api.Models.Payment.Response.PayPalSellerProtection
- Mollie.Api.Models.Mandate.InvoiceStatus
- Mollie.Api.Models.Mandate.MandateStatus
- Mollie.Api.Models.Order.OrderLineDetailsType
- Mollie.Api.Models.Order.OrderLineStatus
- Mollie.Api.Models.Order.OrderStatus
- Mollie.Api.Models.Profile.CategoryCode
- Mollie.Api.Models.Profile.ProfileStatus
- Mollie.Api.Models.Refund.RefundStatus
- Mollie.Api.Models.Settlement.SettlementStatus
- Mollie.Api.Models.Subscription.SubscriptionStatus
- Mollie.Api.Models.Payment.Locale
- Mollie.Api.Models.Currency

You can use these classes similar to how you use enums. For example, when creating a new payment, you can 
```c#
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = new Amount(Currency.EUR, "100.00"),
    Description = "Test payment of the example project",
    RedirectUrl = "http://google.com",
	Method = Mollie.Api.Models.Payment.PaymentMethod.Ideal // instead of "Ideal"
};
```

## 3. Payment API
### Creating a payment
```c#
IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = new Amount(Currency.EUR, "100.00"),
    Description = "Test payment of the example project",
    RedirectUrl = "http://google.com"
};

PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);
```

If you want to create a payment with a specific paymentmethod, there are seperate classes that allow you to set paymentmethod specific parameters. For example, a bank transfer payment allows you to set the billing e-mail and due date. Have a look at the [Mollie create payment documentation](https://www.mollie.com/nl/docs/reference/payments/create) for more information. 

The full list of payment specific request classes is:
- BankTransferPaymentRequest
- BitcoinPaymentRequest
- CreditCardPaymentRequest
- GiftcardPaymentRequest
- IdealPaymentRequest
- KbcPaymentRequest
- PayPalPaymentRequest
- PaySafeCardPaymentRequest
- SepaDirectDebitRequest

For example, if you'd want to create a bank transfer payment, you can instantiate a new BankTransferPaymentRequest:
```c#
IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
BankTransferPaymentRequest paymentRequest = new BankTransferPaymentRequest();
// Set bank transfer specific BillingEmail property
paymentRequest.BillingEmail = "{billingEmail}";
BankTransferPaymentResponse response = (BankTransferPaymentResponse)await paymentClient.CreatePaymentAsync(paymentRequest);
```

Some payment methods also support QR codes. In order to retrieve a QR code, you have to set the `includeQrCode` parameter to `true` when sending the payment request. For example:
```c#
PaymentRequest paymentRequest = new PaymentRequest() {
	Amount = new Amount(Currency.EUR, "100.00"),
	Description = "Description",
	RedirectUrl = "http://www.mollie.com",
	Method = PaymentMethod.Ideal
};
IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
PaymentResponse result = await this._paymentClient.CreatePaymentAsync(paymentRequest, includeQrCode: true);
IdealPaymentResponse idealPaymentResult = result as IdealPaymentResponse;
IdealPaymentResponseDetails idealPaymentDetails = idealPaymentResult.Details;
string qrCode = idealPaymentDetails.QrCode.Src;
```

### Retrieving a payment by id
```c#
IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
PaymentResponse result = await paymentClient.GetPaymentAsync({paymentId});
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

### Updating a payment
Some properties of a payment can be updated after the payment has been created. 
```c#
IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
PaymentUpdateRequest paymentUpdateRequest = new PaymentUpdateRequest() {
	Description = "Updated description",
	Metadata = "My metadata"
};
PaymentResponse updatedPayment = await this._paymentClient.UpdatePaymentAsync({paymentId}, paymentUpdateRequest);
```

### Setting metadata
Mollie allows you to send any metadata you like in JSON notation and will save the data alongside the payment. When you fetch the payment with the API, Mollie will include the metadata. The library allows you to set the metadata JSON string manually, by setting the Metadata property of the PaymentRequest class, but the recommended way of setting/getting the metadata is to use the SetMetadata/Getmetadata methods. 

For example: 
```c#
// Custom metadata class that contains the data you want to include in the metadata class. 
CustomMetadataClass metadataRequest = new CustomMetadataClass() {
    OrderId = 1,
    Description = "{customDescription}"
};

// Create a new payment
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = new Amount(Currency.EUR, "100.00"),
    Description = "{description}",
    RedirectUrl = this.DefaultRedirectUrl,
};

// Set the metadata
paymentRequest.SetMetadata(metadataRequest);

// When we retrieve the payment response, we can convert our metadata back to our custom class
IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
PaymentResponse result = await paymentClient.CreatePaymentAsync(paymentRequest);
CustomMetadataClass metadataResponse = result.GetMetadata<CustomMetadataClass>();
```

### Retrieving a list of payments
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional. The maximum number of payments you can request in a single roundtrip is 250. 
```c#
IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
ListResponse<PaymentResponse> response = await paymentClient.GetPaymentListAsync("{offset}", "{count}");
```



## 4. Payment method API
### Retrieving a list of all payment methods
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
IPaymentMethodClient _paymentMethodClient = new PaymentMethodClient("{yourApiKey}");
ListResponse<PaymentMethodListData> paymentMethodList = await this._paymentMethodClient.GetPaymentMethodListAsync();
foreach (PaymentMethodResponse paymentMethod in paymentMethodList.Items) {
	// Your code here
}
```
### Retrieving a single payment method
```c#
IPaymentMethodClient _paymentMethodClient = new PaymentMethodClient("{yourApiKey}");
PaymentMethodResponse paymentMethodResponse = await paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Ideal);
```

## 5. Refund API
### Create a new refund
```c#
IRefundClient refundClient = new RefundClient("{yourApiKey}");
RefundResponse refundResponse = await this._refundClient.CreateRefundAsync("{paymentId}", new RefundRequest() {
	Amount = new Amount(Currency.EUR, "100"),
	Description = "{description}"
});
```

### Retrieve a refund by payment and refund id
```c#
IRefundClient refundClient = new RefundClient("{yourApiKey}");
RefundResponse refundResponse = await this._refundClient.GetRefundAsync("{paymentId}", "{refundId}");
```

### Retrieve refund list
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
IRefundClient refundClient = new RefundClient("{yourApiKey}");
ListResponse<RefundListData> refundList = await this._refundClient.GetRefundListAsync("{paymentId}", "{offset}", "{count}");
```

### Cancel a refund
```c#
IRefundClient refundClient = new RefundClient("{yourApiKey}");
await refundClient.CancelRefundAsync("{paymentId}", "{refundId}");
```



## 6. Customer API
### Creating a new customer
Customers will appear in the Mollie Dashboard where you can manage their details, and also view their payments and subscriptions.
```c#
CustomerRequest customerRequest = new CustomerRequest() {
	Email = "{email}",
	Name = "{name}",
	Locale = Locale.nl_NL
};

ICustomerClient customerClient = new CustomerClient("{yourApiKey}");
CustomerResponse customerResponse = await customerClient.CreateCustomerAsync(customerRequest);
```

### Retrieve a customer by id
Retrieve a single customer by its ID.
```c#
ICustomerClient customerClient = new CustomerClient("{yourApiKey}");
CustomerResponse customerResponse = await customerClient.GetCustomerAsync(customerId);
```

### Retrieve customer list
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
ICustomerClient customerClient = new CustomerClient("{yourApiKey}");
ListResponse<CustomerResponse> response = await customerClient.GetCustomerListAsync();
```

### Updating a customer
Update an existing customer.
```c#
ICustomerClient customerClient = new CustomerClient("{yourApiKey}");
CustomerRequest updateParameters = new CustomerRequest() {
	Name = "{customerName}"
};
CustomerResponse result = await customerClient.UpdateCustomerAsync("{customerIdToUpdate}", updateParameters);
```

### Deleting a customer
Delete a customer. All mandates and subscriptions created for this customer will be canceled as well.
```c#
ICustomerClient customerClient = new CustomerClient("{yourApiKey}");
await customerClient.DeleteCustomerAsync("{customerIdToDelete}");
```



## 7. Mandate API
Mandates allow you to charge a customer’s credit card or bank account recurrently.

### Creating a new mandate
Create a mandate for a specific customer.
```c#
IMandateClient mandateclient = new MandateClient("{yourApiKey}");
MandateRequest mandateRequest = new MandateRequest() {
	ConsumerAccount = "{iban}",
	ConsumerName = "{customerName}"
};
MandateResponse mandateResponse = await this._mandateClient.CreateMandateAsync("{customerId}", mandateRequest);
```

### Retrieve a mandate by id
Retrieve a mandate by its ID and its customer’s ID. The mandate will either contain IBAN or credit card details, depending on the type of mandate.
```c#
IMandateClient mandateclient = new MandateClient("{yourApiKey}");
MandateResponse mandateResponse = await mandateclient.GetMandateAsync("{customerId}", "{mandateId}");
```

### Retrieve mandate list
Retrieve all mandates for the given customerId, ordered from newest to oldest. Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
IMandateClient mandateclient = new MandateClient("{yourApiKey}");
ListResponse<MandateResponse> response = await mandateclient.GetMandateListAsync("{customerId}");
```

### Revoking a mandate
Revoke a customer’s mandate. You will no longer be able to charge the consumer’s bank account or credit card with this mandate.
```c#
IMandateClient mandateclient = new MandateClient("{yourApiKey}");
await mandateclient.RevokeMandate("{customerId}", "{mandateId}");
```



## 8. Subscription API
With subscriptions, you can schedule recurring payments to take place at regular intervals. For example, by simply specifying an amount and an interval, you can create an endless subscription to charge a monthly fee, until the consumer cancels their subscription. Or, you could use the times parameter to only charge a limited number of times, for example to split a big transaction in multiple parts. You must create a mandate with the customers ID before a subscription can be made.

### Creating a new subscription
Create a subscription for a specific customer.
```c#
ISubscriptionClient subscriptionClient = new SubscriptionClient("{yourApiKey}");
SubscriptionRequest subscriptionRequest = new SubscriptionRequest() {
	Amount = new Amount(Currency.EUR, "100.00"),
	Times = 5,
    	Interval = "1 month",
	Description = "{uniqueIdentifierForSubscription}"
};
SubscriptionResponse subscriptionResponse = await subscriptionClient.CreateSubscriptionAsync("{customerId}", subscriptionRequest);
```

### Retrieve a subscription by id
Retrieve a subscription by its ID and its customer’s ID.
```c#
ISubscriptionClient subscriptionClient = new SubscriptionClient("{yourApiKey}");
SubscriptionResponse subscriptionResponse = await subscriptionClient.GetSubscriptionAsync("{customerId}", "{subscriptionId}");
```

### Retrieve subscription list
Retrieve all subscriptions of a customer.
```c#
ISubscriptionClient subscriptionClient = new SubscriptionClient("{yourApiKey}");
ListResponse<SubscriptionResponse> response = await subscriptionClient.GetSubscriptionListAsync("{customerId}", null, {numberOfSubscriptions});
```

### Cancelling a subscription
```c#
ISubscriptionClient subscriptionClient = new SubscriptionClient("{yourApiKey}");
await subscriptionClient.CancelSubscriptionAsync("{customerId}", "{subscriptionId}");
```

### Updating a subscription
```c#
ISubscriptionClient subscriptionClient = new SubscriptionClient("{yourApiKey}");
SubscriptionUpdateRequest updatedSubscriptionRequest = new SubscriptionUpdateRequest() {
	Description = $"Updated subscription {DateTime.Now}"
};
await subscriptionClient.UpdateSubscriptionAsync("{customerId}", "{subscriptionId}", updatedSubscriptionRequest);
```



## 9. Order API
The Orders API allows you to use Mollie for your order management. Pay after delivery payment methods, such as Klarna Pay later and Klarna Slice it require the Orders API and cannot be used with the Payments API.

### Creating a new order
```c#
IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderRequest orderRequest = new OrderRequest() {
	Amount = new Amount(Currency.EUR, "100.00"),
	OrderNumber = "16738",
	Lines = new List<OrderLineRequest>() {
		new OrderLineRequest() {
			Name = "A box of chocolates",
			Quantity = 1,
			UnitPrice = new Amount(Currency.EUR, "100.00"),
			TotalAmount = new Amount(Currency.EUR, "100.00"),
			VatRate = "21.00",
			VatAmount = new Amount(Currency.EUR, "17.36")
		}
	},
	BillingAddress = new OrderAddressDetails() {
		GivenName = "John",
		FamilyName = "Smit",
		Email = "johnsmit@gmail.com",
		City = "Rotterdam",
		Country = "NL",
		PostalCode = "0000AA",
		Region = "Zuid-Holland",
		StreetAndNumber = "Coolsingel 1"
	},
	RedirectUrl = "http://www.google.nl",
	Locale = Locale.nl_NL
};

OrderResponse result = await orderClient.CreateOrderAsync(orderRequest);
```

If you want to create a order with a specific payment parameters you can provide a specific payment implementation. For example, a bank transfer payment allows you to set the billing e-mail and due date. Have a look at the [Mollie payment specific parameters](https://docs.mollie.com/reference/v2/orders-api/create-order#payment-specific-parameters) for more information. 

The full list of payment specific parameters classes is:
- ApplePaySpecificParameters
- BankTransferSpecificParameters
- BitcoinSpecificParameters
- CreditCardSpecificParameters
- GiftcardSpecificParameters
- IDealSpecificParameters
- KbcSpecificParameters
- PayPalSpecificParameters
- PaySafeCardSpecificParameters
- Przelewy24SpecificParameters
- SepaDirectDebitSpecificParameters

For example, if you'd want to create a order with bank transfer payment:
```c#
IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderRequest orderRequest = new OrderRequest() {
	Amount = new Amount(Currency.EUR, "100.00"),
	OrderNumber = "16738",
	Method = PaymentMethod.BankTransfer,
	Payment = new BankTransferSpecificParameters {
		BillingEmail = "example@example.nl"
	},
	Lines = new List<OrderLineRequest>() {
		new OrderLineRequest() {
			Name = "A box of chocolates",
			Quantity = 1,
			UnitPrice = new Amount(Currency.EUR, "100.00"),
			TotalAmount = new Amount(Currency.EUR, "100.00"),
			VatRate = "21.00",
			VatAmount = new Amount(Currency.EUR, "17.36")
		}
	},
	BillingAddress = new OrderAddressDetails() {
		GivenName = "John",
		FamilyName = "Smit",
		Email = "johnsmit@gmail.com",
		City = "Rotterdam",
		Country = "NL",
		PostalCode = "0000AA",
		Region = "Zuid-Holland",
		StreetAndNumber = "Coolsingel 1"
	},
	RedirectUrl = "http://www.google.nl",
	Locale = Locale.nl_NL
};
```


### Retrieve a order by id
```c#
IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderResponse retrievedOrder = await orderClient.GetOrderAsync({orderId});
```

### Update existing order
```c#
IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderUpdateRequest orderUpdateRequest = new OrderUpdateRequest() {
	OrderNumber = "1337" 
};
OrderResponse updatedOrder = await orderClient.UpdateOrderAsync({orderId}, orderUpdateRequest);
```

### Cancel existing order
```c#
IOrderClient orderClient = new OrderClient("{yourApiKey}");
 OrderResponse canceledOrder = await this._orderClient.GetOrderAsync({orderId});
```

### Update order line
```c#
IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderLineUpdateRequest updateRequest = new OrderLineUpdateRequest() {
	Name = "A fluffy bear"
};
OrderResponse updatedOrder = await orderClient.UpdateOrderLinesAsync({orderId}, createdOrder.Lines.First().Id, updateRequest);
```

### Retrieve list of orders
```c#
IOrderClient orderClient = new OrderClient("{yourApiKey}");
ListResponse<OrderResponse> response = await orderClient.GetOrderListAsync();
```
