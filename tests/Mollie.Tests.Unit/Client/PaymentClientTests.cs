using System;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using RichardSzalay.MockHttp;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Extensions;
using Mollie.Api.Models.Payment.Response.Specific;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class PaymentClientTests : BaseClientTests {
    [Fact]
    public async Task CreatePaymentAsync_WithCustomIdempotencyKey_CustomIdemPotencyKeyIsSent()
    {
        // Given: We create a payment request with only the required parameters
        PaymentRequest paymentRequest = new PaymentRequest()
        {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = "http://www.mollie.com"
        };
        const string customIdempotencyKey = "my-idempotency-key";
        const string jsonToReturnInMockResponse = defaultPaymentJsonResponse;
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.Expect(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}*")
            .WithHeaders("Idempotency-Key", customIdempotencyKey)
            .Respond("application/json", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // Act
        using (paymentClient.WithIdempotencyKey(customIdempotencyKey))
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
        mockHttp.When($"{BaseMollieClient.ApiEndPoint}*")
            .With(request => request.Headers.Contains("Idempotency-Key"))
            .Respond("application/json", jsonToReturnInMockResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        this.AssertPaymentIsEqual(paymentRequest, paymentResponse);
        paymentResponse.AuthorizedAt!.Value.ToUniversalTime().Should().Be(DateTime.SpecifyKind(19.March(2018).At(13, 28, 37), DateTimeKind.Utc));
        paymentResponse.CreatedAt!.Value.ToUniversalTime().Should().Be(DateTime.SpecifyKind(20.March(2018).At(13, 13, 37), DateTimeKind.Utc));
        paymentResponse.PaidAt!.Value.ToUniversalTime().Should().Be(DateTime.SpecifyKind(21.March(2018).At(13, 28, 37), DateTimeKind.Utc));
        paymentResponse.CanceledAt!.Value.ToUniversalTime().Should().Be(DateTime.SpecifyKind(22.March(2018).At(13, 28, 37), DateTimeKind.Utc));
        paymentResponse.ExpiredAt!.Value.ToUniversalTime().Should().Be(DateTime.SpecifyKind(23.March(2018).At(13, 28, 37), DateTimeKind.Utc));
        paymentResponse.FailedAt!.Value.ToUniversalTime().Should().Be(DateTime.SpecifyKind(24.March(2018).At(13, 28, 37), DateTimeKind.Utc));
        paymentResponse.CaptureBefore!.Value.ToUniversalTime().Should().Be(DateTime.SpecifyKind(25.March(2018).At(13, 28, 37), DateTimeKind.Utc));
        paymentResponse.AmountRefunded.Value.Should().Be("10.00");
        paymentResponse.AmountRefunded.Currency.Should().Be(Currency.EUR);
        paymentResponse.AmountRemaining.Value.Should().Be("90.00");
        paymentResponse.AmountRemaining.Currency.Should().Be(Currency.EUR);
        paymentResponse.AmountChargedBack.Value.Should().Be("10.00");
        paymentResponse.AmountChargedBack.Currency.Should().Be(Currency.EUR);
        paymentResponse.CancelUrl.Should().Be("https://webshop.example.org/order/12345/cancel");
        paymentResponse.CountryCode.Should().Be("NL");
        paymentResponse.SettlementId.Should().Be("stl_jDk30akdN");
        paymentResponse.SubscriptionId.Should().Be("sub_rVKGtNd6s3");
        paymentResponse.ApplicationFee.Should().NotBeNull();
        paymentResponse.ApplicationFee.Amount.Value.Should().Be("1.00");
        paymentResponse.ApplicationFee.Amount.Currency.Should().Be(Currency.EUR);
        paymentResponse.ApplicationFee.Description.Should().Be("description");
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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments", jsonResponse, expectedPaymentMethodJson);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        this.AssertPaymentIsEqual(paymentRequest, paymentResponse);
        paymentResponse.Method.Should().Be(paymentRequest.Method);
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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments", expectedJsonResponse, expectedPaymentMethodJson);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        this.AssertPaymentIsEqual(paymentRequest, paymentResponse);
        paymentResponse.Method.Should().BeNull();
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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments", expectedJsonResponse, expectedRoutingInformation);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);
            
        // When: We send the request
        PaymentResponse paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        this.AssertPaymentIsEqual(paymentRequest, paymentResponse);
        paymentResponse.Method.Should().BeNull();
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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments?include=details.qrCode", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.CreatePaymentAsync(paymentRequest, includeQrCode: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetPaymentAsync_NoIncludeParameters_RequestIsDeserializedInExpectedFormat() {
        // Given: We make a request to retrieve a payment without wanting any extra data
        const string paymentId = "tr_WDqYK6vllg";
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var payment = await paymentClient.GetPaymentAsync(paymentId);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        payment.Resource.Should().Be("payment");
        payment.Id.Should().Be(paymentId);
        payment.Amount.Value.Should().Be("100.00");
        payment.Amount.Currency.Should().Be(Currency.EUR);
        payment.Description.Should().Be("Description");
        payment.Method.Should().BeNull();
        payment.Status.Should().Be(PaymentStatus.Open);
        payment.IsCancelable.Should().BeFalse();
        payment.Locale.Should().Be("nl_NL");
        payment.ExpiresAt.Value.ToUniversalTime().Should().Be(DateTime.SpecifyKind(20.March(2018).At(13, 28, 37), DateTimeKind.Utc));
        payment.ProfileId.Should().Be("pfl_QkEhN94Ba");
        payment.SequenceType.Should().Be(SequenceType.OneOff);
        payment.RedirectUrl.Should().Be("https://webshop.example.org/order/12345/");
        payment.WebhookUrl.Should().Be("https://webshop.example.org/payments/webhook/");
        payment.Links.Should().NotBeNull();
        payment.Links.Self.Href.Should().Be("https://api.mollie.com/v2/payments/tr_WDqYK6vllg");
        payment.Links.Self.Type.Should().Be("application/hal+json");
        payment.Links.Checkout.Href.Should().Be("https://www.mollie.com/payscreen/select-method/WDqYK6vllg");
        payment.Links.Checkout.Type.Should().Be("text/html");
        payment.Links.Dashboard.Href.Should().Be("https://www.mollie.com/dashboard/org_12345678/payments/tr_WDqYK6vllg");
        payment.Links.Dashboard.Type.Should().Be("text/html");
        payment.Links.Documentation.Href.Should().Be("https://docs.mollie.com/reference/v2/payments-api/get-payment");
        payment.Links.Documentation.Type.Should().Be("text/html");
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
        
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var payment = await paymentClient.GetPaymentAsync(paymentId);
        
        // Then
        payment.Should().BeOfType<BankTransferPaymentResponse>();
        var bankTransferPayment = payment as BankTransferPaymentResponse;
        bankTransferPayment.Details.BankName.Should().Be("bank-name");
        bankTransferPayment.Details.BankAccount.Should().Be("bank-account");
        bankTransferPayment.Details.BankBic.Should().Be("bank-bic");
        bankTransferPayment.Details.TransferReference.Should().Be("transfer-reference");
        bankTransferPayment.Details.ConsumerName.Should().Be("consumer-name");
        bankTransferPayment.Details.ConsumerAccount.Should().Be("consumer-account");
        bankTransferPayment.Details.ConsumerBic.Should().Be("consumer-bic");
        bankTransferPayment.Details.BillingEmail.Should().Be("billing-email");
        bankTransferPayment.Details.QrCode.Should().NotBeNull();
        bankTransferPayment.Details.QrCode.Height.Should().Be(5);
        bankTransferPayment.Details.QrCode.Width.Should().Be(10);
        bankTransferPayment.Details.QrCode.Src.Should().Be("https://www.mollie.com/qr/12345678.png");
        bankTransferPayment.Links.Should().NotBeNull();
        bankTransferPayment.Links.Status.Should().NotBeNull();
        bankTransferPayment.Links.Status.Href.Should().Be("https://api.mollie.com/v2/payments/tr_WDqYK6vllg");
        bankTransferPayment.Links.Status.Type.Should().Be("application/hal+json");
        bankTransferPayment.Links.PayOnline.Should().NotBeNull();
        bankTransferPayment.Links.PayOnline.Href.Should().Be("https://www.mollie.com/payscreen/select-method/WDqYK6vllg");
        bankTransferPayment.Links.PayOnline.Type.Should().Be("text/html");
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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);
        
        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId);
        
        // Then
        result.Should().BeOfType<BancontactPaymentResponse>();
        var banContactPayment = result as BancontactPaymentResponse;
        banContactPayment.Details.CardNumber.Should().Be("1234567890123456");
        banContactPayment.Details.CardFingerprint.Should().Be("fingerprint");
        banContactPayment.Details.QrCode.Should().NotBeNull();
        banContactPayment.Details.QrCode.Height.Should().Be(5);
        banContactPayment.Details.QrCode.Width.Should().Be(10);
        banContactPayment.Details.QrCode.Src.Should().Be("https://www.mollie.com/qr/12345678.png");
        banContactPayment.Details.ConsumerName.Should().Be("consumer-name");
        banContactPayment.Details.ConsumerAccount.Should().Be("consumer-account");
        banContactPayment.Details.ConsumerBic.Should().Be("consumer-bic");
        banContactPayment.Details.FailureReason.Should().Be("failure-reason");
    }
    
    [Fact]
    public async Task CreatePaymentAsync_SepaDirectDebit_RequestAndResponseAreConvertedToExpectedJsonFormat()
    {
        // Given we create a creditcard specific payment request
        var paymentRequest = new SepaDirectDebitRequest()
        {
            Amount = new Amount() { Currency = Currency.EUR, Value = "100.00" },
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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments", jsonResponse, jsonRequest);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);
        
        // When: We send the request
        var result = await paymentClient.CreatePaymentAsync(paymentRequest);
        
        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        var specificPaymentResponse = result as SepaDirectDebitResponse;
        specificPaymentResponse.Should().NotBeNull();
        specificPaymentResponse.Details.ConsumerName.Should().Be("consumer-name");
        specificPaymentResponse.Details.ConsumerAccount.Should().Be("consumer-account");
        specificPaymentResponse.Details.ConsumerBic.Should().Be("consumer-bic");
        specificPaymentResponse.Details.TransferReference.Should().Be("transfer-reference");
        specificPaymentResponse.Details.BankReasonCode.Should().Be("bank-reason-code");
        specificPaymentResponse.Details.BankReason.Should().Be("bank-reason");
        specificPaymentResponse.Details.BatchReference.Should().Be("batch-reference");
        specificPaymentResponse.Details.MandateReference.Should().Be("mandate-reference");
        specificPaymentResponse.Details.CreditorIdentifier.Should().Be("creditor-identifier");
        specificPaymentResponse.Details.DueDate.Should().Be("03/20/2018 00:00:00");
        specificPaymentResponse.Details.SignatureDate.Should().Be("03/20/2018 00:00:00");
        specificPaymentResponse.Details.EndToEndIdentifier.Should().Be("end-to-end-identifier");
        specificPaymentResponse.Details.BatchReference.Should().Be("batch-reference");
        specificPaymentResponse.Details.FileReference.Should().Be("file-reference");
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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get,
            $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId);

        // Then
        result.Should().BeOfType<PayPalPaymentResponse>();
        var payPalPayment = result as PayPalPaymentResponse;
        payPalPayment.Details.ConsumerName.Should().Be("consumer-name");
        payPalPayment.Details.ConsumerAccount.Should().Be("consumer-account");
        payPalPayment.Details.PayPalReference.Should().Be("paypal-ref");
        payPalPayment.Details.PaypalPayerId.Should().Be("paypal-payer-id");
        payPalPayment.Details.SellerProtection.Should().Be("Eligible");
        payPalPayment.Details.ShippingAddress.Should().NotBeNull();
        payPalPayment.Details.ShippingAddress.StreetAndNumber.Should().Be("street-and-number");
        payPalPayment.Details.ShippingAddress.StreetAdditional.Should().Be("street-additional");
        payPalPayment.Details.ShippingAddress.PostalCode.Should().Be("postal-code");
        payPalPayment.Details.ShippingAddress.City.Should().Be("city");
        payPalPayment.Details.ShippingAddress.Region.Should().Be("region");
        payPalPayment.Details.ShippingAddress.Country.Should().Be("country");
        payPalPayment.Details.PaypalFee.Should().NotBeNull();
        payPalPayment.Details.PaypalFee.Currency.Should().Be("EUR");
        payPalPayment.Details.PaypalFee.Value.Should().Be("100.00");
    }
    
    [Fact]
    public async Task CreatePaymentAsync_CreditcardPayment_RequestAndResponseAreConvertedToExpectedJsonFormat()
    {
        // Given we create a creditcard specific payment request
        var paymentRequest = new CreditCardPaymentRequest()
        {
            Amount = new Amount() { Currency = Currency.EUR, Value = "100.00" },
            Description = "Description",
            Method = PaymentMethod.Ideal,
            RedirectUrl = "http://www.mollie.com",
            WebhookUrl = "http://www.mollie.com/webhook",
            BillingAddress = new AddressObject()
            {
                City = "Amsterdam",
                Country = "NL",
                PostalCode = "1000AA",
                Region = "Noord-Holland",
                StreetAndNumber = "Keizersgracht 313"
            },
            ShippingAddress = new AddressObject()
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
  ""cardToken"": ""card-token"",
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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments", jsonResponse, jsonRequest);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);
        
        // When: We send the request
        var result = await paymentClient.CreatePaymentAsync(paymentRequest);
        
        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        var specificPaymentResponse = result as CreditCardPaymentResponse;
        specificPaymentResponse.Should().NotBeNull();
        specificPaymentResponse.Details.CardNumber.Should().Be("1234567890123456");
        specificPaymentResponse.Details.CardHolder.Should().Be("John Doe");
        specificPaymentResponse.Details.CardFingerprint.Should().Be("fingerprint");
        specificPaymentResponse.Details.CardAudience.Should().Be("audience");
        specificPaymentResponse.Details.CardLabel.Should().Be("American Express");
        specificPaymentResponse.Details.CardCountryCode.Should().Be("NL");
        specificPaymentResponse.Details.CardSecurity.Should().Be("security");
        specificPaymentResponse.Details.FeeRegion.Should().Be("american-express");
        specificPaymentResponse.Details.FailureReason.Should().Be("unknown_reason");
        specificPaymentResponse.Details.FailureMessage.Should().Be("faulure-message");
        specificPaymentResponse.Details.Wallet.Should().Be("applepay");
    }
    
    [Fact]
    public async Task CreatePaymentAsync_GiftcardPayment_RequestAndResponseAreConvertedToExpectedJsonFormat()
    {
        // Given we create a giftcard specific payment request
        var paymentRequest = new GiftcardPaymentRequest()
        {
            Amount = new Amount() { Currency = Currency.EUR, Value = "100.00" },
            Description = "Description",
            Method = PaymentMethod.Ideal,
            RedirectUrl = "http://www.mollie.com",
            WebhookUrl = "http://www.mollie.com/webhook",
            Issuer = "issuer",
            VoucherNumber = "voucher-number",
            VoucherPin = "voucher-pin"
        };
        const string jsonRequest = @"{
  ""issuer"": ""issuer"",
  ""voucherNumber"": ""voucher-number"",
  ""voucherPin"": ""voucher-pin"",
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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments", jsonResponse, jsonRequest);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);
        
        // When: We send the request
        var result = await paymentClient.CreatePaymentAsync(paymentRequest);
        
        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        var specificPaymentResponse = result as GiftcardPaymentResponse;
        specificPaymentResponse.Should().NotBeNull();
        specificPaymentResponse.Details.VoucherNumber.Should().Be("voucher-number");
        specificPaymentResponse.Details.Giftcards.Should().NotBeNull();
        specificPaymentResponse.Details.Giftcards.Count.Should().Be(1);
        specificPaymentResponse.Details.Giftcards[0].Issuer.Should().Be("issuer");
        specificPaymentResponse.Details.Giftcards[0].Amount.Should().NotBeNull();
        specificPaymentResponse.Details.Giftcards[0].Amount.Currency.Should().Be("EUR");
        specificPaymentResponse.Details.Giftcards[0].Amount.Value.Should().Be("100.00");
        specificPaymentResponse.Details.Giftcards[0].VoucherNumber.Should().Be("voucher-number");
        specificPaymentResponse.Details.RemainderAmount.Should().NotBeNull();
        specificPaymentResponse.Details.RemainderAmount.Currency.Should().Be("EUR");
        specificPaymentResponse.Details.RemainderAmount.Value.Should().Be("100.00");
        specificPaymentResponse.Details.RemainderMethod.Should().Be("ideal");
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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);
        
        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId);
        
        // Then
        result.Should().BeOfType<BelfiusPaymentResponse>();
        var belfiusPayment = result as BelfiusPaymentResponse;
        belfiusPayment!.Details.ConsumerName.Should().Be("consumer-name");
        belfiusPayment.Details.ConsumerAccount.Should().Be("consumer-account");
        belfiusPayment.Details.ConsumerBic.Should().Be("consumer-bic");
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
        
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);
        
        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId);
        
        // Then
        result.Should().BeOfType<IngHomePayPaymentResponse>();
        var ingHomePayPayment = result as IngHomePayPaymentResponse;
        ingHomePayPayment!.Details.ConsumerName.Should().Be("consumer-name");
        ingHomePayPayment.Details.ConsumerAccount.Should().Be("consumer-account");
        ingHomePayPayment.Details.ConsumerBic.Should().Be("consumer-bic");
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
        
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);
        
        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId);
        
        // Then
        result.Should().BeOfType<KbcPaymentResponse>();
        var kbcPayment = result as KbcPaymentResponse;
        kbcPayment!.Details.ConsumerName.Should().Be("consumer-name");
        kbcPayment.Details.ConsumerAccount.Should().Be("consumer-account");
        kbcPayment.Details.ConsumerBic.Should().Be("consumer-bic");
    }

    [Fact]
    public async Task CreatePaymentAsync_IdealPayment_RequestAndResponseAreConvertedToExpectedJsonFormat()
    {
        // Given we create a ideal specific payment request
        var paymentRequest = new IdealPaymentRequest()
        {
            Amount = new Amount() { Currency = Currency.EUR, Value = "100.00" },
            Description = "Description",
            Method = PaymentMethod.Ideal,
            RedirectUrl = "http://www.mollie.com",
            WebhookUrl = "http://www.mollie.com/webhook",
            Issuer = "ideal_INGBNL2A"
        };
        const string jsonRequest = @"{
  ""issuer"": ""ideal_INGBNL2A"",
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
            ""method"": ""ideal"",
            ""expiresAt"": ""2018-03-20T13:28:37+00:00"",
            ""details"": {
                ""consumerName"": ""consumer-name"",
                ""consumerAccount"": ""consumer-account"",
                ""consumerBic"": ""consumer-bic"",
                ""qrCode"": {
                    ""height"": 5,
                    ""width"": 10,
                    ""src"": ""https://www.mollie.com/qr/12345678.png""
                }
            }
        }";
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Post, $"{BaseMollieClient.ApiEndPoint}payments", jsonResponse, jsonRequest);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);
        
        // When: We send the request
        var result = await paymentClient.CreatePaymentAsync(paymentRequest);
        
        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        var specificPaymentResponse = result as IdealPaymentResponse;
        specificPaymentResponse!.Details.ConsumerName.Should().Be("consumer-name");
        specificPaymentResponse.Details.ConsumerAccount.Should().Be("consumer-account");
        specificPaymentResponse.Details.ConsumerBic.Should().Be("consumer-bic");
        specificPaymentResponse.Details.QrCode.Should().NotBeNull();
        specificPaymentResponse.Details.QrCode.Height.Should().Be(5);
        specificPaymentResponse.Details.QrCode.Width.Should().Be(10);
        specificPaymentResponse.Details.QrCode.Src.Should().Be("https://www.mollie.com/qr/12345678.png");
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
        
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}", jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);
        
        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId);
        
        // Then
        result.Should().BeOfType<SofortPaymentResponse>();
        var sofortPayment = result as SofortPaymentResponse;
        sofortPayment!.Details.ConsumerName.Should().Be("consumer-name");
        sofortPayment.Details.ConsumerAccount.Should().Be("consumer-account");
        sofortPayment.Details.ConsumerBic.Should().Be("consumer-bic");
    }
        
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task GetPaymentAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await paymentClient.GetPaymentAsync(paymentId));

        // Then
        exception.Message.Should().Be("Required URL argument 'paymentId' is null or empty");
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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, 
            $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}?include=details.qrCode",
            jsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var result = await paymentClient.GetPaymentAsync(paymentId, includeQrCode: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.Should().BeOfType<IdealPaymentResponse>();
        var paymentResponse = result as IdealPaymentResponse;
        paymentResponse.Details.QrCode.Should().NotBeNull();
        paymentResponse.Details.QrCode.Height.Should().Be(5);
        paymentResponse.Details.QrCode.Width.Should().Be(5);
        paymentResponse.Details.QrCode.Src.Should().Be("https://www.mollie.com/qr/12345678.png");
    }

    [Fact]
    public async Task GetPaymentAsync_IncludeRemainderDetails_QueryStringContainsIncludeRemainderDetailsParameter() {
        // Given: We make a request to retrieve a payment without wanting any extra data
        const string paymentId = "abcde";
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}?include=details.remainderDetails", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.GetPaymentAsync(paymentId, includeRemainderDetails: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetPaymentListAsync_IncludeQrCode_QueryStringContainsIncludeQrCodeParameter() {
        // Given: We make a request to retrieve a payment without wanting any extra data
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments?include=details.qrCode", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}?embed=refunds", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.GetPaymentAsync(paymentId, embedRefunds: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetPaymentListAsync_EmbedRefunds_QueryStringContainsEmbedRefundsParameter()
    {
        // Given: We make a request to retrieve a payment with embedded refunds
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments?embed=refunds", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}?embed=chargebacks", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.GetPaymentAsync(paymentId, embedChargebacks: true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task GetPaymentListAsync_EmbedChargebacks_QueryStringContainsEmbedChargebacksParameter()
    {
        // Given: We make a request to retrieve a payment with embedded refunds
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments?embed=chargebacks", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, $"{BaseMollieClient.ApiEndPoint}payments{expectedQueryString}", defaultPaymentJsonResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

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
        var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Delete, $"{BaseMollieClient.ApiEndPoint}payments/{paymentId}", defaultPaymentJsonResponse, expectedContent);
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        await paymentClient.DeletePaymentAsync(paymentId, true);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
    }
        
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task DeletePaymentAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await paymentClient.DeletePaymentAsync(paymentId));

        // Then
        exception.Message.Should().Be("Required URL argument 'paymentId' is null or empty");
    }
        
    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public async Task UpdatePaymentAsync_NoPaymentIdIsGiven_ArgumentExceptionIsThrown(string paymentId) {
        // Arrange
        var mockHttp = new MockHttpMessageHandler();
        HttpClient httpClient = mockHttp.ToHttpClient();
        PaymentClient paymentClient = new PaymentClient("abcde", httpClient);

        // When: We send the request
        var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await paymentClient.UpdatePaymentAsync(paymentId, new PaymentUpdateRequest()));

        // Then
        exception.Message.Should().Be("Required URL argument 'paymentId' is null or empty");
    }

    private void AssertPaymentIsEqual(PaymentRequest paymentRequest, PaymentResponse paymentResponse) {
        paymentResponse.Amount.Value.Should().Be(paymentRequest.Amount.Value);
        paymentResponse.Amount.Currency.Should().Be(paymentRequest.Amount.Currency);
        paymentResponse.Description.Should().Be(paymentRequest.Description);
        if (paymentRequest.Routings != null) {
            paymentResponse.Routings.Count.Should().Be(paymentRequest.Routings.Count);
            for (int i = 0; i < paymentRequest.Routings.Count; i++) {
                var paymentRequestRouting = paymentRequest.Routings[i];
                var paymentResponseRouting = paymentResponse.Routings[i];
                paymentResponseRouting.Amount.Should().Be(paymentRequestRouting.Amount);
                paymentResponseRouting.Destination.Type.Should().Be(paymentRequestRouting.Destination.Type);
                paymentResponseRouting.Destination.OrganizationId.Should().Be(paymentRequestRouting.Destination.OrganizationId);
                paymentResponseRouting.ReleaseDate.Should().Be(paymentRequestRouting.ReleaseDate);
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