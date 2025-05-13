using System;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Models.Payment.Request.PaymentSpecificParameters;
using Mollie.Api.Models.Payment.Response.PaymentSpecificParameters;
using Xunit;

using SortDirection = Mollie.Api.Models.SortDirection;

namespace Mollie.Tests.Unit.Client;

public class PaymentClientTests : BaseClientTests {
    [Fact]
    public async Task CreatePaymentAsync_WithCustomIdempotencyKey_CustomIdemPotencyKeyIsSent()
    {
        // Given: We create a payment request with only the required parameters
        var paymentRequest = new PaymentRequest()
        {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = "http://www.mollie.com"
        };
        const string customIdempotencyKey1 = "my-idempotency-key-1";
        const string customIdempotencyKey2 = "my-idempotency-key-2";
        const string jsonToReturnInMockResponse = defaultPaymentJsonResponse;
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Post, $"{BaseMollieClient.DefaultBaseApiEndPoint}*")
            .WithHeaders("Idempotency-Key", customIdempotencyKey1)
            .Respond("application/json", jsonToReturnInMockResponse);
        mockHttp.Expect(HttpMethod.Post, $"{BaseMollieClient.DefaultBaseApiEndPoint}*")
            .WithHeaders("Idempotency-Key", customIdempotencyKey2)
            .Respond("application/json", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // Act
        using (paymentClient.WithIdempotencyKey(customIdempotencyKey1))
        {
            await paymentClient.CreatePaymentAsync(paymentRequest);
        }
        using (paymentClient.WithIdempotencyKey(customIdempotencyKey2))
        {
            await paymentClient.CreatePaymentAsync(paymentRequest);
        }

        // Assert
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task CreatePaymentAsync_PaymentWithRequiredParameters_ResponseIsDeserializedInExpectedFormat() {
        // Given: we create a payment request with only the required parameters
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = "http://www.mollie.com"
        };
        const string jsonToReturnInMockResponse = defaultPaymentJsonResponse;

        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.DefaultBaseApiEndPoint}*")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        AssertPaymentIsEqual(paymentRequest, paymentResponse);
        paymentResponse.AuthorizedAt!.Value.ToUniversalTime().ShouldBe(DateTime.SpecifyKind(new DateTime(2018, 3, 19, 13, 28, 37), DateTimeKind.Utc));
        paymentResponse.CreatedAt!.ToUniversalTime().ShouldBe(DateTime.SpecifyKind(new DateTime(2018, 3, 20, 13, 13, 37), DateTimeKind.Utc));
        paymentResponse.PaidAt!.Value.ToUniversalTime().ShouldBe(DateTime.SpecifyKind(new DateTime(2018, 3, 21, 13, 28, 37), DateTimeKind.Utc));
        paymentResponse.CanceledAt!.Value.ToUniversalTime().ShouldBe(DateTime.SpecifyKind(new DateTime(2018, 3, 22, 13, 28, 37), DateTimeKind.Utc));
        paymentResponse.ExpiredAt!.Value.ToUniversalTime().ShouldBe(DateTime.SpecifyKind(new DateTime(2018, 3, 23, 13, 28, 37), DateTimeKind.Utc));
        paymentResponse.FailedAt!.Value.ToUniversalTime().ShouldBe(DateTime.SpecifyKind(new DateTime(2018, 3, 24, 13, 28, 37), DateTimeKind.Utc));
        paymentResponse.CaptureBefore!.Value.ToUniversalTime().ShouldBe(DateTime.SpecifyKind(new DateTime(2018, 3, 25, 13, 28, 37), DateTimeKind.Utc));
        paymentResponse.AmountRefunded!.Value.ShouldBe("10.00");
        paymentResponse.AmountRefunded.Currency.ShouldBe(Currency.EUR);
        paymentResponse.AmountRemaining!.Value.ShouldBe("90.00");
        paymentResponse.AmountRemaining.Currency.ShouldBe(Currency.EUR);
        paymentResponse.AmountChargedBack!.Value.ShouldBe("10.00");
        paymentResponse.AmountChargedBack.Currency.ShouldBe(Currency.EUR);
        paymentResponse.CancelUrl.ShouldBe("https://webshop.example.org/order/12345/cancel");
        paymentResponse.CountryCode.ShouldBe("NL");
        paymentResponse.SettlementId.ShouldBe("stl_jDk30akdN");
        paymentResponse.SubscriptionId.ShouldBe("sub_rVKGtNd6s3");
        paymentResponse.ApplicationFee.ShouldNotBeNull();
        paymentResponse.ApplicationFee!.Amount.Value.ShouldBe("1.00");
        paymentResponse.ApplicationFee.Amount.Currency.ShouldBe(Currency.EUR);
        paymentResponse.ApplicationFee.Description.ShouldBe("description");
    }

    [Fact]
    public async Task CreatePaymentAsync_PaymentWithSinglePaymentMethod_RequestIsSerializedInExpectedFormat() {
        // Given: We create a payment request with a single payment method
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = "http://www.mollie.com",
            Method = PaymentMethod.Ideal
        };
        string expectedPaymentMethodJson = $"\"method\":[\"{PaymentMethod.Ideal}\"";
        const string jsonResponse = @"{
                ""amount"":{
                    ""currency"":""EUR"",
                    ""value"":""100.00""
                },
                ""description"":""Description"",
                ""method"":""ideal"",
                ""redirectUrl"":""http://www.mollie.com""}";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments", jsonResponse, expectedPaymentMethodJson);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        AssertPaymentIsEqual(paymentRequest, paymentResponse);
        paymentResponse.Method.ShouldBe(paymentRequest.Method);
    }

    [Fact]
    public async Task CreatePaymentAsync_PaymentWithMultiplePaymentMethods_RequestIsSerializedInExpectedFormat() {
        // Given: We create a payment request with multiple payment methods
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = "http://www.mollie.com",
            Methods = new List<string>() {
                PaymentMethod.Ideal,
                PaymentMethod.CreditCard,
                PaymentMethod.DirectDebit
            }
        };
        string expectedPaymentMethodJson = $"\"method\":[\"{PaymentMethod.Ideal}\",\"{PaymentMethod.CreditCard}\",\"{PaymentMethod.DirectDebit}\"]";
        const string expectedJsonResponse = @"{
                ""amount"":{
                    ""currency"":""EUR"",
                    ""value"":""100.00""
                },
                ""description"":""Description"",
                ""method"": null,
                ""redirectUrl"":""http://www.mollie.com""}";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments", expectedJsonResponse, expectedPaymentMethodJson);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        AssertPaymentIsEqual(paymentRequest, paymentResponse);
        paymentResponse.Method.ShouldBeNull();
    }

    [Fact]
    public async Task CreatePayment_WithRoutingInformation_RequestIsSerializedInExpectedFormat() {
        // Given: We create a payment request with the routing request
        PaymentRoutingRequest routingRequest = new PaymentRoutingRequest {
            Amount = new Amount("EUR", 100),
            Destination = new RoutingDestination {
                Type = "organization",
                OrganizationId = "organization-id"
            },
            ReleaseDate = new DateTime(2022, 1, 14)
        };
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = "http://www.mollie.com",
            Routings = new List<PaymentRoutingRequest> {
                routingRequest
            }
        };
        string expectedRoutingInformation = $"\"routing\":[{{\"amount\":{{\"currency\":\"EUR\",\"value\":\"100.00\"}},\"destination\":{{\"type\":\"organization\",\"organizationId\":\"organization-id\"}},\"releaseDate\":\"2022-01-14\"}}]}}";
        const string expectedJsonResponse = @"{
                ""amount"":{
                    ""currency"":""EUR"",
                    ""value"":""100.00""
                },
                ""description"":""Description"",
                ""method"": null,
                ""redirectUrl"":""http://www.mollie.com"",
                ""routing"": [{
                        ""amount"": {
                            ""currency"": ""EUR"",
                            ""value"": ""100.00""
                        },
                        ""destination"": {
                            ""type"": ""organization"",
                            ""organizationId"": ""organization-id""
                        },
                        ""releaseDate"": ""2022-01-14""
                    }
                ]}";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments", expectedJsonResponse, expectedRoutingInformation);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        AssertPaymentIsEqual(paymentRequest, paymentResponse);
        paymentResponse.Method.ShouldBeNull();
    }

    [Fact]
    public async Task CreatePaymentAsync_IncludeQrCode_QueryStringContainsIncludeQrCodeParameter() {
        // Given: We make a request to create a payment and include the QR code
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = "http://www.mollie.com",
            Method = PaymentMethod.Ideal
        };
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments?include=details.qrCode", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.CreatePaymentAsync(paymentRequest, includeQrCode: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetPaymentAsync_NoIncludeParameters_RequestIsDeserializedInExpectedFormat() {
        // Given: We make a request to retrieve a payment without wanting any extra data
        const string paymentId = "tr_WDqYK6vllg";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var payment = await paymentClient.GetPaymentAsync(paymentId);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        payment.Resource.ShouldBe("payment");
        payment.Id.ShouldBe(paymentId);
        payment.Amount.Value.ShouldBe("100.00");
        payment.Amount.Currency.ShouldBe(Currency.EUR);
        payment.Description.ShouldBe("Description");
        payment.Method.ShouldBeNull();
        payment.Status.ShouldBe(PaymentStatus.Open);
        payment.IsCancelable.ShouldBe(false);
        payment.Locale.ShouldBe("nl_NL");
        payment.ExpiresAt!.Value.ToUniversalTime().ShouldBe(DateTime.SpecifyKind(new DateTime(2018, 3, 20, 13, 28, 37), DateTimeKind.Utc));
        payment.ProfileId.ShouldBe("pfl_QkEhN94Ba");
        payment.SequenceType.ShouldBe(SequenceType.OneOff);
        payment.RedirectUrl.ShouldBe("https://webshop.example.org/order/12345/");
        payment.WebhookUrl.ShouldBe("https://webshop.example.org/payments/webhook/");
        payment.Links.ShouldNotBeNull();
        payment.Links.Self.Href.ShouldBe("https://api.mollie.com/v2/payments/tr_WDqYK6vllg");
        payment.Links.Self.Type.ShouldBe("application/hal+json");
        payment.Links.Checkout!.Href.ShouldBe("https://www.mollie.com/payscreen/select-method/WDqYK6vllg");
        payment.Links.Checkout.Type.ShouldBe("text/html");
        payment.Links.Dashboard.Href.ShouldBe("https://www.mollie.com/dashboard/org_12345678/payments/tr_WDqYK6vllg");
        payment.Links.Dashboard.Type.ShouldBe("text/html");
        payment.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/v2/payments-api/get-payment");
        payment.Links.Documentation.Type.ShouldBe("text/html");
    }

    [Fact]
    public async Task GetPaymentAsync_ForBankTransferPayment_DetailsAreDeserialized()
    {
        // Given: We make a request to retrieve a bank transfer payment
        const string paymentId = "tr_WDqYK6vllg";
        const string jsonResponse = @"{
            ""resource"": ""payment"",
            ""id"": ""tr_WDqYK6vllg"",
            ""mode"": ""test"",
            ""createdAt"": ""2018-03-20T13:13:37+00:00"",
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""method"": ""banktransfer"",
            ""expiresAt"": ""2018-03-20T13:28:37+00:00"",
            ""details"": {
                ""bankName"": ""bank-name"",
                ""bankAccount"": ""bank-account"",
                ""bankBic"": ""bank-bic"",
                ""transferReference"": ""transfer-reference"",
                ""consumerName"": ""consumer-name"",
                ""consumerAccount"": ""consumer-account"",
                ""consumerBic"": ""consumer-bic"",
                ""billingEmail"": ""billing-email"",
                ""qrCode"":{
                    ""height"": 5,
                    ""width"": 10,
                    ""src"": ""https://www.mollie.com/qr/12345678.png""
                }
            },
            ""_links"": {
                ""status"": {
                    ""href"": ""https://api.mollie.com/v2/payments/tr_WDqYK6vllg"",
                    ""type"": ""application/hal+json""
                },
                ""payOnline"": {
                    ""href"": ""https://www.mollie.com/payscreen/select-method/WDqYK6vllg"",
                    ""type"": ""text/html""
                }
            }
        }";

        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var payment = await paymentClient.GetPaymentAsync(paymentId);

        // Then
        payment.ShouldBeOfType<BankTransferPaymentResponse>();
        var bankTransferPayment = payment as BankTransferPaymentResponse;
        bankTransferPayment!.Details!.BankName.ShouldBe("bank-name");
        bankTransferPayment.Details.BankAccount.ShouldBe("bank-account");
        bankTransferPayment.Details.BankBic.ShouldBe("bank-bic");
        bankTransferPayment.Details.TransferReference.ShouldBe("transfer-reference");
        bankTransferPayment.Details.ConsumerName.ShouldBe("consumer-name");
        bankTransferPayment.Details.ConsumerAccount.ShouldBe("consumer-account");
        bankTransferPayment.Details.ConsumerBic.ShouldBe("consumer-bic");
        bankTransferPayment.Details.BillingEmail.ShouldBe("billing-email");
        bankTransferPayment.Details.QrCode.ShouldNotBeNull();
        bankTransferPayment.Details.QrCode.Height.ShouldBe(5);
        bankTransferPayment.Details.QrCode.Width.ShouldBe(10);
        bankTransferPayment.Details.QrCode.Src.ShouldBe("https://www.mollie.com/qr/12345678.png");
        bankTransferPayment.Links.ShouldNotBeNull();
        bankTransferPayment.Links.Status.ShouldNotBeNull();
        bankTransferPayment.Links.Status.Href.ShouldBe("https://api.mollie.com/v2/payments/tr_WDqYK6vllg");
        bankTransferPayment.Links.Status.Type.ShouldBe("application/hal+json");
        bankTransferPayment.Links.PayOnline.ShouldNotBeNull();
        bankTransferPayment.Links.PayOnline.Href.ShouldBe("https://www.mollie.com/payscreen/select-method/WDqYK6vllg");
        bankTransferPayment.Links.PayOnline.Type.ShouldBe("text/html");
    }

    [Fact]
    public async Task GetPaymentAsync_ForBanContactPayment_DetailsAreDeserialized()
    {
        // Given: We make a request to retrieve a bancontact payment
        const string paymentId = "tr_WDqYK6vllg";
        const string jsonResponse = @"{
            ""resource"": ""payment"",
            ""id"": ""tr_WDqYK6vllg"",
            ""mode"": ""test"",
            ""createdAt"": ""2018-03-20T13:13:37+00:00"",
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""method"": ""bancontact"",
            ""expiresAt"": ""2018-03-20T13:28:37+00:00"",
            ""details"": {
                ""cardNumber"": ""1234567890123456"",
                ""cardFingerprint"": ""fingerprint"",
                ""qrCode"":{
                    ""height"": 5,
                    ""width"": 10,
                    ""src"": ""https://www.mollie.com/qr/12345678.png""
                },
                ""consumerName"": ""consumer-name"",
                ""consumerAccount"": ""consumer-account"",
                ""consumerBic"": ""consumer-bic"",
                ""failureReason"": ""failure-reason""
            }
        }";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId);

        // Then
        result.ShouldBeOfType<BancontactPaymentResponse>();
        var banContactPayment = result as BancontactPaymentResponse;
        banContactPayment!.Details!.CardNumber.ShouldBe("1234567890123456");
        banContactPayment.Details.QrCode.ShouldNotBeNull();
        banContactPayment.Details.QrCode.Height.ShouldBe(5);
        banContactPayment.Details.QrCode.Width.ShouldBe(10);
        banContactPayment.Details.QrCode.Src.ShouldBe("https://www.mollie.com/qr/12345678.png");
        banContactPayment.Details.ConsumerName.ShouldBe("consumer-name");
        banContactPayment.Details.ConsumerAccount.ShouldBe("consumer-account");
        banContactPayment.Details.ConsumerBic.ShouldBe("consumer-bic");
        banContactPayment.Details.FailureReason.ShouldBe("failure-reason");
    }

    [Fact]
    public async Task CreatePaymentAsync_SepaDirectDebit_RequestAndResponseAreConvertedToExpectedJsonFormat()
    {
        // Given we create a creditcard specific payment request
        var paymentRequest = new SepaDirectDebitRequest()
        {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            Method = PaymentMethod.Ideal,
            RedirectUrl = "http://www.mollie.com",
            WebhookUrl = "http://www.mollie.com/webhook",
            ConsumerName = "consumer-name",
            ConsumerAccount = "consumer-account"
        };
        const string jsonRequest = @"{
  ""consumerName"": ""consumer-name"",
  ""consumerAccount"": ""consumer-account"",
  ""amount"": {
    ""currency"": ""EUR"",
    ""value"": ""100.00""
  },
  ""description"": ""Description"",
  ""redirectUrl"": ""http://www.mollie.com"",
  ""webhookUrl"": ""http://www.mollie.com/webhook"",
  ""method"": [
    ""ideal""
  ]
}";
        const string jsonResponse = @"{
            ""resource"": ""payment"",
            ""id"": ""tr_WDqYK6vllg"",
            ""mode"": ""test"",
            ""createdAt"": ""2018-03-20T13:13:37+00:00"",
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""method"": ""directdebit"",
            ""expiresAt"": ""2018-03-20T13:28:37+00:00"",
            ""details"": {
                ""consumerName"": ""consumer-name"",
                ""consumerAccount"": ""consumer-account"",
                ""consumerBic"": ""consumer-bic"",
                ""transferReference"": ""transfer-reference"",
                ""bankReasonCode"": ""bank-reason-code"",
                ""bankReason"": ""bank-reason"",
                ""batchReference"": ""batch-reference"",
                ""mandateReference"": ""mandate-reference"",
                ""creditorIdentifier"": ""creditor-identifier"",
                ""dueDate"": ""2018-03-20"",
                ""signatureDate"": ""2018-03-20"",
                ""endToEndIdentifier"": ""end-to-end-identifier"",
                ""batchReference"": ""batch-reference"",
                ""fileReference"": ""file-reference""
            }
        }";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments", jsonResponse, jsonRequest);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var result = await paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        var specificPaymentResponse = result as SepaDirectDebitResponse;
        specificPaymentResponse.ShouldNotBeNull();
        specificPaymentResponse.Details!.ConsumerName.ShouldBe("consumer-name");
        specificPaymentResponse.Details.ConsumerAccount.ShouldBe("consumer-account");
        specificPaymentResponse.Details.ConsumerBic.ShouldBe("consumer-bic");
        specificPaymentResponse.Details.TransferReference.ShouldBe("transfer-reference");
        specificPaymentResponse.Details.BankReasonCode.ShouldBe("bank-reason-code");
        specificPaymentResponse.Details.BankReason.ShouldBe("bank-reason");
        specificPaymentResponse.Details.BatchReference.ShouldBe("batch-reference");
        specificPaymentResponse.Details.MandateReference.ShouldBe("mandate-reference");
        specificPaymentResponse.Details.CreditorIdentifier.ShouldBe("creditor-identifier");
        specificPaymentResponse.Details.DueDate.ShouldBe("03/20/2018 00:00:00");
        specificPaymentResponse.Details.SignatureDate.ShouldBe("03/20/2018 00:00:00");
        specificPaymentResponse.Details.EndToEndIdentifier.ShouldBe("end-to-end-identifier");
        specificPaymentResponse.Details.BatchReference.ShouldBe("batch-reference");
        specificPaymentResponse.Details.FileReference.ShouldBe("file-reference");
    }

    [Fact]
    public async Task GetPaymentAsync_ForPayPalPayment_DetailsAreDeserialized()
    {
        // Given: We make a request to retrieve a paypal payment
        const string paymentId = "tr_WDqYK6vllg";
        const string jsonResponse = @"{
            ""resource"": ""payment"",
            ""id"": ""tr_WDqYK6vllg"",
            ""mode"": ""test"",
            ""createdAt"": ""2018-03-20T13:13:37+00:00"",
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""method"": ""paypal"",
            ""expiresAt"": ""2018-03-20T13:28:37+00:00"",
            ""details"": {
                ""consumerName"": ""consumer-name"",
                ""consumerAccount"": ""consumer-account"",
                ""paypalReference"": ""paypal-ref"",
                ""paypalPayerId"": ""paypal-payer-id"",
                ""sellerProtection"": ""Eligible"",
                ""shippingAddress"": {
                    ""streetAndNumber"": ""street-and-number"",
                    ""streetAdditional"": ""street-additional"",
                    ""postalCode"": ""postal-code"",
                    ""city"": ""city"",
                    ""region"": ""region"",
                    ""country"": ""country""
                },
                ""paypalFee"": {
                    ""currency"": ""EUR"",
                    ""value"": ""100.00""
                }
            }
        }";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId);

        // Then
        result.ShouldBeOfType<PayPalPaymentResponse>();
        var payPalPayment = result as PayPalPaymentResponse;
        payPalPayment!.Details!.ConsumerName.ShouldBe("consumer-name");
        payPalPayment.Details.ConsumerAccount.ShouldBe("consumer-account");
        payPalPayment.Details.PayPalReference.ShouldBe("paypal-ref");
        payPalPayment.Details.PaypalPayerId.ShouldBe("paypal-payer-id");
        payPalPayment.Details.SellerProtection.ShouldBe("Eligible");
        payPalPayment.Details.ShippingAddress.ShouldNotBeNull();
        payPalPayment.Details.ShippingAddress.StreetAndNumber.ShouldBe("street-and-number");
        payPalPayment.Details.ShippingAddress.StreetAdditional.ShouldBe("street-additional");
        payPalPayment.Details.ShippingAddress.PostalCode.ShouldBe("postal-code");
        payPalPayment.Details.ShippingAddress.City.ShouldBe("city");
        payPalPayment.Details.ShippingAddress.Region.ShouldBe("region");
        payPalPayment.Details.ShippingAddress.Country.ShouldBe("country");
        payPalPayment.Details.PaypalFee.ShouldNotBeNull();
        payPalPayment.Details.PaypalFee.Currency.ShouldBe("EUR");
        payPalPayment.Details.PaypalFee.Value.ShouldBe("100.00");
    }

    [Fact]
    public async Task CreatePaymentAsync_CreditcardPayment_RequestAndResponseAreConvertedToExpectedJsonFormat()
    {
        // Given we create a creditcard specific payment request
        var paymentRequest = new CreditCardPaymentRequest()
        {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            Method = PaymentMethod.Ideal,
            RedirectUrl = "http://www.mollie.com",
            WebhookUrl = "http://www.mollie.com/webhook",
            BillingAddress = new PaymentAddressDetails()
            {
                City = "Amsterdam",
                Country = "NL",
                PostalCode = "1000AA",
                Region = "Noord-Holland",
                StreetAndNumber = "Keizersgracht 313"
            },
            ShippingAddress = new PaymentAddressDetails()
            {
                City = "Amsterdam",
                Country = "NL",
                PostalCode = "1000AA",
                Region = "Noord-Holland",
                StreetAndNumber = "Keizersgracht 313"
            },
            CardToken = "card-token"
        };
        const string jsonRequest = @"{
  ""cardToken"": ""card-token"",
  ""amount"": {
    ""currency"": ""EUR"",
    ""value"": ""100.00""
  },
  ""description"": ""Description"",
  ""redirectUrl"": ""http://www.mollie.com"",
  ""webhookUrl"": ""http://www.mollie.com/webhook"",
  ""billingAddress"": {
    ""streetAndNumber"": ""Keizersgracht 313"",
    ""postalCode"": ""1000AA"",
    ""city"": ""Amsterdam"",
    ""region"": ""Noord-Holland"",
    ""country"": ""NL""
  },
  ""shippingAddress"": {
    ""streetAndNumber"": ""Keizersgracht 313"",
    ""postalCode"": ""1000AA"",
    ""city"": ""Amsterdam"",
    ""region"": ""Noord-Holland"",
    ""country"": ""NL""
  },
  ""method"": [
    ""ideal""
  ]
}";
        const string jsonResponse = @"{
            ""resource"": ""payment"",
            ""id"": ""tr_WDqYK6vllg"",
            ""mode"": ""test"",
            ""createdAt"": ""2018-03-20T13:13:37+00:00"",
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""method"": ""creditcard"",
            ""expiresAt"": ""2018-03-20T13:28:37+00:00"",
            ""details"": {
                ""cardNumber"": ""1234567890123456"",
                ""cardHolder"": ""John Doe"",
                ""cardFingerprint"": ""fingerprint"",
                ""cardAudience"": ""audience"",
                ""cardLabel"": ""American Express"",
                ""cardCountryCode"": ""NL"",
                ""cardSecurity"": ""security"",
                ""feeRegion"": ""american-express"",
                ""failureReason"": ""unknown_reason"",
                ""failureMessage"": ""faulure-message"",
                ""wallet"": ""applepay""
            }
        }";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments", jsonResponse, jsonRequest);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new("abcde", httpClient);

        // When: We send the request
        var result = await paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        var specificPaymentResponse = result as CreditCardPaymentResponse;
        specificPaymentResponse.ShouldNotBeNull();
        specificPaymentResponse.Details!.CardNumber.ShouldBe("1234567890123456");
        specificPaymentResponse.Details.CardHolder.ShouldBe("John Doe");
        specificPaymentResponse.Details.CardFingerprint.ShouldBe("fingerprint");
        specificPaymentResponse.Details.CardAudience.ShouldBe("audience");
        specificPaymentResponse.Details.CardLabel.ShouldBe("American Express");
        specificPaymentResponse.Details.CardCountryCode.ShouldBe("NL");
        specificPaymentResponse.Details.CardSecurity.ShouldBe("security");
        specificPaymentResponse.Details.FeeRegion.ShouldBe("american-express");
        specificPaymentResponse.Details.FailureReason.ShouldBe("unknown_reason");
        specificPaymentResponse.Details.FailureMessage.ShouldBe("faulure-message");
        specificPaymentResponse.Details.Wallet.ShouldBe("applepay");
    }

    [Fact]
    public async Task CreatePaymentAsync_GiftcardPayment_RequestAndResponseAreConvertedToExpectedJsonFormat()
    {
        // Given we create a giftcard specific payment request
        var paymentRequest = new GiftcardPaymentRequest()
        {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            Method = PaymentMethod.Ideal,
            RedirectUrl = "http://www.mollie.com",
            WebhookUrl = "http://www.mollie.com/webhook",
            Issuer = "issuer",
            VoucherNumber = "voucher-number",
            VoucherPin = "voucher-pin"
        };
        const string jsonRequest = @"{
  ""voucherNumber"": ""voucher-number"",
  ""voucherPin"": ""voucher-pin"",
  ""amount"": {
    ""currency"": ""EUR"",
    ""value"": ""100.00""
  },
  ""description"": ""Description"",
  ""redirectUrl"": ""http://www.mollie.com"",
  ""webhookUrl"": ""http://www.mollie.com/webhook"",
  ""issuer"": ""issuer"",
  ""method"": [
    ""ideal""
  ]
}";
        const string jsonResponse = @"{
            ""resource"": ""payment"",
            ""id"": ""tr_WDqYK6vllg"",
            ""mode"": ""test"",
            ""createdAt"": ""2018-03-20T13:13:37+00:00"",
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""method"": ""giftcard"",
            ""expiresAt"": ""2018-03-20T13:28:37+00:00"",
            ""details"": {
                ""voucherNumber"": ""voucher-number"",
                ""giftcards"": [
                    {
                        ""issuer"": ""issuer"",
                        ""amount"": {
                            ""currency"": ""EUR"",
                            ""value"": ""100.00""
                        },
                        ""voucherNumber"": ""voucher-number""
                    }
                ],
                ""RemainderAmount"": {
                    ""currency"": ""EUR"",
                    ""value"": ""100.00""
                },
                ""RemainderMethod"": ""ideal""
            }
        }";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments", jsonResponse, jsonRequest);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var result = await paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        var specificPaymentResponse = result as GiftcardPaymentResponse;
        specificPaymentResponse.ShouldNotBeNull();
        specificPaymentResponse!.Details!.VoucherNumber.ShouldBe("voucher-number");
        specificPaymentResponse.Details.Giftcards.ShouldNotBeNull();
        specificPaymentResponse.Details.Giftcards.Count.ShouldBe(1);
        specificPaymentResponse.Details.Giftcards[0].Issuer.ShouldBe("issuer");
        specificPaymentResponse.Details.Giftcards[0].Amount.ShouldNotBeNull();
        specificPaymentResponse.Details.Giftcards[0].Amount.Currency.ShouldBe("EUR");
        specificPaymentResponse.Details.Giftcards[0].Amount.Value.ShouldBe("100.00");
        specificPaymentResponse.Details.Giftcards[0].VoucherNumber.ShouldBe("voucher-number");
        specificPaymentResponse.Details.RemainderAmount.ShouldNotBeNull();
        specificPaymentResponse.Details.RemainderAmount.Currency.ShouldBe("EUR");
        specificPaymentResponse.Details.RemainderAmount.Value.ShouldBe("100.00");
        specificPaymentResponse.Details.RemainderMethod.ShouldBe("ideal");
    }

    [Fact]
    public async Task GetPaymentAsync_ForBelfiusPayment_DetailsAreDeserialized()
    {
        // Given: We make a request to retrieve a belfius payment
        const string paymentId = "tr_WDqYK6vllg";
        const string jsonResponse = @"{
            ""resource"": ""payment"",
            ""id"": ""tr_WDqYK6vllg"",
            ""mode"": ""test"",
            ""createdAt"": ""2018-03-20T13:13:37+00:00"",
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""method"": ""belfius"",
            ""expiresAt"": ""2018-03-20T13:28:37+00:00"",
            ""details"": {
                ""consumerName"": ""consumer-name"",
                ""consumerAccount"": ""consumer-account"",
                ""consumerBic"": ""consumer-bic""
            }
        }";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId);

        // Then
        result.ShouldBeOfType<BelfiusPaymentResponse>();
        var belfiusPayment = result as BelfiusPaymentResponse;
        belfiusPayment!.Details!.ConsumerName.ShouldBe("consumer-name");
        belfiusPayment.Details.ConsumerAccount.ShouldBe("consumer-account");
        belfiusPayment.Details.ConsumerBic.ShouldBe("consumer-bic");
    }

    [Fact]
    public async Task GetPaymentAsync_ForIngHomePay_DetailsAreDeserialized()
    {
        // Given: We make a request to retrieve a ing home pay payment
        const string paymentId = "tr_WDqYK6vllg";
        const string jsonResponse = @"{
            ""resource"": ""payment"",
            ""id"": ""tr_WDqYK6vllg"",
            ""mode"": ""test"",
            ""createdAt"": ""2018-03-20T13:13:37+00:00"",
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""method"": ""inghomepay"",
            ""expiresAt"": ""2018-03-20T13:28:37+00:00"",
            ""details"": {
                ""consumerName"": ""consumer-name"",
                ""consumerAccount"": ""consumer-account"",
                ""consumerBic"": ""consumer-bic""
            }
        }";

        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId);

        // Then
        result.ShouldBeOfType<IngHomePayPaymentResponse>();
        var ingHomePayPayment = result as IngHomePayPaymentResponse;
        ingHomePayPayment!.Details!.ConsumerName.ShouldBe("consumer-name");
        ingHomePayPayment.Details.ConsumerAccount.ShouldBe("consumer-account");
        ingHomePayPayment.Details.ConsumerBic.ShouldBe("consumer-bic");
    }

    [Fact]
    public async Task GetPaymentAsync_ForKbcPayment_DetailsAreDeserialized()
    {
        // Given: We make a request to retrieve a ing home pay payment
        const string paymentId = "tr_WDqYK6vllg";
        const string jsonResponse = @"{
            ""resource"": ""payment"",
            ""id"": ""tr_WDqYK6vllg"",
            ""mode"": ""test"",
            ""createdAt"": ""2018-03-20T13:13:37+00:00"",
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""method"": ""kbc"",
            ""expiresAt"": ""2018-03-20T13:28:37+00:00"",
            ""details"": {
                ""consumerName"": ""consumer-name"",
                ""consumerAccount"": ""consumer-account"",
                ""consumerBic"": ""consumer-bic""
            }
        }";

        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId);

        // Then
        result.ShouldBeOfType<KbcPaymentResponse>();
        var kbcPayment = result as KbcPaymentResponse;
        kbcPayment!.Details!.ConsumerName.ShouldBe("consumer-name");
        kbcPayment.Details.ConsumerAccount.ShouldBe("consumer-account");
        kbcPayment.Details.ConsumerBic.ShouldBe("consumer-bic");
    }

    [Fact]
    public async Task GetPaymentAsync_ForSofortPayment_DetailsAreDeserialized()
    {
        // Given: We make a request to retrieve a ing home pay payment
        const string paymentId = "tr_WDqYK6vllg";
        const string jsonResponse = @"{
            ""resource"": ""payment"",
            ""id"": ""tr_WDqYK6vllg"",
            ""mode"": ""test"",
            ""createdAt"": ""2018-03-20T13:13:37+00:00"",
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""method"": ""sofort"",
            ""expiresAt"": ""2018-03-20T13:28:37+00:00"",
            ""details"": {
                ""consumerName"": ""consumer-name"",
                ""consumerAccount"": ""consumer-account"",
                ""consumerBic"": ""consumer-bic""
            }
        }";

        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId);

        // Then
        result.ShouldBeOfType<SofortPaymentResponse>();
        var sofortPayment = result as SofortPaymentResponse;
        sofortPayment!.Details!.ConsumerName.ShouldBe("consumer-name");
        sofortPayment.Details.ConsumerAccount.ShouldBe("consumer-account");
        sofortPayment.Details.ConsumerBic.ShouldBe("consumer-bic");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetPaymentAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string? paymentId) {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await paymentClient.GetPaymentAsync(paymentId));
