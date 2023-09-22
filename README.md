# MollieApi
![](https://github.com/Viincenttt/MollieApi/workflows/Run%20automated%20tests/badge.svg)

This project allows you to easily add the [Mollie payment provider](https://www.mollie.com) to your application. Mollie has excellent [documentation](https://docs.mollie.com/) which I highly recommend you read before using this library. 

## Support
If you have encounter any issues while using this library or have any feature requests, feel free to open an issue on GitHub. If you need help integrating the Mollie API into your .NET application, please contact me on [LinkedIn](https://www.linkedin.com/in/vincent-kok-4aa44211/). 

Want to chat with other developers regarding the Mollie API? The official Mollie developer Discord is a great place to provide feedback, ask questions and chat with other developers: [Mollie Developer Discord](https://discord.gg/Pdy49HxCWZ)

## Contributions

Have you spotted a bug or want to add a missing feature? All pull requests are welcome! Please provide a description of the bug or feature you have fixed/added. Make sure to target the latest development branch. 
## Table of contents
[1. Getting started](#1-getting-started)  
[2. Payment API](#2-payment-api)  
[3. Payment method API](#3-payment-method-api)  
[4. Refund API](#4-refund-api)  
[5. Customer API](#5-customer-api)  
[6. Mandate API](#6-mandate-api)  
[7. Subscription API](#7-subscription-api)  
[8. Order API](#8-order-api)  
[9. Organizations API](#9-organizations-api)  
[10. Connect Api](#10-connect-api)  
[11. Profile Api](#11-profile-api)  
[12. Captures API](#12-captures-api)  
[13. Onboarding Api](#13-onboarding-api)  
[14. Payment link Api](#14-payment-link-api)  
[15. Balances Api](#15-balances-api)  
[16. Terminal Api](#16-terminal-api)  
[17. Client Link Api](#17-client-link-api)  
[18. Wallet Api](#18-wallet-api)  

## 1. Getting started

### Installing the library
The easiest way to install the Mollie Api library is to use the [Nuget Package](https://www.nuget.org/packages/Mollie.Api).
```
Install-Package Mollie.Api
```

### Creating a API client
Every API has it's own API client class. For example: PaymentClient, PaymentMethodClient, CustomerClient, MandateClient, SubscriptionClient, IssuerClient and RefundClient classes. All of these API client classes also have their own interface. The recommended way to instantiate API clients, is to use the built in dependency injection extension method:
```c#
builder.Services.AddMollieApi(options => {
    options.ApiKey = builder.Configuration["Mollie:ApiKey"];
    options.RetryPolicy = MollieHttpRetryPolicies.TransientHttpErrorRetryPolicy();
});
```

Alternatively, you can create the API client manually using the constructor. All API clients require a api/oauth key in the constructor and you can also pass in an optional `HttpClient` object. If you do not pass a `HttpClient` object, one will be created automatically. In the latter case, you are responsible for disposing the API client by calling the `Dispose()` method on the API client object or by using a `using` statement. 
```c#
using IPaymentClient paymentClient = new PaymentClient("{yourApiKey}", new HttpClient());
```

### Example projects
An example ASP.NET Core web application project is included. In order to use this project you have to set your Mollie API key in the appsettings.json file. The example project demonstrates the Payment API, Mandate API, Customer API and Subscription API. 

### Supported API's
This library currently supports the following API's:
- Payments API
- PaymentMethod API
- PaymentLink API
- Customers API
- Mandates API
- Subscriptions API
- Refund API
- Connect API
- Chargebacks API
- Invoices API
- Permissions API
- Profiles API
- Organizations API
- Order API
- Captures API
- Onboarding API
- Balances API
- Terminal API
- ClientLink API
- Wallet API

### Supported .NET versions
This library is built using .NET standard 2.0. This means that the package supports the following .NET implementations:
| .NET implementation  | Version support |
| ------------- | ------------- |
| .NET and .NET Core | 2.0, 2.1, 2.2, 3.0, 3.1, 5.0, 6.0, 7.0  |
| .NET Framework  | 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1  |
| Mono | 5.4, 6.4  |
| Universal Windows Platform | 10.0.16299, TBD |
| Xamarin.iOS | 10.14, 12.16 |
| Xamarin.Mac | 3.8, 5.16 |
| Xamarin.Android | 8.0, 10.0 |

Source: https://docs.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0

### List of constant value strings
In the past, this library used enums that were decorated with the EnumMemberAttribute for static values that were defined in the Mollie documentation. We have now moved away from this idea and are using constant strings everywhere. The reason for this is that enum values often broke when Mollie added new values to their API. This means that I had to release a new version every time when Mollie added a new value and all library consumers had to update their version. The following static classes are available with const string values that you can use to set and compare values in your code:
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
- Mollie.Api.Models.Order.OrderLineDetailsCategory
- Mollie.Api.Models.Order.OrderLineDetailsType
- Mollie.Api.Models.Order.OrderLineStatus
- Mollie.Api.Models.Order.OrderStatus
- Mollie.Api.Models.Order.OrderLineOperation
- Mollie.Api.Models.Profile.CategoryCode
- Mollie.Api.Models.Profile.ProfileStatus
- Mollie.Api.Models.Refund.RefundStatus
- Mollie.Api.Models.Settlement.SettlementStatus
- Mollie.Api.Models.Subscription.SubscriptionStatus
- Mollie.Api.Models.Connect.AppPermissions
- Mollie.Api.Models.Onboarding.Response.OnboardingStatus
- Mollie.Api.Models.Balance.Response.BalanceReport.ReportGrouping
- Mollie.Api.Models.Balance.Response.BalanceTransaction.BalanceTransactionContextType
- Mollie.Api.Models.Payment.Locale
- Mollie.Api.Models.Currency
- Mollie.Api.Models.CompanyEntityType

You can use these classes similar to how you use enums. For example, when creating a new payment, you can do the following:
```c#
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = new Amount(Currency.EUR, 100.00m),
    Description = "Test payment of the example project",
    RedirectUrl = "http://google.com",
	Method = Mollie.Api.Models.Payment.PaymentMethod.Ideal // instead of "Ideal"
};
```
### Testing
During the process of building your integration, it is important to properly test it. You can access the test mode of the Mollie API in two ways: by using the Test API key, or, if you are using organization access tokens or app tokens, by providing the testmode parameter in your API request. If you are using the Test API key, you do not have to set the testmode parameter anywhere. Any entity you create, retrieve, update or delete using a Test API key can only interact with the test system of Mollie, which is completely isolated from the production environment. 

### Idempotency and retrying requests
When issuing requests to an API, there is always a small chance of issues on either side of the connection. For example, the API may not respond to the request within a reasonable timeframe. Your server will then consider the request to have ‘timed out’. However, your request may still arrive at the API eventually and get executed, despite your server considering it a timeout.

Mollie supports the Idempotency-Key industry standard. When sending a request to the Mollie API, you can send along a header with a unique value. If another request is made with the exact same header value within one hour, the Mollie API will return a cached version of the initial response. This way, your API requests become what we call idempotent. Read more on this in the [Mollie documentation on Idempotency](https://docs.mollie.com/overview/api-idempotency)

The library automatically generates a unique GUID for each request and adds it to the Idempotency-Key header. Using a transient fault handling library such as [Polly](https://github.com/App-vNext/Polly), you are able to automatically retry requests in case of a timeout or 5xx status code error using the following example code:
```c#
using Polly;
using Polly.Extensions.Http;

public static class MollieApiClientServiceExtensions {
        public static IServiceCollection AddMollieApi(this IServiceCollection services, IConfiguration configuration) {
		MollieConfiguration mollieConfiguration = configuration.GetSection("Mollie").Get<MollieConfiguration>();
            
            	services.AddHttpClient<IPaymentClient, PaymentClient>(httpClient => new PaymentClient(mollieConfiguration.ApiKey, httpClient))
                	.AddPolicyHandler(GetDefaultRetryPolicy());
	}
	
	static IAsyncPolicy<HttpResponseMessage> GetDefaultRetryPolicy() {
            return HttpPolicyExtensions
	    	// Timeout errors or 5xx static code errors
                .HandleTransientHttpError() 
		// Requests are retried three times, with different intervals
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                    retryAttempt)));
        }
}
```

## 2. Payment API
### Creating a payment
```c#
using IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = new Amount(Currency.EUR, 100.00m),
    Description = "Test payment of the example project",
    RedirectUrl = "http://google.com"
};

PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);
string checkoutUrl = paymentResponse.Links.Checkout.Href;
```

If you want to create a payment with a specific paymentmethod, there are seperate classes that allow you to set paymentmethod specific parameters. For example, a bank transfer payment allows you to set the billing e-mail and due date. Have a look at the [Mollie create payment documentation](https://www.mollie.com/nl/docs/reference/payments/create) for more information. 

The full list of payment specific request classes is:
- ApplePayPaymentRequest
- BankTransferPaymentRequest
- CreditCardPaymentRequest
- GiftcardPaymentRequest
- IdealPaymentRequest
- KbcPaymentRequest
- PayPalPaymentRequest
- PaySafeCardPaymentRequest
- Przelewy24PaymentRequest
- SepaDirectDebitRequest

For example, if you'd want to create a bank transfer payment, you can instantiate a new BankTransferPaymentRequest:
```c#
using IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
BankTransferPaymentRequest paymentRequest = new BankTransferPaymentRequest();
// Set bank transfer specific BillingEmail property
paymentRequest.BillingEmail = "{billingEmail}";
BankTransferPaymentResponse response = (BankTransferPaymentResponse)await paymentClient.CreatePaymentAsync(paymentRequest);
```

#### Redirecting a customer to the checkout link
Once you have created a payment, you can redirect the customer to the checkout link where he can do the actual payment. The `PaymentResponse` object contains a `Links` property that contains the checkout link. 
```c#
using IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = new Amount(Currency.EUR, 100.00m),
    Description = "Test payment of the example project",
    RedirectUrl = "http://google.com"
};

PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);
string checkoutUrl = paymentResponse.Links.Checkout.Href;
```

#### QR codes
Some payment methods also support QR codes. In order to retrieve a QR code, you have to set the `includeQrCode` parameter to `true` when sending the payment request. For example:
```c#
PaymentRequest paymentRequest = new PaymentRequest() {
	Amount = new Amount(Currency.EUR, 100.00m),
	Description = "Description",
	RedirectUrl = "http://www.mollie.com",
	Method = PaymentMethod.Ideal
};
using IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
PaymentResponse result = await this._paymentClient.CreatePaymentAsync(paymentRequest, includeQrCode: true);
IdealPaymentResponse idealPaymentResult = result as IdealPaymentResponse;
IdealPaymentResponseDetails idealPaymentDetails = idealPaymentResult.Details;
string qrCode = idealPaymentDetails.QrCode.Src;
```

#### Passing multiple payment methods
It is also possible to pass multiple payment methods when creating a new payment. Mollie will then only show the payment methods you've specified when creating the payment request. 
```c#
PaymentRequest paymentRequest = new PaymentRequest() {
	Amount = new Amount(Currency.EUR, 100.00m),
	Description = "Description",
	RedirectUrl = "http://www.mollie.com",
	Methods = new List<string>() {
		PaymentMethod.Ideal,
		PaymentMethod.CreditCard,
		PaymentMethod.DirectDebit
	}
};
```

### Retrieving a payment by id
```c#
using IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
PaymentResponse result = await paymentClient.GetPaymentAsync({paymentId});
```

Keep in mind that some payment methods have specific payment detail values. For example: PayPal payments have reference and customer reference properties. In order to access these properties you have to cast the PaymentResponse to the PayPalPaymentResponse and access the Detail property. 

Take a look at the [Mollie payment response documentation](https://www.mollie.com/nl/docs/reference/payments/get) for a full list of payment methods that have extra detail fields.

The full list of payment specific response classes is:
- BancontactPaymentResponse
- BankTransferPaymentResponse
- BelfiusPaymentResponse
- CreditCardPaymentResponse
- GiftcardPaymentResponse
- IdealPaymentResponse
- IngHomePayPaymentResponse
- KbcPaymentResponse
- PayPalPaymentResponse
- PaySafeCardPaymentResponse
- SepaDirectDebitResponse
- SofortPaymentResponse
- PointOfSalePaymentResponse

### Updating a payment
Some properties of a payment can be updated after the payment has been created. 
```c#
using IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
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
    Amount = new Amount(Currency.EUR, 100.00m),
    Description = "{description}",
    RedirectUrl = this.DefaultRedirectUrl,
};

// Set the metadata
paymentRequest.SetMetadata(metadataRequest);

// When we retrieve the payment response, we can convert our metadata back to our custom class
using IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
PaymentResponse result = await paymentClient.CreatePaymentAsync(paymentRequest);
CustomMetadataClass metadataResponse = result.GetMetadata<CustomMetadataClass>();
```

### Retrieving a list of payments
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional. The maximum number of payments you can request in a single roundtrip is 250. 
```c#
using IPaymentClient paymentClient = new PaymentClient("{yourApiKey}");
ListResponse<PaymentResponse> response = await paymentClient.GetPaymentListAsync("{offset}", "{count}");
```



## 3. Payment method API
### Retrieving a list of all payment methods
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
using IPaymentMethodClient _paymentMethodClient = new PaymentMethodClient("{yourApiKey}");
ListResponse<PaymentMethodListData> paymentMethodList = await this._paymentMethodClient.GetPaymentMethodListAsync();
foreach (PaymentMethodResponse paymentMethod in paymentMethodList.Items) {
	// Your code here
}
```
### Retrieving a single payment method
```c#
using IPaymentMethodClient _paymentMethodClient = new PaymentMethodClient("{yourApiKey}");
PaymentMethodResponse paymentMethodResponse = await paymentMethodClient.GetPaymentMethodAsync(PaymentMethod.Ideal);
```

## 4. Refund API
### Create a new refund
```c#
using IRefundClient refundClient = new RefundClient("{yourApiKey}");
RefundResponse refundResponse = await this._refundClient.CreateRefundAsync("{paymentId}", new RefundRequest() {
	Amount = new Amount(Currency.EUR, "100"),
	Description = "{description}"
});
```

### Retrieve a refund by payment and refund id
```c#
using IRefundClient refundClient = new RefundClient("{yourApiKey}");
RefundResponse refundResponse = await this._refundClient.GetRefundAsync("{paymentId}", "{refundId}");
```

### Retrieve refund list
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
using IRefundClient refundClient = new RefundClient("{yourApiKey}");
ListResponse<RefundListData> refundList = await this._refundClient.GetRefundListAsync("{paymentId}", "{offset}", "{count}");
```

### Cancel a refund
```c#
using IRefundClient refundClient = new RefundClient("{yourApiKey}");
await refundClient.CancelRefundAsync("{paymentId}", "{refundId}");
```



## 5. Customer API
### Creating a new customer
Customers will appear in the Mollie Dashboard where you can manage their details, and also view their payments and subscriptions.
```c#
CustomerRequest customerRequest = new CustomerRequest() {
	Email = "{email}",
	Name = "{name}",
	Locale = Locale.nl_NL
};

using ICustomerClient customerClient = new CustomerClient("{yourApiKey}");
CustomerResponse customerResponse = await customerClient.CreateCustomerAsync(customerRequest);
```

### Retrieve a customer by id
Retrieve a single customer by its ID.
```c#
using ICustomerClient customerClient = new CustomerClient("{yourApiKey}");
CustomerResponse customerResponse = await customerClient.GetCustomerAsync(customerId);
```

### Retrieve customer list
Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
using ICustomerClient customerClient = new CustomerClient("{yourApiKey}");
ListResponse<CustomerResponse> response = await customerClient.GetCustomerListAsync();
```

### Updating a customer
Update an existing customer.
```c#
using ICustomerClient customerClient = new CustomerClient("{yourApiKey}");
CustomerRequest updateParameters = new CustomerRequest() {
	Name = "{customerName}"
};
CustomerResponse result = await customerClient.UpdateCustomerAsync("{customerIdToUpdate}", updateParameters);
```

### Deleting a customer
Delete a customer. All mandates and subscriptions created for this customer will be canceled as well.
```c#
using ICustomerClient customerClient = new CustomerClient("{yourApiKey}");
await customerClient.DeleteCustomerAsync("{customerIdToDelete}");
```

### Create customer payment
Creates a payment for the customer.
```c#
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = new Amount(Currency.EUR, 100.00m),
    Description = "{description}",
    RedirectUrl = this.DefaultRedirectUrl,
};
using ICustomerClient customerClient = new CustomerClient("{yourApiKey}");
PaymentResponse result = await customerClient.CreateCustomerPayment({customerId}, paymentRequest);
```


## 6. Mandate API
Mandates allow you to charge a customer’s credit card or bank account recurrently.

### Creating a new mandate
Create a mandate for a specific customer.
```c#
using IMandateClient mandateclient = new MandateClient("{yourApiKey}");
MandateRequest mandateRequest = new SepaDirectDebitMandateRequest() { // Or PayPalMandateRequest
    ConsumerName = "John Smit",
    MandateReference = "My reference",
    SignatureDate = DateTime.Now
};
MandateResponse mandateResponse = await this._mandateClient.CreateMandateAsync("{customerId}", mandateRequest);
```

### Retrieve a mandate by id
Retrieve a mandate by its ID and its customer’s ID. The mandate will either contain IBAN or credit card details, depending on the type of mandate.
```c#
using IMandateClient mandateclient = new MandateClient("{yourApiKey}");
MandateResponse mandateResponse = await mandateclient.GetMandateAsync("{customerId}", "{mandateId}");
```

### Retrieve mandate list
Retrieve all mandates for the given customerId, ordered from newest to oldest. Mollie allows you to set offset and count properties so you can paginate the list. The offset and count parameters are optional.
```c#
using IMandateClient mandateclient = new MandateClient("{yourApiKey}");
ListResponse<MandateResponse> response = await mandateclient.GetMandateListAsync("{customerId}");
```

### Revoking a mandate
Revoke a customer’s mandate. You will no longer be able to charge the consumer’s bank account or credit card with this mandate.
```c#
using IMandateClient mandateclient = new MandateClient("{yourApiKey}");
await mandateclient.RevokeMandate("{customerId}", "{mandateId}");
```



## 7. Subscription API
With subscriptions, you can schedule recurring payments to take place at regular intervals. For example, by simply specifying an amount and an interval, you can create an endless subscription to charge a monthly fee, until the consumer cancels their subscription. Or, you could use the times parameter to only charge a limited number of times, for example to split a big transaction in multiple parts. You must create a mandate with the customers ID before a subscription can be made.

### Creating a new subscription
Create a subscription for a specific customer.
```c#
using ISubscriptionClient subscriptionClient = new SubscriptionClient("{yourApiKey}");
SubscriptionRequest subscriptionRequest = new SubscriptionRequest() {
	Amount = new Amount(Currency.EUR, 100.00m),
	Times = 5,
    	Interval = "1 month",
	Description = "{uniqueIdentifierForSubscription}"
};
SubscriptionResponse subscriptionResponse = await subscriptionClient.CreateSubscriptionAsync("{customerId}", subscriptionRequest);
```

### Retrieve a subscription by id
Retrieve a subscription by its ID and its customer’s ID.
```c#
using ISubscriptionClient subscriptionClient = new SubscriptionClient("{yourApiKey}");
SubscriptionResponse subscriptionResponse = await subscriptionClient.GetSubscriptionAsync("{customerId}", "{subscriptionId}");
```

### Cancelling a subscription
A subscription can be canceled any time by calling DELETE on the resource endpoint.
```c#
using ISubscriptionClient subscriptionClient = new SubscriptionClient("{yourApiKey}");
await subscriptionClient.CancelSubscriptionAsync("{customerId}", "{subscriptionId}");
```

### Retrieve customer subscription list
Retrieve all subscriptions of a customer.
```c#
using ISubscriptionClient subscriptionClient = new SubscriptionClient("{yourApiKey}");
ListResponse<SubscriptionResponse> response = await subscriptionClient.GetSubscriptionListAsync("{customerId}", null, {numberOfSubscriptions});
```

### Retrieve all subscription list
Retrieve all subscriptions
```c#
using ISubscriptionClient subscriptionClient = new SubscriptionClient("{yourApiKey}");
ListResponse<SubscriptionResponse> response = await subscriptionClient.GetAllSubscriptionList();
```

### List subscription payments
Retrieve all payments of a specific subscriptions of a customer.
```c#
using ISubscriptionClient subscriptionClient = new SubscriptionClient("{yourApiKey}");
ListResponse<PaymentResponse> response = await subscriptionClient.GetSubscriptionPaymentListAsync({customerId}, {subscriptionId});
```

### Updating a subscription
Some fields of a subscription can be updated by calling PATCH on the resource endpoint. Each field is optional. You cannot update a canceled subscription.
```c#
using ISubscriptionClient subscriptionClient = new SubscriptionClient("{yourApiKey}");
SubscriptionUpdateRequest updatedSubscriptionRequest = new SubscriptionUpdateRequest() {
	Description = $"Updated subscription {DateTime.Now}"
};
await subscriptionClient.UpdateSubscriptionAsync("{customerId}", "{subscriptionId}", updatedSubscriptionRequest);
```



## 8. Order API
The Orders API allows you to use Mollie for your order management. Pay after delivery payment methods, such as Klarna Pay later and Klarna Slice it require the Orders API and cannot be used with the Payments API.

### Creating a new order
```c#
using IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderRequest orderRequest = new OrderRequest() {
	Amount = new Amount(Currency.EUR, 100.00m),
	OrderNumber = "16738",
	Lines = new List<OrderLineRequest>() {
		new OrderLineRequest() {
			Name = "A box of chocolates",
			Quantity = 1,
			UnitPrice = new Amount(Currency.EUR, 100.00m),
			TotalAmount = new Amount(Currency.EUR, 100.00m),
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
- BillieSpecificParameters
- CreditCardSpecificParameters
- GiftcardSpecificParameters
- IDealSpecificParameters
- KbcSpecificParameters
- KlarnaSpecificParameters
- PaySafeCardSpecificParameters
- SepaDirectDebitSpecificParameters


For example, if you'd want to create a order with bank transfer payment:
```c#
using IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderRequest orderRequest = new OrderRequest() {
	Amount = new Amount(Currency.EUR, 100.00m),
	OrderNumber = "16738",
	Method = PaymentMethod.BankTransfer,
	Payment = new BankTransferSpecificParameters {
		BillingEmail = "example@example.nl"
	},
	Lines = new List<OrderLineRequest>() {
		new OrderLineRequest() {
			Name = "A box of chocolates",
			Quantity = 1,
			UnitPrice = new Amount(Currency.EUR, 100.00m),
			TotalAmount = new Amount(Currency.EUR, 100.00m),
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

#### Passing multiple payment methods
It is also possible to pass multiple payment methods when creating a new order. Mollie will then only show the payment methods you've specified when creating the payment request. 
```c#
OrderRequest orderRequest = new OrderRequest() {
	Amount = new Amount(Currency.EUR, 100.00m),
	OrderNumber = "16738",
	Methods = new List<string>() {
		PaymentMethod.Ideal,
		PaymentMethod.CreditCard,
		PaymentMethod.DirectDebit
	}
	...
```

### Retrieve a order by id
Retrieve a single order by its ID.
```c#
using IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderResponse retrievedOrder = await orderClient.GetOrderAsync({orderId});
```

### Update existing order
This endpoint can be used to update the billing and/or shipping address of an order.
```c#
using IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderUpdateRequest orderUpdateRequest = new OrderUpdateRequest() {
	OrderNumber = "1337" 
};
OrderResponse updatedOrder = await orderClient.UpdateOrderAsync({orderId}, orderUpdateRequest);
```

### Update order line
This endpoint can be used to update an order line. Only the lines that belong to an order with status created, pending or authorized can be updated.
```c#
using IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderLineUpdateRequest updateRequest = new OrderLineUpdateRequest() {
	Name = "A fluffy bear"
};
OrderResponse updatedOrder = await orderClient.UpdateOrderLinesAsync({orderId}, createdOrder.Lines.First().Id, updateRequest);
```

### Manage order lines
Use this endpoint to update, cancel, or add one or more order lines. This endpoint sends a single authorisation request that contains the final order lines and amount to the supplier.
```c#
using IOrderClient orderClient = new OrderClient("{yourApiKey}");
ManageOrderLinesRequest manageOrderLinesRequest = new ManageOrderLinesRequest() {
	Operations = new List<ManageOrderLinesOperation> {
		new ManageOrderLinesAddOperation() {
			Data = new ManageOrderLinesAddOperationData {
				Name = "new-order-line",
				// Other properties of order line to add
			}
		},
		new ManageOrderLinesUpdateOperation {
			Data = new ManageOrderLinesUpdateOperationData {
				Id = "{yourOrderLineIdToUpdate}",
				Name = "updated-name"
				// ... Other properties you'd like to update
			}
		},
		new ManageOrderLinesCancelOperation {
			Data = new ManagerOrderLinesCancelOperationData {
				Id = "{yourOrderLineIdToCancel}",
				Quantity = 1
			}
		}
	}
};
OrderResponse updatedOrder = await this._orderClient.ManageOrderLinesAsync(createdOrder.Id, manageOrderLinesRequest);
```


### Retrieve list of orders
Retrieve all orders.
```c#
using IOrderClient orderClient = new OrderClient("{yourApiKey}");
ListResponse<OrderResponse> response = await orderClient.GetOrderListAsync();
```

### Cancel existing order
Cancels an existing order. Take a look at the documentation on this endpoint to see which conditions need to apply before an order can be canceled.
```c#
using IOrderClient orderClient = new OrderClient("{yourApiKey}");
 OrderResponse canceledOrder = await orderClient.GetOrderAsync({orderId});
```

### Cancel order lines
This endpoint can be used to cancel one or more order lines that were previously authorized using a pay after delivery payment method. Use the Cancel Order API if you want to cancel the entire order or the remainder of the order.
```c#
OrderLineCancellationRequest cancellationRequest = new OrderLineCancellationRequest() {
	Lines = new List<OrderLineDetails>() {
		Id = {orderLineId},
		Quantity = 5,
		Amount = new Amount("EUR", 5)
	}
};
using IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderResponse result = await orderClient.CancelOrderLinesAsync({orderId}, cancellationRequest);
```

### Create order payment
An order has an automatically created payment that your customer can use to pay for the order. When the payment expires you can create a new payment for the order using this endpoint.
```c#
OrderPaymentRequest orderPaymentRequest = new OrderPaymentRequest() {
	Method = PaymentMethod.Ideal,
	CustomerId = {customerId},
	MandateId = {mandateId}
};
using IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderResponse result = await orderClient.CreateOrderPaymentAsync({orderId}, orderPaymentRequest);
```

### Create order refund
When using the Orders API, refunds should be made against the Order. When using pay after delivery payment methods such as Klarna Pay later and Klarna Slice it, this ensures that your customer will receive credit invoices with the correct product information on them and generally have a great experience.
```c#
OrderRefundRequest orderRefundRequest = new OrderRefundRequest() {
	Lines = new List<OrderLineDetails>() {
		Id = {orderLineId},
		Quantity = 5,
		Amount = new Amount("EUR", 5)
	},
	Description = ""
};
using IOrderClient orderClient = new OrderClient("{yourApiKey}");
OrderResponse result = await orderClient.CreateOrderRefundAsync({orderId}, orderRefundRequest);
```

### List order refunds
Retrieve all order refunds.
```C#
using IOrderClient orderClient = new OrderClient("{yourApiKey}");
ListResponse<RefundResponse> result = await orderClient.GetOrderRefundListAsync({orderId});
```

## 9. Organizations API
### Get current organization
Retrieve the currently authenticated organization.
```C#
using IOrganizationsClient client = new OrganizationsClient("{yourApiKey}");
OrganizationResponse result = await client.GetCurrentOrganizationAsync();
```

### Get organization
```C#
using IOrganizationsClient client = new OrganizationsClient("{yourApiKey}");
OrganizationResponse result = await client.GetOrganizationAsync({organizationId});
```

## 10. Connect Api
### Creating a authorization URL
The Authorize endpoint is the endpoint on Mollie web site where the merchant logs in, and grants authorization to your client application. E.g. when the merchant clicks on the Connect with Mollie button, you should redirect the merchant to the Authorize endpoint.
```C#
using IConnectClient client = new ConnectClient({clientId}, {clientSecret});
List<string> scopes = new List<string>() { AppPermissions.PaymentsRead };
string authorizationUrl = client.GetAuthorizationUrl({state}, scopes);
```

### Generate token
Exchange the auth code received at the Authorize endpoint for an actual access token, with which you can communicate with the Mollie API.
```C#
using IConnectClient client = new ConnectClient({clientId}, {clientSecret});
TokenRequest request = new TokenRequest({authCode}, {redirectUrl});
TokenResponse result = client.GetAccessTokenAsync(request);
```

### Revoke token
Revoke an access- or a refresh token. Once revoked the token can not be used anymore.
```C#
using IConnectClient client = new ConnectClient({clientId}, {clientSecret});
RevokeTokenRequest revokeTokenRequest = new RevokeTokenRequest() {
	TokenTypeHint = TokenType.AccessToken,
	Token = {accessToken}
};
TokenResponse result = client.RevokeTokenAsync(revokeTokenRequest);
```

## 11. Profile Api
### Create profile
In order to process payments, you need to create a website profile. A website profile can easily be created via the Dashboard manually. However, the Mollie API also allows automatic profile creation via the Profiles API.
```C#
ProfileRequest profileRequest = new ProfileRequest() {
	Name = {name},
	Website = {website},
	...
};
using IProfileClient client = new ProfileClient({yourApiKey});
ProfileResponse response = client.CreateProfileAsync(profileRequest);
```

### Get profile
Retrieve details of a profile, using the profile’s identifier.
```C#
using IProfileClient client = new ProfileClient({yourApiKey});
ProfileResponse profileResponse = await client.GetProfileAsync({profileId});
```

### Get current profile
Use this API if you are creating a plugin or SaaS application that allows users to enter a Mollie API key, and you want to give a confirmation of the website profile that will be used in your plugin or application.
```C#
using IProfileClient client = new ProfileClient({yourApiKey});
ProfileResponse profileResponse = await client.GetCurrentProfileAsync();
```

### Update profile
A profile is required to process payments. A profile can easily be created and updated via the Dashboard manually. However, the Mollie API also allows automatic profile creation and updates via the Profiles API.
```C#
ProfileRequest profileRequest = new ProfileRequest() {
	Name = {name},
	Website = {website},
	...
};
using IProfileClient client = new ProfileClient({yourApiKey});
ProfileResponse profileResponse = await client.UpdateProfileAsync({profileId}, profileRequest);
```

### Enable or disable payment method
Enable or disable a payment method on a specific or authenticated profile to use it with payments.
```C#
using IProfileClient client = new ProfileClient({yourApiKey});
PaymentMethodResponse paymentMethodResponse = await profileClient.EnablePaymentMethodAsync({profileId}, PaymentMethod.Ideal);
await profileClient.DisablePaymentMethodAsync({profileId}, PaymentMethod.Ideal);
```

### Enable or disable gift card issuer
Enable or disable a gift card issuer on a specific or authenticated profile to use it with payments.
```C#
using IProfileClient client = new ProfileClient({yourApiKey});
const string issuerToToggle = "festivalcadeau";
EnableGiftCardIssuerResponse paymentMethodResponse = await profileClient.EnableGiftCardIssuerAsync({profileId}, issuerToToggle);
await profileClient.DisableGiftCardIssuerAsync({profileId}, issuerToToggle);
```

## 12. Captures API
### Create capture
Some payment methods allow you to first collect a consumer’s authorization, and capture the amount at a later point. 
By default, Mollie captures payments automatically. If however you configured your payment with captureMode: manual, you can capture the payment using this endpoint after having collected the consumer’s authorization.
```C#
PaymentRequest paymentRequest = new PaymentRequest() {
	Amount = new Amount(Currency.EUR, 10m),
	Description = "Description",
	RedirectUrl = RedirectUrl = "http://www.google.nl",
	Method = PaymentMethod.CreditCard,
	CaptureMode = CaptureMode.Manual
};
using IPaymentClient paymentClient = new PaymentClient({yourApiKey});
var paymentResponse = await paymentClient.GetPaymentAsync(paymentResponse.Id);

CaptureResponse captureResponse = await _captureClient.CreateCapture(paymentResponse.Id, new CaptureRequest {
	Amount = new Amount(Currency.EUR, 10m),
	Description = "capture"
});
using ICaptureClient captureClient = new CaptureClient({yourApiKey});
CaptureResponse result = await captureClient.GetCaptureAsync({paymentId}, {captureId});
```

### Get capture
Retrieve a single capture by its ID. Note the original payment’s ID is needed as well.
```C#
using ICaptureClient captureClient = new CaptureClient({yourApiKey});
CaptureResponse result = await captureClient.GetCaptureAsync({paymentId}, {captureId});
```

### List captures
Retrieve all captures for a certain payment.
```C#
using ICaptureClient captureClient = new CaptureClient({yourApiKey});
ListResponse<CaptureResponse> result = await captureClient.GetCapturesListAsync({paymentId});
```

## 13. Onboarding Api
### Get onboarding status
Get the status of onboarding of the authenticated organization.
```C#
using IOnboardingClient onboardingClient = new OnboardingClient({yourApiKey});
OnboardingStatusResponse onboardingResponse = await onboardingClient.GetOnboardingStatusAsync();
```

### Submit onboarding data
Submit data that will be prefilled in the merchant’s onboarding. Please note that the data you submit will only be processed when the onboarding status is needs-data.
```C#
SubmitOnboardingDataRequest submitOnboardingDataRequest = new SubmitOnboardingDataRequest() {
	Organization = new OnboardingOrganizationRequest() {
		Name = {name},
		Address = new AddressObject() {
			StreetAndNumber = {streetAndNumber}
		}
	},
	Profile = new OnboardingProfileRequest() {
		Name = {name}
	}
};
using IOnboardingClient onboardingClient = new OnboardingClient({yourApiKey});
await onboardingClient.SubmitOnboardingDataAsync(submitOnboardingDataRequest);
```

## 14. Payment link Api
### Create payment link
```C#
PaymentLinkRequest paymentLinkRequest = new PaymentLinkRequest() {
	Description = "Test",
	Amount = new Amount(Currency.EUR, 50),
	WebhookUrl = this.DefaultWebhookUrl,
	RedirectUrl = this.DefaultRedirectUrl,
	ExpiresAt = DateTime.Now.AddDays(1)
};
using IPaymentLinkClient client = new PaymentLinkClient({yourApiKey});
await this._paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);
```

### Get payment link
```C#
using IPaymentLinkClient client = new PaymentLinkClient({yourApiKey});
await this._paymentLinkClient.GetPaymentLinkAsync({yourPaymentLinkId});
```

### List payment links
```C#
using IPaymentLinkClient client = new PaymentLinkClient({yourApiKey});
ListResponse<PaymentLinkResponse> response = await this._paymentLinkClient.GetPaymentLinkListAsync();
```

## 15. Balances Api
### Get balance
Retrieve a balance using a balance id string identifier. 
```C#
using IBalanceClient client = new BalanceClient({yourApiKey});
BalanceResponse balanceResponse = await this._balanceClient.GetBalanceAsync({yourBalanceId});
```

### Get primary balance
Retrieve the primary balance. This is the balance of your account’s primary currency, where all payments are settled to by default.
```C#
using IBalanceClient client = new BalanceClient({yourApiKey});
BalanceResponse balanceResponse = await this._balanceClient.GetPrimaryBalanceAsync();
```

### List balances
Retrieve all the organization’s balances, including the primary balance, ordered from newest to oldest.
```C#
using IBalanceClient client = new BalanceClient({yourApiKey});
ListResponse<BalanceResponse> balanceList = await this._balanceClient.ListBalancesAsync();
```

### Get balance report
You can retrieve reports in two different formats. With the status-balances format, transactions are grouped by status (e.g. pending, available), then by transaction type, and then by other sub-groupings where available (e.g. payment method). With the transaction-categories format, transactions are grouped by transaction type, then by status, and then again by other sub-groupings where available.

This applies to both the "Get balance report" method as well as the "Get primary balance report" method. 
```C#
using IBalanceClient client = new BalanceClient({yourApiKey});
string reportGrouping = ReportGrouping.TransactionCategories;
BalanceReportResponse balanceReport = await this._balanceClient.GetBalanceReportAsync({yourBalanceId}, grouping: reportGrouping);
```

### Get primary balance report
With the Get primary balance report endpoint you can retrieve a summarized report for all movements on your primary balance within a given timeframe.
```C#
using IBalanceClient client = new BalanceClient({yourApiKey});
string reportGrouping = ReportGrouping.StatusBalances;
BalanceReportResponse balanceReport = await this._balanceClient.GetPrimaryBalanceReportAsync(grouping: reportGrouping);
```

### List balance transactions
With the List balance transactions endpoint you can retrieve a list of all the movements on your balance. This includes payments, refunds, chargebacks, and settlements.
```C#
using IBalanceClient client = new BalanceClient({yourApiKey});
BalanceTransactionResponse balanceTransactions = await this._balanceClient.ListBalanceTransactionsAsync({yourBalanceId});
```

Each transaction in the list of transactions, has a context. The context properties depends on the "type" field of the transaction. For example, a transaction with the type "payment" has a context with a "payment-id" value. For a full list of transaction types and their context specific properties, take a look at the Mollie documentation of the "[List balance transactions](https://docs.mollie.com/reference/v2/balances-api/list-balance-transactions)" endpoint.

### List primary balance transactions
With the List primary balance transactions endpoint you can retrieve a list of all the movements on your primary balance. This includes payments, refunds, chargebacks, and settlements.
```C#
using IBalanceClient client = new BalanceClient({yourApiKey});
BalanceTransactionResponse balanceTransactions = await this._balanceClient.ListPrimaryBalanceTransactionsAsync();
```

## 16. Terminal Api
### List terminals
Retrieve all point-of-sale terminal devices linked to your organization or profile, ordered from newest to oldest. 
```C#
using ITerminalClient client = new TerminalClient({yourApiKey});
ListResponse<TerminalResponse> response = await client.GetTerminalListAsync();
```

### Get terminal by id
```C#
using ITerminalClient client = new TerminalClient({yourApiKey});
TerminalResponse response = await client.GetTerminalAsync({yourTerminalId});
```

## 17. Client Link Api
### Create a client link
Link a new organization to your OAuth application, in effect creating a new client. 
```C#
var request = new ClientLinkRequest
{
	Owner = new ClientLinkOwner
	{
		Email = "norris@chucknorrisfacts.net",
		GivenName = "Chuck",
		FamilyName = "Norris",
		Locale = "en_US"
	},
	Address = new AddressObject()
	{
		StreetAndNumber = "Keizersgracht 126",
		PostalCode = "1015 CW",
		City = "Amsterdam",
		Country = "NL"
	},
	Name = "Mollie B.V.",
	RegistrationNumber = "30204462",
	VatNumber = "NL815839091B01"
};
using IClientLinkClient client = new ClientLinkClient({yourClientId}, {yourClientSecret});
ClientLinkResponse response = await clientLinkClient.CreateClientLinkAsync(request);
```

### Generate the client link URL
To the ClientLink link you created through the API, you can then add the OAuth details of your application, the client_id, scope you want to request
```C#
using IClientLinkClient client = new ClientLinkClient({yourClientId}, {yourClientSecret});
ClientLinkResponse response = await clientLinkClient.CreateClientLinkAsync(request);
var clientLinkUrl = response.Links.ClientLink;
string result = clientLinkClient.GenerateClientLinkWithParameters(clientLinkUrl, {yourState}, {yourScope}, {forceApprovalPrompt});
```

## 18. Wallet Api
## Request Apple Pay Payment Session
For integrating Apple Pay in your own checkout on the web, you need to provide merchant validation. For more information on this topic, read the [Mollie documentation on Apple Pay Payment sessions](https://docs.mollie.com/reference/v2/wallets-api/request-apple-pay-payment-session). 
```C#
var request = new ApplePayPaymentSessionRequest() {
	Domain = "pay.mywebshop.com",
	ValidationUrl = "https://apple-pay-gateway-cert.apple.com/paymentservices/paymentSession"
};
using var walletClient = new WalletClient({yourApiClient});
ApplePayPaymentSessionResponse response = await walletClient.RequestApplePayPaymentSessionAsync(request);
```
