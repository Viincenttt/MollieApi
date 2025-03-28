# MollieApi
![](https://github.com/Viincenttt/MollieApi/workflows/Run%20automated%20tests/badge.svg)

This project allows you to easily add the [Mollie payment provider](https://www.mollie.com) to your application. Mollie has excellent [documentation](https://docs.mollie.com/) which I highly recommend you read before using this library. 

## Support
If you have encounter any issues while using this library or have any feature requests, feel free to open an issue on GitHub. If you need help integrating the Mollie API into your .NET application, please contact me on [LinkedIn](https://www.linkedin.com/in/vincent-kok-4aa44211/). 

Want to chat with other developers regarding the Mollie API? The official Mollie developer Discord is a great place to provide feedback, ask questions and chat with other developers: [Mollie Developer Discord](https://discord.gg/Pdy49HxCWZ)

## Contributions
Have you spotted a bug or want to add a missing feature? All pull requests are welcome! Please provide a description of the bug or feature you have fixed/added. Make sure to target the latest development branch. 

## Getting started and documentation
The library is easy and simple to use. Take a look at the [getting started guide](https://github.com/Viincenttt/MollieApi/wiki/01.-Getting-started) and create your first payment using the Mollie API in no time. For the full documentation of all library functions, please take a look at the [documentation on the Wiki](https://github.com/Viincenttt/MollieApi/wiki/). There is also a [.NET Blazor example project](https://github.com/Viincenttt/MollieApi/tree/development/samples/Mollie.WebApplication.Blazor) available that displays various features of the library. 

### Creating a payment in under a minute
Install the [NuGet package](https://www.nuget.org/packages/Mollie.Api)
```
Install-Package Mollie.Api
```

Example code to create a iDeal payment for €100
```c#
using IPaymentClient paymentClient = new PaymentClient("{yourApiKey}", new HttpClient());
PaymentRequest paymentRequest = new PaymentRequest() {
    Amount = new Amount(Currency.EUR, 100.00m),
    Description = "Test payment of the example project",
    RedirectUrl = "http://google.com",
	Method = Mollie.Api.Models.Payment.PaymentMethod.Ideal
};
PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);
string checkoutUrl = paymentResponse.Links.Checkout.Href;
```

## Supported .NET versions
This library is built using .NET standard 2.0. This means that the package supports the following .NET implementations:
| .NET implementation  | Version support |
| ------------- | ------------- |
| .NET and .NET Core | 2.0, 2.1, 2.2, 3.0, 3.1, 5.0, 6.0, 7.0, 8.0  |
| .NET Framework  | 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1  |
| Mono | 5.4, 6.4  |
| Universal Windows Platform | 10.0.16299, TBD |
| Xamarin.iOS | 10.14, 12.16 |
| Xamarin.Mac | 3.8, 5.16 |
| Xamarin.Android | 8.0, 10.0 |

Note: This library does use the `required` keyword is some of it's model classes. That means you'll need to a project that is using language version 11 or higher. 

Source: https://docs.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0

## Supported API's
This library currently supports the following API's:
- [Payment API](https://github.com/Viincenttt/MollieApi/wiki/02.-Payment-API)
- [PaymentMethod API](https://github.com/Viincenttt/MollieApi/wiki/03.-Payment-method-API)
- [PaymentLink API](https://github.com/Viincenttt/MollieApi/wiki/14.-Payment-link-Api)
- [Customer API](https://github.com/Viincenttt/MollieApi/wiki/05.-Customer-API)
- [Mandate API](https://github.com/Viincenttt/MollieApi/wiki/06.-Mandate-API)
- [Subscription API](https://github.com/Viincenttt/MollieApi/wiki/07.-Subscription-API)
- [Refund API](https://github.com/Viincenttt/MollieApi/wiki/04.-Refund-API)
- [Connect API](https://github.com/Viincenttt/MollieApi/wiki/10.-Connect-Api)
- Chargeback API
- Invoice API
- Permissions API
- [Profile API](https://github.com/Viincenttt/MollieApi/wiki/11.-Profile-Api)
- [Organizations API](https://github.com/Viincenttt/MollieApi/wiki/09.-Organization-API)
- [Order API](https://github.com/Viincenttt/MollieApi/wiki/08.-Order-API)
- [Capture API](https://github.com/Viincenttt/MollieApi/wiki/12.-Captures-API)
- [Onboarding API](https://github.com/Viincenttt/MollieApi/wiki/13.-Onboarding-Api)
- [Balances API](https://github.com/Viincenttt/MollieApi/wiki/15.-Balances-Api)
- Terminal API
- ClientLink API
- Wallet API
- Client API
- Capability API
