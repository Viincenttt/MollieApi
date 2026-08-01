using System;
using System.Threading.Tasks;
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
using Mollie.Api.Models.Customer.Response;
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

    public PaymentTests(
        IPaymentClient paymentClient,
        ICustomerClient customerClient,
        IMandateClient mandateClient,
        ITerminalClient terminalClient,
        ICaptureClient captureClient) {
        _paymentClient = paymentClient;
        _customerClient = customerClient;
        _mandateClient = mandateClient;
        _terminalClient = terminalClient;
        _captureClient = captureClient;
    }

    [Fact]
    public async Task CanRetrievePaymentList() {
        // When: Retrieve payment list with default settings
        var result = await _paymentClient.GetPaymentListAsync();
        var response = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
        response.Items.Select(x => x.CreatedAt).ShouldBeInOrder(Shouldly.SortDirection.Descending);
    }

    [Fact]
    public async Task CanRetrievePaymentListInDescendingOrder()
    {
        // When: Retrieve payment list in ascending order
        var result = await _paymentClient.GetPaymentListAsync(sort: SortDirection.Desc);
        var response = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
        response.Items.Select(x => x.CreatedAt).ShouldBeInOrder(Shouldly.SortDirection.Descending);
    }

    [Fact]
    public async Task CanRetrievePaymentListInAscendingOrder()
    {
        // When: Retrieve payment list in ascending order
        var result = await _paymentClient.GetPaymentListAsync(sort: SortDirection.Asc);
        var response = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
        response.Items.Select(x => x.CreatedAt).ShouldBeInOrder(Shouldly.SortDirection.Ascending);
    }

    [Fact]
    public async Task ListPaymentsNeverReturnsMorePaymentsThenTheNumberOfRequestedPayments() {
        // Given: Number of payments requested is 5
        int numberOfPayments = 5;

        // When: Retrieve 5 payments
        var result = await _paymentClient.GetPaymentListAsync(null, numberOfPayments);
        var response = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
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
        var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var payment = result.Data!;

        // Then: Make sure we get a valid response
        result.Success.ShouldBeTrue();
        payment.ShouldNotBeNull();
        payment.Amount.ShouldBe(paymentRequest.Amount);
        payment.Description.ShouldBe(paymentRequest.Description);
        payment.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
    }

    [Fact]
    public async Task CanCreateDefaultPaymentWithCustomIdempotencyKey() {
        // Given: we create a payment request with only the required parameters
        var paymentRequest = new PaymentRequest {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };
        var idempotencyKey = Guid.NewGuid();

        // When: We send the payment request to Mollie
        using (_paymentClient.WithIdempotencyKey(idempotencyKey.ToString()))
        {
            var firstResult = await _paymentClient.CreatePaymentAsync(paymentRequest);
            var secondResult = await _paymentClient.CreatePaymentAsync(paymentRequest);
            var firstAttempt = firstResult.Data!;
            var secondAttempt = secondResult.Data!;

            // Then: Make sure the responses have the same payment Id
            firstResult.Success.ShouldBeTrue();
            secondResult.Success.ShouldBeTrue();
            firstAttempt.Id.ShouldBe(secondAttempt.Id);
        }
    }

    [Fact]
    public async Task CanCreateDefaultPaymentWithAllFields() {
        // Given: we create a payment request where all parameters have a value
        var paymentRequest = new PaymentRequest {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Locale = Locale.nl_NL,
            Metadata = "{\"firstName\":\"John\",\"lastName\":\"Doe\"}",
            Method = PaymentMethod.BankTransfer,
            WebhookUrl = DefaultWebhookUrl
        };

        // When: We send the payment request to Mollie
        var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var payment = result.Data!;

        // Then: Make sure all requested parameters match the response parameter values
        result.Success.ShouldBeTrue();
        payment.ShouldNotBeNull();
        payment.Amount.ShouldBe(paymentRequest.Amount);
        payment.Description.ShouldBe(paymentRequest.Description);
        payment.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
        payment.Locale.ShouldBe(paymentRequest.Locale);
        payment.WebhookUrl.ShouldBe(paymentRequest.WebhookUrl);
        IsJsonResultEqual(payment.Metadata, paymentRequest.Metadata).ShouldBeTrue();
    }

    [Fact]
    public async Task CanUpdatePayment() {
        // Given: We create a payment with only the required parameters
        var paymentRequest = new PaymentRequest {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };
        var createResult = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var result = createResult.Data!;

        // When: We update this payment
        var paymentUpdateRequest = new PaymentUpdateRequest {
            Description = "Updated description",
            Metadata = "My metadata"
        };
        var updateResult = await _paymentClient.UpdatePaymentAsync(result.Id, paymentUpdateRequest);
        var updatedPayment = updateResult.Data!;

        // Then: Make sure the payment is updated
        createResult.Success.ShouldBeTrue();
        updateResult.Success.ShouldBeTrue();
        updatedPayment.Description.ShouldBe(paymentUpdateRequest.Description);
        updatedPayment.Metadata.ShouldBe(paymentUpdateRequest.Metadata);
    }

    [Fact]
    public async Task CanCreatePaymentWithSinglePaymentMethod() {
        // Given: we create a payment request and specify multiple payment methods
        var paymentRequest = new PaymentRequest {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Method = PaymentMethod.CreditCard
        };

        // When: We send the payment request to Mollie
        var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var payment = result.Data!;

        // Then: Make sure we get a valid response
        result.Success.ShouldBeTrue();
        payment.ShouldNotBeNull();
        payment.Amount.ShouldBe(paymentRequest.Amount);
        payment.Description.ShouldBe(paymentRequest.Description);
        payment.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
        payment.Method.ShouldBe(paymentRequest.Method);
    }

    [Fact]
    public async Task CanCreatePaymentWithMultiplePaymentMethods() {
        // When: we create a payment request and specify multiple payment methods
        var paymentRequest = new PaymentRequest {
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
        var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var payment = result.Data!;

        // Then: Make sure we get a valid response
        result.Success.ShouldBeTrue();
        payment.ShouldNotBeNull();
        payment.Amount.ShouldBe(paymentRequest.Amount);
        payment.Description.ShouldBe(paymentRequest.Description);
        payment.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
        payment.Method.ShouldBeNull();
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
        var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var payment = result.Data!;

        // Then: Make sure all requested parameters match the response parameter values
        result.Success.ShouldBeTrue();
        payment.ShouldNotBeNull();
        payment.ShouldBeOfType(expectedResponseType);
        payment.Amount.ShouldBe(paymentRequest.Amount);
        payment.Description.ShouldBe(paymentRequest.Description);
        payment.RedirectUrl.ShouldBe(paymentRequest.RedirectUrl);
        payment.Method.ShouldBe(paymentRequest.Method);
        payment.Links.ShouldNotBeNull();
    }

    [Fact]
    public async Task CanCreatePaymentAndRetrieveIt() {
        // When: we create a new payment request
        var paymentRequest = new PaymentRequest {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Locale = Locale.de_DE
        };

        // When: We send the payment request to Mollie and attempt to retrieve it
        var createResult = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var paymentResponse = createResult.Data!;
        var getResult = await _paymentClient.GetPaymentAsync(paymentResponse.Id);
        var result = getResult.Data!;

        // Then
        createResult.Success.ShouldBeTrue();
        getResult.Success.ShouldBeTrue();
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
            var customerResult = await _customerClient.GetCustomerAsync(mandate.Links.Customer);
            var customer = customerResult.Data!;
            var paymentRequest = new PaymentRequest {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = DefaultRedirectUrl,
                SequenceType = SequenceType.First,
                CustomerId = customer.Id
            };

            // When: We send the payment request to Mollie and attempt to retrieve it
            var createResult = await _paymentClient.CreatePaymentAsync(paymentRequest);
            var paymentResponse = createResult.Data!;
            var getResult = await _paymentClient.GetPaymentAsync(paymentResponse.Id);
            var result = getResult.Data!;

            // Then: Make sure the recurringtype parameter is entered
            customerResult.Success.ShouldBeTrue();
            createResult.Success.ShouldBeTrue();
            getResult.Success.ShouldBeTrue();
            result.SequenceType.ShouldBe(SequenceType.First);
        }
    }

    [Fact]
    public async Task CanCreatePaymentWithMetaData() {
        // When: We create a payment with meta data
        string metadata = "this is my metadata";
        var paymentRequest = new PaymentRequest {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Metadata = metadata
        };

        // When: We send the payment request to Mollie
        var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var payment = result.Data!;

        // Then: Make sure we get the same json result as metadata
        result.Success.ShouldBeTrue();
        payment.Metadata.ShouldBe(metadata);
    }

    [Fact]
    public async Task CanCreatePaymentWithJsonMetaData() {
        // When: We create a payment with meta data
        string json = "{\"order_id\":\"4.40\"}";
        var paymentRequest = new PaymentRequest {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Metadata = json
        };

        // When: We send the payment request to Mollie
        var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var payment = result.Data!;

        // Then: Make sure we get the same json result as metadata
        result.Success.ShouldBeTrue();
        IsJsonResultEqual(payment.Metadata, json).ShouldBeTrue();
    }

    [Fact]
    public async Task CanCreatePaymentWithCustomMetaDataClass() {
        // When: We create a payment with meta data
        CustomMetadataClass metadataRequest = new CustomMetadataClass() {
            OrderId = 1,
            Description = "Custom description"
        };

        var paymentRequest = new PaymentRequest {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
        };
        paymentRequest.SetMetadata(metadataRequest);

        // When: We send the payment request to Mollie
        var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var payment = result.Data!;
        CustomMetadataClass? metadataResponse = payment.GetMetadata<CustomMetadataClass>();

        // Then: Make sure we get the same json result as metadata
        result.Success.ShouldBeTrue();
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
        var paymentRequest = new PaymentRequest {
            Amount = new Amount(Currency.EUR, 90m),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Lines = new List<PaymentLine> {
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
        var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var payment = result.Data!;

        // Assert
        result.Success.ShouldBeTrue();
        payment.Lines.ShouldBeEquivalentTo(paymentRequest.Lines);
        payment.BillingAddress.ShouldBeEquivalentTo(paymentRequest.BillingAddress);
        payment.ShippingAddress.ShouldBeEquivalentTo(paymentRequest.ShippingAddress);
    }

    [Fact]
    public async Task CanCreatePaymentWithMandate() {
        // When: We create a payment with a mandate id
        MandateResponse? validMandate = await GetFirstValidMandate();
        if (validMandate != null) {
            var customerResult = await _customerClient.GetCustomerAsync(validMandate.Links.Customer);
            var customer = customerResult.Data!;
            var paymentRequest = new PaymentRequest {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = DefaultRedirectUrl,
                SequenceType = SequenceType.Recurring,
                CustomerId = customer.Id,
                MandateId = validMandate.Id
            };

            // When: We send the payment request to Mollie
            var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
            var payment = result.Data!;

            // Then: Make sure we get the mandate id back in the details
            customerResult.Success.ShouldBeTrue();
            result.Success.ShouldBeTrue();
            payment.MandateId.ShouldBe(validMandate.Id);
            payment.Links.Mandate!.Href.ShouldEndWith(validMandate.Id);
            payment.Links.Customer!.Href.ShouldEndWith(customer.Id);
        }
    }

    [Fact]
    public async Task CanCreatePaymentWithDecimalAmountAndRetrieveIt() {
        // When: we create a new payment request
        var paymentRequest = new PaymentRequest {
            Amount = new Amount(Currency.EUR, 100.1235m),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Locale = Locale.de_DE
        };

        // When: We send the payment request to Mollie and attempt to retrieve it
        var createResult = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var paymentResponse = createResult.Data!;
        var getResult = await _paymentClient.GetPaymentAsync(paymentResponse.Id);
        var result = getResult.Data!;

        // Then
        createResult.Success.ShouldBeTrue();
        getResult.Success.ShouldBeTrue();
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
        var paymentRequest = new PaymentRequest {
            Amount = new Amount(Currency.EUR, initialAmount),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Locale = Locale.de_DE
        };

        // When: We send the payment request to Mollie and attempt to retrieve it
        var createResult = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var paymentResponse = createResult.Data!;
        var getResult = await _paymentClient.GetPaymentAsync(paymentResponse.Id);
        var result = getResult.Data!;

        decimal responseAmount = paymentResponse.Amount; // Implicit cast
        decimal resultAmount = result.Amount; // Implicit cast

        // Then
        createResult.Success.ShouldBeTrue();
        getResult.Success.ShouldBeTrue();
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
        var terminalListResult = await _terminalClient.GetTerminalListAsync();
        var terminals = terminalListResult.Data!;
        TerminalResponse? terminal = terminals.Items.FirstOrDefault();
        if (terminal != null) {
            string terminalId = terminals.Items.First().Id;
            PointOfSalePaymentRequest paymentRequest = new() {
                Amount = new Amount(Currency.EUR, 10m),
                Description = "Description",
                Method = PaymentMethod.PointOfSale,
                TerminalId = terminalId
            };

            // When
            var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
            var response = result.Data!;

            // Then
            terminalListResult.Success.ShouldBeTrue();
            result.Success.ShouldBeTrue();
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
        var paymentRequest = new PaymentRequest {
            Amount = new Amount(Currency.EUR, 10m),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Method = PaymentMethod.CreditCard,
            CaptureMode = CaptureMode.Manual
        };

        // When
        var createResult = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var paymentResponse = createResult.Data!;
        // Perform payment before API call
        var getResult = await _paymentClient.GetPaymentAsync(paymentResponse.Id);
        paymentResponse = getResult.Data!;
        var captureResult = await _captureClient.CreateCapture(paymentResponse.Id, new CaptureRequest {
            Amount = new Amount(Currency.EUR, 10m),
            Description = "capture"
        });
        var captureResponse = captureResult.Data!;

        // Then
        createResult.Success.ShouldBeTrue();
        getResult.Success.ShouldBeTrue();
        captureResult.Success.ShouldBeTrue();
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
        var result = await _paymentClient.CreatePaymentAsync(paymentRequest);
        var paymentResponse = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        paymentResponse.CaptureDelay.ShouldBe(paymentRequest.CaptureDelay);
    }

    private async Task<MandateResponse?> GetFirstValidMandate() {
        var customerListResult = await _customerClient.GetCustomerListAsync();
        var customers = customerListResult.Data!;

        foreach (CustomerResponse customer in customers.Items) {
            var mandateListResult = await _mandateClient.GetMandateListAsync(customer.Id);
            var customerMandates = mandateListResult.Data!;
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
