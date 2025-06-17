using System;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Capture;
using Mollie.Api.Models.Capture.Request;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Tests.Integration.Framework;
using System.Collections.Generic;
using System.Linq;
using Shouldly;
using Mollie.Api.Models.Capture.Response;
using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Mandate.Response;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Payment.Request.PaymentSpecificParameters;
using Mollie.Api.Models.Payment.Response.PaymentSpecificParameters;
using Mollie.Api.Models.Terminal.Response;
using Xunit;
using SortDirection = Mollie.Api.Models.SortDirection;

namespace Mollie.Tests.Integration.Api;

public class PaymentTests : BaseMollieApiTestClass, IDisposable {
    private readonly IPaymentClient _paymentClient;
    private readonly ICustomerClient _customerClient;
    private readonly IMandateClient _mandateClient;
    private readonly ITerminalClient _terminalClient;
    private readonly ICaptureClient _captureClient;

    private readonly IHttpClientFactory _httpClientFactory;

    public PaymentTests(
        IPaymentClient paymentClient,
        ICustomerClient customerClient,
        IMandateClient mandateClient,
        ITerminalClient terminalClient,
        ICaptureClient captureClient,
        IHttpClientFactory httpClientFactory) {
        _paymentClient = paymentClient;
        _customerClient = customerClient;
        _mandateClient = mandateClient;
        _terminalClient = terminalClient;
        _captureClient = captureClient;
        _httpClientFactory = httpClientFactory;
    }

    [Fact]
    public async Task CanRetrievePaymentList() {
        // When: Retrieve payment list with default settings
        ListResponse<PaymentResponse> response = await _paymentClient.GetPaymentListAsync();

        // Then
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
        response.Items.Select(x => x.CreatedAt).ShouldBeInOrder(Shouldly.SortDirection.Descending);
    }

    [Fact]
    public async Task CanRetrievePaymentListInDescendingOrder()
    {
        // When: Retrieve payment list in ascending order
        ListResponse<PaymentResponse> response = await _paymentClient.GetPaymentListAsync(sort: SortDirection.Desc);

        // Then
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
        response.Items.Select(x => x.CreatedAt).ShouldBeInOrder(Shouldly.SortDirection.Descending);
    }

    [Fact]
    public async Task CanRetrievePaymentListInAscendingOrder()
    {
        // When: Retrieve payment list in ascending order
        ListResponse<PaymentResponse> response = await _paymentClient.GetPaymentListAsync(sort: SortDirection.Asc);

        // Then
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
        response.Items.Select(x => x.CreatedAt).ShouldBeInOrder(Shouldly.SortDirection.Ascending);
    }

    [Fact]
    public async Task ListPaymentsNeverReturnsMorePaymentsThenTheNumberOfRequestedPayments() {
        // Given: Number of payments requested is 5
        int numberOfPayments = 5;

        // When: Retrieve 5 payments
        ListResponse<PaymentResponse> response = await _paymentClient.GetPaymentListAsync(null, numberOfPayments);

        // Then
        response.Items.Count.ShouldBeLessThanOrEqualTo(numberOfPayments);
    }

    [Fact]
    public async Task CanCreateDefaultPaymentWithOnlyRequiredFields() {
        // Given: we create a payment request with only the required parameters
        var paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };

