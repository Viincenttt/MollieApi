# Mollie Api Client for .NET
[![NuGet](https://img.shields.io/nuget/v/Mollie.Api.svg)](https://www.nuget.org/packages/Mollie.Api)
![Build](https://github.com/Viincenttt/MollieApi/workflows/Run%20automated%20tests/badge.svg)
[![GitHub Repo stars](https://img.shields.io/github/stars/Viincenttt/MollieApi)](https://github.com/Viincenttt/MollieApi/stargazers)
[![GitHub contributors](https://img.shields.io/github/contributors/Viincenttt/MollieApi)](https://github.com/Viincenttt/MollieApi/graphs/contributors)
[![GitHub last commit](https://img.shields.io/github/last-commit/Viincenttt/MollieApi)](https://github.com/Viincenttt/MollieApi)
[![GitHub commit activity](https://img.shields.io/github/commit-activity/m/Viincenttt/MollieApi)](https://github.com/Viincenttt/MollieApi/graphs/commit-activity)
[![open issues](https://img.shields.io/github/issues/Viincenttt/MollieApi)](https://github.com/Viincenttt/MollieApi/issues)
[![Read the Wiki](https://img.shields.io/badge/docs-Wiki-blue)](https://github.com/Viincenttt/MollieApi/wiki)

Easily integrate the [Mollie payment provider](https://www.mollie.com) into your .NET application.

Full documentation of this library is available on the [Wiki](https://github.com/Viincenttt/MollieApi/wiki) — including usage examples, API references, and integration tips.

Mollie offers excellent [API documentation](https://docs.mollie.com/) that we highly recommend reviewing before using this library. If you encounter any issues or have feature requests, feel free to [open an issue](https://github.com/Viincenttt/MollieApi/issues). 

> 💬 **Need help with integration?**  
> I’m happy to assist you with your implementation or questions. Feel free to [connect with me on LinkedIn](https://www.linkedin.com/in/vincent-kok-4aa44211/) — I’d love to help!

Have feedback or ideas? Join the [official Mollie Developer Discord](https://discord.gg/Pdy49HxCWZ) or [open an issue](https://github.com/Viincenttt/MollieApi/issues).

---

## 📚 Table of Contents
- [Sponsor This Project](#-sponsor-this-project)
- [Full documentation](#-full-documentation)
- [Getting Started](#-getting-started)
  - [Dependency Injection](#dependency-injection)
  - [Manual Instantiation](#manual-instantiation)
  - [Create a Payment in under a minute](#-create-a-payment-in-under-a-minute)
  - [Example Project (Blazor)](#-blazor-example-project)
- [Supported APIs](#-supported-apis)
- [Contributions](#-contributions)
- [Supported .NET Versions](#-supported-net-versions)

---

## 💖 Sponsor This Project
This project is proudly sponsored by Mollie — thank you for supporting open source and developer tooling!

If this library has helped you or saved you time, please consider [sponsoring me on GitHub](https://github.com/sponsors/Viincenttt) as well.
Your support helps me keep improving the library and providing integration help to the community!

---

## 📖 Full Documentation
Looking for the full API docs, usage examples, and advanced guides?

👉 **Check out the full Wiki here:**  
➡️ [https://github.com/Viincenttt/MollieApi/wiki](https://github.com/Viincenttt/MollieApi/wiki)
You'll find:
- Getting started walkthroughs
- All supported APIs and code samples
- Best practices for integration

--- 

## 🛠 Getting started
Install via NuGet:
```bash
Install-Package Mollie.Api
```

### Dependency Injection
You can register all API client interfaces using the built-in DI extension:
```csharp
builder.Services.AddMollieApi(options => {
    options.ApiKey = builder.Configuration["Mollie:ApiKey"];
    options.RetryPolicy = MollieHttpRetryPolicies.TransientHttpErrorRetryPolicy();
});
```
Each API (e.g. payments, customers, mandates) has its own dedicated API client class and interface:
* `IPaymentClient`, `PaymentClient`
* `ICustomerClient`, `CustomerClient`
* `ISubscriptionClient`, `SubscriptionClient`
* `IMandateClient`, `MandateClient`
* ... and more

After registering via DI, inject the interface you need in your services or controllers.

### Manual Instantiation
If you prefer not to use DI, you can manually instantiate a client:
``` csharp
using IPaymentClient paymentClient = new PaymentClient("{yourApiKey}", new HttpClient());
```
If you do not provide a HttpClient, one will be created automatically — in that case, remember to dispose the client properly.

### 🚀 Create a Payment in under a minute
Here’s a quick example of how to create an **iDEAL** payment for €100:
```csharp
using IPaymentClient paymentClient = new PaymentClient("{yourApiKey}", new HttpClient());

var paymentRequest = new PaymentRequest {
    Amount = new Amount(Currency.EUR, 100.00m),
    Description = "The .NET library makes creating payments so easy!",
    RedirectUrl = "https://github.com/Viincenttt/MollieApi",
    Method = PaymentMethod.Ideal
};

PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);
// Redirect your user to the checkout URL
string checkoutUrl = paymentResponse.Links.Checkout.Href;
```

### 🧪 Blazor Example Project
Want to see the library in action? Check out the full-featured .NET Blazor example project, which demonstrates real-world usage of several APIs:
* Payments
* Payment links
* Orders
* Customers
* Mandates
* Subscriptions
* Payment Methods
* Terminals

🔗 [View the Example Project on GitHub](https://github.com/Viincenttt/MollieApi/tree/development/samples/Mollie.WebApplication.Blazor)
> It’s a great starting point if you’re new to Mollie or want to explore advanced scenarios like multi-step checkouts or managing recurring payments.

---

## 📦 Supported API's
This library currently supports the following API's:
- [Payment API](https://github.com/Viincenttt/MollieApi/wiki/02.-Payment-API)
- [PaymentMethod API](https://github.com/Viincenttt/MollieApi/wiki/03.-Payment-method-API)
- [PaymentLink API](https://github.com/Viincenttt/MollieApi/wiki/14.-Payment-link-Api)
- [Customer API](https://github.com/Viincenttt/MollieApi/wiki/05.-Customer-API)
- [Mandate API](https://github.com/Viincenttt/MollieApi/wiki/06.-Mandate-API)
- [Subscription API](https://github.com/Viincenttt/MollieApi/wiki/07.-Subscription-API)
- [Refund API](https://github.com/Viincenttt/MollieApi/wiki/04.-Refund-API)
- [Connect API](https://github.com/Viincenttt/MollieApi/wiki/10.-Connect-Api)
- Chargeback API (documentation coming soon)
- Invoice API (documentation coming soon)
- Permissions API (documentation coming soon)
- [Profile API](https://github.com/Viincenttt/MollieApi/wiki/11.-Profile-Api)
- [Organizations API](https://github.com/Viincenttt/MollieApi/wiki/09.-Organization-API)
- [Order API](https://github.com/Viincenttt/MollieApi/wiki/08.-Order-API)
- [Capture API](https://github.com/Viincenttt/MollieApi/wiki/12.-Captures-API)
- [Onboarding API](https://github.com/Viincenttt/MollieApi/wiki/13.-Onboarding-Api)
- [Balances API](https://github.com/Viincenttt/MollieApi/wiki/15.-Balances-Api)
- Terminal API (documentation coming soon)
- ClientLink API (documentation coming soon)
- Wallet API (documentation coming soon)
- Client API (documentation coming soon)
- Capability API (documentation coming soon)

---

## 🤝 Contributions
Spotted a bug or want to add a new feature? Contributions are welcome! Please target the latest `development` branch and include a clear description of your changes.

---

## ✅ Supported .NET Versions
This library targets [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard?tabs=net-standard-2-0), making it compatible with a wide range of platforms:
| .NET implementation  | Version support |
| ------------- | ------------- |
| .NET and .NET Core | 2.0, 2.1, 2.2, 3.0, 3.1, 5.0, 6.0, 7.0, 8.0, 9.0 |
| .NET Framework  | 4.6.1, 4.6.2, 4.7, 4.7.1, 4.7.2, 4.8, 4.8.1  |
| Mono | 5.4, 6.4  |
| Universal Windows Platform | 10.0.16299, TBD |
| Xamarin.iOS | 10.14, 12.16 |
| Xamarin.Mac | 3.8, 5.16 |
| Xamarin.Android | 8.0, 10.0 |
| Unity | 2018.1 |

> ⚠️ Note: This library uses the required keyword in some model classes. Your project must target **C# 11 or higher**.