#pragma warning restore CS8604 // Possible null reference argument.

        // Then
        exception.Message.ShouldBe("Required URL argument 'paymentId' is null or empty");
    }

    [Fact]
    public async Task GetPaymentAsync_IncludeQrCode_QueryStringContainsIncludeQrCodeParameter() {
        // Given: We make a request to retrieve a payment without wanting any extra data
        const string paymentId = "abcde";
        const string jsonResponse = @"{
            ""resource"": ""payment"",
            ""id"": ""tr_WDqYK6vllg"",
            ""amount"":{
                ""currency"":""EUR"",
                ""value"":""100.00""
            },
            ""description"":""Description"",
            ""method"": ""ideal"",
            ""details"": {
                ""qrCode"":{
                    ""height"": 5,
                    ""width"": 5,
                    ""src"": ""https://www.mollie.com/qr/12345678.png""
                }
            }
        }";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}?include=details.qrCode",
            jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId, includeQrCode: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.ShouldBeOfType<IdealPaymentResponse>();
        var paymentResponse = result as IdealPaymentResponse;
        paymentResponse!.Details!.QrCode.ShouldNotBeNull();
        paymentResponse.Details.QrCode.Height.ShouldBe(5);
        paymentResponse.Details.QrCode.Width.ShouldBe(5);
        paymentResponse.Details.QrCode.Src.ShouldBe("https://www.mollie.com/qr/12345678.png");
    }

    [Fact]
    public async Task GetPaymentAsync_IncludeRemainderDetails_QueryStringContainsIncludeRemainderDetailsParameter() {
        // Given: We make a request to retrieve a payment without wanting any extra data
        const string paymentId = "abcde";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}?include=details.remainderDetails", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.GetPaymentAsync(paymentId, includeRemainderDetails: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetPaymentListAsync_IncludeQrCode_QueryStringContainsIncludeQrCodeParameter() {
        // Given: We make a request to retrieve a payment without wanting any extra data
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments?include=details.qrCode", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.GetPaymentListAsync(includeQrCode: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetPaymentAsync_EmbedRefunds_QueryStringContainsEmbedRefundsParameter()
    {
        // Given: We make a request to retrieve a payment with embedded refunds
        const string paymentId = "abcde";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}?embed=refunds", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.GetPaymentAsync(paymentId, embedRefunds: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetPaymentListAsync_EmbedRefunds_QueryStringContainsEmbedRefundsParameter()
    {
        // Given: We make a request to retrieve a payment with embedded refunds
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments?embed=refunds", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.GetPaymentListAsync(embedRefunds: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetPaymentAsync_EmbedChargebacks_QueryStringContainsEmbedChargebacksParameter()
    {
        // Given: We make a request to retrieve a payment with embedded refunds
        const string paymentId = "abcde";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}?embed=chargebacks", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.GetPaymentAsync(paymentId, embedChargebacks: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetPaymentListAsync_EmbedChargebacks_QueryStringContainsEmbedChargebacksParameter()
    {
        // Given: We make a request to retrieve a payment with embedded refunds
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments?embed=chargebacks", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.GetPaymentListAsync(embedChargebacks: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Theory]
    [InlineData(null, "")]
    [InlineData(SortDirection.Desc, "?sort=desc")]
    [InlineData(SortDirection.Asc, "?sort=asc")]
    public async Task GetPaymentListAsync_AddSortDirection_QueryStringContainsSortDirection(SortDirection? sortDirection, string expectedQueryString)
    {
        // Given: We make a request to retrieve a payment with embedded refunds
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments{expectedQueryString}", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.GetPaymentListAsync(embedChargebacks: true, sort: sortDirection);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeletePaymentAsync_TestmodeIsTrue_RequestContainsTestmodeModel() {
        // Given: We make a request to retrieve a payment with embedded refunds
        const string paymentId = "payment-id";
        string expectedContent = "\"testmode\":true";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Delete, $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}", defaultPaymentJsonResponse, expectedContent);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.CancelPaymentAsync(paymentId, true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task ReleasePaymentAuthorizationAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string? paymentId) {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await paymentClient.ReleasePaymentAuthorization(paymentId));
#pragma warning restore CS8604 // Possible null reference argument.

        // Then
        exception.Message.ShouldBe("Required URL argument 'paymentId' is null or empty");
    }

    [Fact]
    public async Task ReleasePaymentAuthorizationAsync_WithTestModeParameter_QueryStringContainsTestModeParameter() {
        // Given: We make a request to retrieve a payment with a the test mode parameter
        const string paymentId = "abcde";
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Post,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}payments/{paymentId}/release-authorization?testmode=true",
            string.Empty);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.ReleasePaymentAuthorization(paymentId, testmode: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task DeletePaymentAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string? paymentId) {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await paymentClient.CancelPaymentAsync(paymentId));
