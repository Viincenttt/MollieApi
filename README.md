# MollieApi

This project allows you to easily add the [Mollie payment provider](https://www.mollie.com) to your application. Mollie has excellent [documentation](https://www.mollie.com/nl/docs/overzicht) which I highly recommend you read before using this library. Please keep in mind that this is a 3rd party library and I am in no way associated with Mollie. 

If you have encounter any issues while using this library or have any feature requests, feel free to open an issue on GitHub. 

## Table of contents
[1. Mollie API v1 and V2](#1-mollie-api-v1-and-v2)  
[2. Getting started](#2-getting-started)  
[3. Payment API](#3-payment-api)  
[4. Payment method API](#4-payment-method-api)  
[5. Refund API](#5-refund-api)  
[6. Customer API](#6-customer-api)  
[7. Mandate API](#7-mandate-api)  
[8. Subscription API](#8-subscription-api)  

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

### Creating a API client object
Every API has it's own API client class. For example: PaymentClient, PaymentMethodClient, CustomerClient, MandateClient, SubscriptionClient, IssuerClient and RefundClient classes. All of these API client classes also have their own interface. 

These client API classes allow you to send and receive requests to the Mollie REST webservice. To create a API client class, you simple instantiate a new object for the API you require. For example, if you want to create new payments, you can use the PaymentClient class. 
```c#
IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
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
- IDealPaymentRequest
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
BankTransferPaymentResponse response = (BankTransferPaymentResponse)await this._paymentClient.CreatePaymentAsync(paymentRequest);
```

### Retrieving a payment by id
```c#
IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
PaymentResponse result = await paymentClient.GetPaymentAsync(paymentResponse.Id);
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

### Retrieving a list off payments
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
With subscriptions, you can schedule recurring payments to take place at regular intervals. For example, by simply specifying an amount and an interval, you can create an endless subscription to charge a monthly fee, until the consumer cancels their subscription. Or, you could use the times parameter to only charge a limited number of times, for example to split a big transaction in multiple parts.

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