        // When: We send the payment request to Mollie
        PaymentResponse result = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // Then: Make sure we get a valid response
        result.ShouldNotBeNull();
        result.Amount.ShouldBe(paymentRequest.Amount);
        result.Description.ShouldBe(paymentRequest.Description);
        result.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
    }

    [Fact]
    public async Task CanCreateDefaultPaymentWithCustomIdempotencyKey() {
        // Given: we create a payment request with only the required parameters
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };

        // When: We send the payment request to Mollie
        using (_paymentClient.WithIdempotencyKey("my-idempotency-key"))
        {
            PaymentResponse firstAttempt = await _paymentClient.CreatePaymentAsync(paymentRequest);
            PaymentResponse secondAttempt = await _paymentClient.CreatePaymentAsync(paymentRequest);

            // Then: Make sure the responses have the same payment Id
            firstAttempt.Id.ShouldBe(secondAttempt.Id);
        }
    }

    [Fact]
    public async Task CanCreateDefaultPaymentWithAllFields() {
        // Given: we create a payment request where all parameters have a value
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Locale = Locale.nl_NL,
            Metadata = "{\"firstName\":\"John\",\"lastName\":\"Doe\"}",
            Method = PaymentMethod.BankTransfer,
            WebhookUrl = DefaultWebhookUrl
        };

        // When: We send the payment request to Mollie
        PaymentResponse result = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // Then: Make sure all requested parameters match the response parameter values
        result.ShouldNotBeNull();
        result.Amount.ShouldBe(paymentRequest.Amount);
        result.Description.ShouldBe(paymentRequest.Description);
        result.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
        result.Locale.ShouldBe(paymentRequest.Locale);
        result.WebhookUrl.ShouldBe(paymentRequest.WebhookUrl);
        IsJsonResultEqual(result.Metadata, paymentRequest.Metadata).ShouldBeTrue();
    }

    [Fact]
    public async Task CanUpdatePayment() {
        // Given: We create a payment with only the required parameters
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };
        PaymentResponse result = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // When: We update this payment
        PaymentUpdateRequest paymentUpdateRequest = new PaymentUpdateRequest() {
            Description = "Updated description",
            Metadata = "My metadata"
        };
        PaymentResponse updatedPayment = await _paymentClient.UpdatePaymentAsync(result.Id, paymentUpdateRequest);

        // Then: Make sure the payment is updated
        updatedPayment.Description.ShouldBe(paymentUpdateRequest.Description);
        updatedPayment.Metadata.ShouldBe(paymentUpdateRequest.Metadata);
    }

    [Fact]
    public async Task CanCreatePaymentWithSinglePaymentMethod() {
        // Given: we create a payment request and specify multiple payment methods
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Method = PaymentMethod.CreditCard
        };

        // When: We send the payment request to Mollie
        PaymentResponse result = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // Then: Make sure we get a valid response
        result.ShouldNotBeNull();
        result.Amount.ShouldBe(paymentRequest.Amount);
        result.Description.ShouldBe(paymentRequest.Description);
        result.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
        result.Method.ShouldBe(paymentRequest.Method);
    }

    [Fact]
    public async Task CanCreatePaymentWithMultiplePaymentMethods() {
        // When: we create a payment request and specify multiple payment methods
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Methods = new List<string>() {
                PaymentMethod.Ideal,
                PaymentMethod.CreditCard,
                PaymentMethod.DirectDebit
            }
        };

        // When: We send the payment request to Mollie
        PaymentResponse result = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // Then: Make sure we get a valid response
        result.ShouldNotBeNull();
        result.Amount.ShouldBe(paymentRequest.Amount);
        result.Description.ShouldBe(paymentRequest.Description);
        result.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
        result.Method.ShouldBeNull();
    }

    [Theory]
    [InlineData(typeof(PaymentRequest), PaymentMethod.Bancontact, typeof(BancontactPaymentResponse))]
    [InlineData(typeof(BankTransferPaymentRequest), PaymentMethod.BankTransfer, typeof(BankTransferPaymentResponse))]
    [InlineData(typeof(PayPalPaymentRequest), PaymentMethod.PayPal, typeof(PayPalPaymentResponse))]
    [InlineData(typeof(PaymentRequest), PaymentMethod.Belfius, typeof(BelfiusPaymentResponse))]
    [InlineData(typeof(PaymentRequest), PaymentMethod.Eps, typeof(EpsPaymentResponse))]
    [InlineData(typeof(PaymentRequest), null, typeof(PaymentResponse))]
    public async Task CanCreateSpecificPaymentType(Type paymentType, string paymentMethod, Type expectedResponseType) {
        // When: we create a specific payment type with some bank transfer specific values
        PaymentRequest paymentRequest = (PaymentRequest)Activator.CreateInstance(paymentType)!;
        paymentRequest.Amount = new Amount(Currency.EUR, "100.00");
        paymentRequest.Description = "Description";
        paymentRequest.RedirectUrl = DefaultRedirectUrl;
        paymentRequest.Method = paymentMethod;

        // Set required billing email for Przelewy24
        if (paymentRequest is Przelewy24PaymentRequest request) {
            request.BillingEmail = "example@example.com";
        }

        // When: We send the payment request to Mollie
        PaymentResponse result = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // Then: Make sure all requested parameters match the response parameter values
        result.ShouldNotBeNull();
        result.ShouldBeOfType(expectedResponseType);
        result.Amount.ShouldBe(paymentRequest.Amount);
        result.Description.ShouldBe(paymentRequest.Description);
        result.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
        result.Method.ShouldBe(paymentRequest.Method);
    }

    [Fact]
    public async Task CanCreatePaymentAndRetrieveIt() {
        // When: we create a new payment request
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Locale = Locale.de_DE
        };

        // When: We send the payment request to Mollie and attempt to retrieve it
        PaymentResponse paymentResponse = await _paymentClient.CreatePaymentAsync(paymentRequest);
        PaymentResponse result = await _paymentClient.GetPaymentAsync(paymentResponse.Id);

        // Then
        result.ShouldNotBeNull();
        result.Id.ShouldBe(paymentResponse.Id);
        result.Amount.ShouldBe(paymentRequest.Amount);
        result.Description.ShouldBe(paymentRequest.Description);
        result.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
        result.Method.ShouldBe(paymentRequest.Method);
    }

    [Fact]
    public async Task CanCreateRecurringPaymentAndRetrieveIt() {
        // When: we create a new recurring payment
        MandateResponse? mandate = await GetFirstValidMandate();
        if (mandate != null) {
            CustomerResponse customer = await _customerClient.GetCustomerAsync(mandate.Links.Customer);
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = DefaultRedirectUrl,
                SequenceType = SequenceType.First,
                CustomerId = customer.Id
            };

            // When: We send the payment request to Mollie and attempt to retrieve it
            PaymentResponse paymentResponse = await _paymentClient.CreatePaymentAsync(paymentRequest);
            PaymentResponse result = await _paymentClient.GetPaymentAsync(paymentResponse.Id);

            // Then: Make sure the recurringtype parameter is entered
            result.SequenceType.ShouldBe(SequenceType.First);
        }
    }

    [Fact]
    public async Task CanCreatePaymentWithMetaData() {
        // When: We create a payment with meta data
        string metadata = "this is my metadata";
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Metadata = metadata
        };

        // When: We send the payment request to Mollie
        PaymentResponse result = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // Then: Make sure we get the same json result as metadata
        result.Metadata.ShouldBe(metadata);
    }

    [Fact]
    public async Task CanCreatePaymentWithJsonMetaData() {
        // When: We create a payment with meta data
        string json = "{\"order_id\":\"4.40\"}";
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Metadata = json
        };

        // When: We send the payment request to Mollie
        PaymentResponse result = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // Then: Make sure we get the same json result as metadata
        IsJsonResultEqual(result.Metadata, json).ShouldBeTrue();
    }

    [Fact]
    public async Task CanCreatePaymentWithCustomMetaDataClass() {
        // When: We create a payment with meta data
        CustomMetadataClass metadataRequest = new CustomMetadataClass() {
            OrderId = 1,
            Description = "Custom description"
        };

        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
        };
        paymentRequest.SetMetadata(metadataRequest);

        // When: We send the payment request to Mollie
        PaymentResponse result = await _paymentClient.CreatePaymentAsync(paymentRequest);
        CustomMetadataClass? metadataResponse = result.GetMetadata<CustomMetadataClass>();

        // Then: Make sure we get the same json result as metadata
        metadataResponse.ShouldNotBeNull();
        metadataResponse.OrderId.ShouldBe(metadataRequest.OrderId);
        metadataResponse.Description.ShouldBe(metadataRequest.Description);
    }

    [Fact]
    public async Task CanCreatePaymentWithLines() {
        // Arrange
        var address = new PaymentAddressDetails {
            Title = "Mr",
            GivenName = "John",
            FamilyName = "Doe",
            OrganizationName = "Mollie",
            StreetAndNumber = "Keizersgracht 126",
            Email = "johndoe@mollie.com",
            City = "Amsterdam",
            Country = "NL",
            Phone = "+31600000000",
            Region = "Zuid-Holland",
            PostalCode = "1015CW"
        };
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, 90m),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Lines = new List<PaymentLine>() {
                new() {
                    Type = OrderLineDetailsType.Digital,
                    Description = "Star wars lego",
                    Quantity = 1,
                    QuantityUnit = "pcs",
                    UnitPrice = new Amount(Currency.EUR, 100m),
                    TotalAmount = new Amount(Currency.EUR, 90m),
                    DiscountAmount = new Amount(Currency.EUR, 10m),
                    ProductUrl = "http://www.lego.com/starwars",
                    ImageUrl = "http://www.lego.com/starwars.jpg",
                    Sku = "my-sku",
                    VatAmount = new Amount(Currency.EUR, 15.62m),
                    VatRate = "21.00"
                }
            },
            ShippingAddress = address,
            BillingAddress = address
        };

        // Act
        PaymentResponse result = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // Assert
        result.Lines.ShouldBeEquivalentTo(paymentRequest.Lines);
        result.BillingAddress.ShouldBeEquivalentTo(paymentRequest.BillingAddress);
        result.ShippingAddress.ShouldBeEquivalentTo(paymentRequest.ShippingAddress);
    }

    [Fact]
    public async Task CanCreatePaymentWithMandate() {
        // When: We create a payment with a mandate id
        MandateResponse? validMandate = await GetFirstValidMandate();
        if (validMandate != null) {
            CustomerResponse customer = await _customerClient.GetCustomerAsync(validMandate.Links.Customer);
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = DefaultRedirectUrl,
                SequenceType = SequenceType.Recurring,
                CustomerId = customer.Id,
                MandateId = validMandate.Id
            };

            // When: We send the payment request to Mollie
            PaymentResponse result = await _paymentClient.CreatePaymentAsync(paymentRequest);

            // Then: Make sure we get the mandate id back in the details
            result.MandateId.ShouldBe(validMandate.Id);
            result.Links.Mandate!.Href.ShouldEndWith(validMandate.Id);
            result.Links.Customer!.Href.ShouldEndWith(customer.Id);
        }
    }

    [Fact]
    public async Task CanCreatePaymentWithDecimalAmountAndRetrieveIt() {
        // When: we create a new payment request
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, 100.1235m),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Locale = Locale.de_DE
        };

        // When: We send the payment request to Mollie and attempt to retrieve it
        PaymentResponse paymentResponse = await _paymentClient.CreatePaymentAsync(paymentRequest);
        PaymentResponse result = await _paymentClient.GetPaymentAsync(paymentResponse.Id);

        // Then
        result.ShouldNotBeNull();
        result.Id.ShouldBe(paymentResponse.Id);
        result.Amount.ShouldBe(paymentRequest.Amount);
        result.Description.ShouldBe(paymentRequest.Description);
        result.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
    }

    [Fact]
    public async Task CanCreatePaymentWithImplicitAmountCastAndRetrieveIt() {
        var initialAmount = 100.75m;

        // When: we create a new payment request
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, initialAmount),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Locale = Locale.de_DE
        };

        // When: We send the payment request to Mollie and attempt to retrieve it
        PaymentResponse paymentResponse = await _paymentClient.CreatePaymentAsync(paymentRequest);
        PaymentResponse result = await _paymentClient.GetPaymentAsync(paymentResponse.Id);

        decimal responseAmount = paymentResponse.Amount; // Implicit cast
        decimal resultAmount = result.Amount; // Implicit cast

        // Then
        result.ShouldNotBeNull();
        result.Id.ShouldBe(paymentResponse.Id);
        result.Amount.ShouldBe(paymentRequest.Amount);
        result.Description.ShouldBe(paymentRequest.Description);
        result.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
        resultAmount.ShouldBe(responseAmount);
        resultAmount.ShouldBe(initialAmount);
    }

    [Fact]
    public async Task CanCreatePointOfSalePayment() {
        // Given
        ListResponse<TerminalResponse> terminals = await _terminalClient.GetTerminalListAsync();
        TerminalResponse? terminal = terminals.Items.FirstOrDefault();
        if (terminal != null) {
            string terminalId = terminals.Items.First().Id;
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, 10m),
                Description = "Description",
                Method = PaymentMethod.PointOfSale,
                TerminalId = terminalId
            };

            // When
            PaymentResponse response = await _paymentClient.CreatePaymentAsync(paymentRequest);

            // Then
            response.ShouldNotBeNull();
            response.Amount.ShouldBe(paymentRequest.Amount);
            response.Description.ShouldBe(paymentRequest.Description);
            response.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
            response.ShouldBeOfType<PointOfSalePaymentResponse>();
            PointOfSalePaymentResponse posResponse = (PointOfSalePaymentResponse)response;
            posResponse.Details!.TerminalId.ShouldBe(paymentRequest.TerminalId);
            posResponse.Details.CardNumber.ShouldBeNull();
            posResponse.Details.CardFingerprint.ShouldBeNull();
            posResponse.Details.CardAudience.ShouldBeNull();
            posResponse.Details.CardLabel.ShouldBeNull();
            posResponse.Details.CardCountryCode.ShouldBeNull();
            posResponse.Method.ShouldBe(PaymentMethod.PointOfSale);
        }
    }

    [Fact(Skip = "We can only test this in debug mode, because we have to set the payment status to authorized")]
    public async Task CanCreatePaymentWithManualCaptureMode() {
        // Given
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, 10m),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Method = PaymentMethod.CreditCard,
            CaptureMode = CaptureMode.Manual
        };

        // When
        PaymentResponse paymentResponse = await _paymentClient.CreatePaymentAsync(paymentRequest);
        // Perform payment before API call
        paymentResponse = await _paymentClient.GetPaymentAsync(paymentResponse.Id);
        CaptureResponse captureResponse = await _captureClient.CreateCapture(paymentResponse.Id, new CaptureRequest {
            Amount = new Amount(Currency.EUR, 10m),
            Description = "capture"
        });

        // Then
        captureResponse.ShouldNotBeNull();
        paymentResponse.Status.ShouldBe(PaymentStatus.Authorized);
        paymentRequest.CaptureMode.ShouldBe(CaptureMode.Manual);
        paymentResponse.CaptureBefore.ShouldNotBeNull();
    }

    [Fact]
    public async Task CanCreatePaymentWithCaptureDelay() {
        // Given
        PaymentRequest paymentRequest = new() {
            Amount = new Amount(Currency.EUR, 10m),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Method = PaymentMethod.CreditCard,
            CaptureDelay = "2 days"
        };

        // When
        PaymentResponse paymentResponse = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        paymentResponse.CaptureDelay.ShouldBe(paymentRequest.CaptureDelay);
    }

    private async Task<MandateResponse?> GetFirstValidMandate() {
        ListResponse<CustomerResponse> customers = await _customerClient.GetCustomerListAsync();

        foreach (CustomerResponse customer in customers.Items) {
            ListResponse<MandateResponse> customerMandates = await _mandateClient.GetMandateListAsync(customer.Id);
            MandateResponse? firstValidMandate = customerMandates.Items.FirstOrDefault(x => x.Status == MandateStatus.Valid);
            if (firstValidMandate != null) {
                return firstValidMandate;
            }
        }

        return null;
    }

    public void Dispose()
    {
        _paymentClient.Dispose();
        _customerClient.Dispose();
        _mandateClient.Dispose();
        _terminalClient.Dispose();
        _captureClient.Dispose();
    }
}

public record CustomMetadataClass {
    public required int OrderId { get; init; }
    public required string Description { get; init; }
}