#pragma warning restore CS8604 // Possible null reference argument.

        // Then
        exception.Message.ShouldBe("Required URL argument 'paymentId' is null or empty");
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task UpdatePaymentAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string? paymentId) {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        var paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
#pragma warning disable CS8604 // Possible null reference argument.
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await paymentClient.UpdatePaymentAsync(paymentId, new PaymentUpdateRequest()));
#pragma warning restore CS8604 // Possible null reference argument.

        // Then
        exception.Message.ShouldBe("Required URL argument 'paymentId' is null or empty");
    }

    private void AssertPaymentIsEqual(PaymentRequest paymentRequest, PaymentResponse paymentResponse) {
        paymentResponse.Amount.Value.ShouldBe(paymentRequest.Amount.Value);
        paymentResponse.Amount.Currency.ShouldBe(paymentRequest.Amount.Currency);
        paymentResponse.Description.ShouldBe(paymentRequest.Description);
        if (paymentRequest.Routings != null) {
            paymentResponse.Routings!.Count.ShouldBe(paymentRequest.Routings.Count);
            for (int i = 0; i < paymentRequest.Routings.Count; i++) {
                var paymentRequestRouting = paymentRequest.Routings[i];
                var paymentResponseRouting = paymentResponse.Routings[i];
                paymentResponseRouting.Amount.ShouldBe(paymentRequestRouting.Amount);
                paymentResponseRouting.Destination.Type.ShouldBe(paymentRequestRouting.Destination.Type);
                paymentResponseRouting.Destination.OrganizationId.ShouldBe(paymentRequestRouting.Destination.OrganizationId);
                paymentResponseRouting.ReleaseDate.ShouldBe(paymentRequestRouting.ReleaseDate);
            }
        }
    }

    private const string defaultPaymentJsonResponse = @"
{
    ""resource"": ""payment"",
    ""id"": ""tr_WDqYK6vllg"",
    ""mode"": ""test"",
    ""createdAt"": ""2018-03-20T13:13:37+00:00"",
    ""amount"":{
        ""currency"":""EUR"",
        ""value"":""100.00""
    },
    ""description"":""Description"",
    ""method"": null,
    ""metadata"": {
        ""order_id"": ""12345""
    },
    ""status"": ""open"",
    ""isCancelable"": false,
    ""locale"": ""nl_NL"",
    ""restrictPaymentMethodsToCountry"": ""NL"",
    ""expiresAt"": ""2018-03-20T13:28:37+00:00"",
    ""details"": null,
    ""profileId"": ""pfl_QkEhN94Ba"",
    ""sequenceType"": ""oneoff"",
    ""redirectUrl"": ""https://webshop.example.org/order/12345/"",
    ""webhookUrl"": ""https://webshop.example.org/payments/webhook/"",
    ""authorizedAt"": ""2018-03-19T13:28:37+00:00"",
    ""paidAt"": ""2018-03-21T13:28:37+00:00"",
    ""canceledAt"": ""2018-03-22T13:28:37+00:00"",
    ""expiredAt"": ""2018-03-23T13:28:37+00:00"",
    ""failedAt"": ""2018-03-24T13:28:37+00:00"",
    ""captureBefore"": ""2018-03-25T13:28:37+00:00"",
    ""amountRefunded"": {
        ""currency"": ""EUR"",
        ""value"": ""10.00""
    },
    ""amountRemaining"": {
        ""currency"": ""EUR"",
        ""value"": ""90.00""
    },
    ""amountChargedBack"": {
        ""currency"": ""EUR"",
        ""value"": ""10.00""
    },
    ""cancelUrl"": ""https://webshop.example.org/order/12345/cancel"",
    ""countryCode"": ""NL"",
    ""settlementId"": ""stl_jDk30akdN"",
    ""subscriptionId"": ""sub_rVKGtNd6s3"",
    ""applicationFee"": {
        ""amount"": {
            ""currency"": ""EUR"",
            ""value"": ""1.00""
        },
        ""description"": ""description""
    },
    ""_links"": {
        ""self"": {
            ""href"": ""https://api.mollie.com/v2/payments/tr_WDqYK6vllg"",
            ""type"": ""application/hal+json""
        },
        ""checkout"": {
            ""href"": ""https://www.mollie.com/payscreen/select-method/WDqYK6vllg"",
            ""type"": ""text/html""
        },
        ""dashboard"": {
            ""href"": ""https://www.mollie.com/dashboard/org_12345678/payments/tr_WDqYK6vllg"",
            ""type"": ""text/html""
        },
        ""documentation"": {
            ""href"": ""https://docs.mollie.com/reference/v2/payments-api/get-payment"",
            ""type"": ""text/html""
        }
    }
}";
}
