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
using FluentAssertions;
using Mollie.Api.Models.Capture.Response;
using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Mandate.Response;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Payment.Request.PaymentSpecificParameters;
using Mollie.Api.Models.Payment.Response.PaymentSpecificParameters;
using Mollie.Api.Models.Terminal.Response;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class PaymentTests : BaseMollieApiTestClass, IDisposable {
    private readonly IPaymentClient _paymentClient;
    private readonly ICustomerClient _customerClient;
    private readonly IMandateClient _mandateClient;
    private readonly ITerminalClient _terminalClient;
    private readonly ICaptureClient _captureClient;

    public PaymentTests() {
        _paymentClient = new PaymentClient(ApiKey);
        _customerClient = new CustomerClient(ApiKey);
        _mandateClient = new MandateClient(ApiKey);
        _terminalClient = new TerminalClient(ApiKey);
        _captureClient = new CaptureClient(ApiKey);
    }

    [DefaultRetryFact]
    public async Task CanRetrievePaymentList() {
        // When: Retrieve payment list with default settings
        ListResponse<PaymentResponse> response = await _paymentClient.GetPaymentListAsync();

        // Then
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().BeInDescendingOrder(x => x.CreatedAt);
    }

    [DefaultRetryFact]
    public async Task CanRetrievePaymentListInDescendingOrder()
    {
        // When: Retrieve payment list in ascending order
        ListResponse<PaymentResponse> response = await _paymentClient.GetPaymentListAsync(sort: SortDirection.Desc);

        // Then
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().BeInDescendingOrder(x => x.CreatedAt);
    }

    [DefaultRetryFact]
    public async Task CanRetrievePaymentListInAscendingOrder()
    {
        // When: Retrieve payment list in ascending order
        ListResponse<PaymentResponse> response = await _paymentClient.GetPaymentListAsync(sort: SortDirection.Asc);

        // Then
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
        response.Items.Should().BeInAscendingOrder(x => x.CreatedAt);
    }

    [DefaultRetryFact]
    public async Task ListPaymentsNeverReturnsMorePaymentsThenTheNumberOfRequestedPayments() {
        // Given: Number of payments requested is 5
        int numberOfPayments = 5;

        // When: Retrieve 5 payments
        ListResponse<PaymentResponse> response = await _paymentClient.GetPaymentListAsync(null, numberOfPayments);

        // Then
        response.Items.Count.Should().BeLessOrEqualTo(numberOfPayments);
    }

    [DefaultRetryFact]
    public async Task CanCreateDefaultPaymentWithOnlyRequiredFields() {
        // Given: we create a payment request with only the required parameters
        PaymentRequest paymentRequest = new KbcPaymentRequest {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };

        // When: We send the payment request to Mollie
        PaymentResponse result = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // Then: Make sure we get a valid response
        result.Should().NotBeNull();
        result.Amount.Should().Be(paymentRequest.Amount);
        result.Description.Should().Be(paymentRequest.Description);
        result.RedirectUrl.Should().Be(paymentRequest.RedirectUrl);
    }

    [DefaultRetryFact]
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
            firstAttempt.Id.Should().Be(secondAttempt.Id);
        }
    }

    [DefaultRetryFact]
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
        result.Should().NotBeNull();
        result.Amount.Should().Be(paymentRequest.Amount);
        result.Description.Should().Be(paymentRequest.Description);
        result.RedirectUrl.Should().Be(paymentRequest.RedirectUrl);
        result.Locale.Should().Be(paymentRequest.Locale);
        result.WebhookUrl.Should().Be(paymentRequest.WebhookUrl);
        IsJsonResultEqual(result.Metadata, paymentRequest.Metadata).Should().BeTrue();
    }

    [DefaultRetryFact]
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
        updatedPayment.Description.Should().Be(paymentUpdateRequest.Description);
        updatedPayment.Metadata.Should().Be(paymentUpdateRequest.Metadata);
    }

    [DefaultRetryFact]
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
        result.Should().NotBeNull();
        result.Amount.Should().Be(paymentRequest.Amount);
        result.Description.Should().Be(paymentRequest.Description);
        result.RedirectUrl.Should().Be(paymentRequest.RedirectUrl);
        result.Method.Should().Be(paymentRequest.Method);
    }

    [DefaultRetryFact]
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
        result.Should().NotBeNull();
        result.Amount.Should().Be(paymentRequest.Amount);
        result.Description.Should().Be(paymentRequest.Description);
        result.RedirectUrl.Should().Be(paymentRequest.RedirectUrl);
        result.Method.Should().BeNull();
    }

    [DefaultRetryTheory]
    [InlineData(typeof(PaymentRequest), PaymentMethod.Bancontact, typeof(BancontactPaymentResponse))]
    [InlineData(typeof(BankTransferPaymentRequest), PaymentMethod.BankTransfer, typeof(BankTransferPaymentResponse))]
    [InlineData(typeof(PayPalPaymentRequest), PaymentMethod.PayPal, typeof(PayPalPaymentResponse))]
    [InlineData(typeof(PaymentRequest), PaymentMethod.Belfius, typeof(BelfiusPaymentResponse))]
    [InlineData(typeof(KbcPaymentRequest), PaymentMethod.Kbc, typeof(KbcPaymentResponse))]
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
        result.Should().NotBeNull();
        result.Should().BeOfType(expectedResponseType);
        result.Amount.Should().Be(paymentRequest.Amount);
        result.Description.Should().Be(paymentRequest.Description);
        result.RedirectUrl.Should().Be(paymentRequest.RedirectUrl);
        result.Method.Should().Be(paymentRequest.Method);
    }

    [Fact]
    public async Task CanCreateVoucherPaymentAndRetrieveIt() {
        // When: we create a voucher payment
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Method = PaymentMethod.Voucher,
            Lines = [
                new() {
                    Type = OrderLineDetailsType.Digital,
                    Categories = [
                        "gift"
                    ],
                    Description = "Star wars lego",
                    Quantity = 1,
                    QuantityUnit = "pcs",
                    UnitPrice = new Amount(Currency.EUR, 100m),
                    TotalAmount = new Amount(Currency.EUR, 100m),
                    ProductUrl = "http://www.lego.com/starwars",
                    ImageUrl = "http://www.lego.com/starwars.jpg",
                    Sku = "my-sku",
                    VatAmount = new Amount(Currency.EUR, 17.36m),
                    VatRate = "21.00"
                }
            ]
        };

        // When: We send the payment request to Mollie and attempt to retrieve it
        PaymentResponse paymentResponse = await _paymentClient.CreatePaymentAsync(paymentRequest);
        PaymentResponse result = await _paymentClient.GetPaymentAsync(paymentResponse.Id);

        // Then
        result.Should().NotBeNull();
        result.Id.Should().Be(paymentResponse.Id);
        result.Amount.Should().Be(paymentRequest.Amount);
        result.Description.Should().Be(paymentRequest.Description);
        result.RedirectUrl.Should().Be(paymentRequest.RedirectUrl);
        result.Method.Should().Be(paymentRequest.Method);
    }

    [DefaultRetryFact]
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
        result.Should().NotBeNull();
        result.Id.Should().Be(paymentResponse.Id);
        result.Amount.Should().Be(paymentRequest.Amount);
        result.Description.Should().Be(paymentRequest.Description);
        result.RedirectUrl.Should().Be(paymentRequest.RedirectUrl);
        result.Method.Should().Be(paymentRequest.Method);
    }

    [DefaultRetryFact]
    public async Task CanCreateRecurringPaymentAndRetrieveIt() {
        // When: we create a new recurring payment
        MandateResponse mandate = await GetFirstValidMandate();
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
            result.SequenceType.Should().Be(SequenceType.First);
        }
    }

    [DefaultRetryFact]
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
        result.Metadata.Should().Be(metadata);
    }

    [DefaultRetryFact]
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
        IsJsonResultEqual(result.Metadata, json).Should().BeTrue();
    }

    [DefaultRetryFact]
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
        CustomMetadataClass metadataResponse = result.GetMetadata<CustomMetadataClass>();

        // Then: Make sure we get the same json result as metadata
        metadataResponse.Should().NotBeNull();
        metadataResponse.OrderId.Should().Be(metadataRequest.OrderId);
        metadataResponse.Description.Should().Be(metadataRequest.Description);
    }

    [DefaultRetryFact]
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
        result.Lines.Should().BeEquivalentTo(paymentRequest.Lines);
        result.BillingAddress.Should().BeEquivalentTo(paymentRequest.BillingAddress);
        result.ShippingAddress.Should().BeEquivalentTo(paymentRequest.ShippingAddress);
    }

    [DefaultRetryFact]
    public async Task CanCreatePaymentWithMandate() {
        // When: We create a payment with a mandate id
        MandateResponse validMandate = await GetFirstValidMandate();
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
            result.MandateId.Should().Be(validMandate.Id);
            result.Links.Mandate!.Href.Should().EndWith(validMandate.Id);
            result.Links.Customer!.Href.Should().EndWith(customer.Id);
        }
    }

    [DefaultRetryFact]
    public async Task PaymentWithDifferentHttpInstance() {
        // When: We create a PaymentClient with our own HttpClient instance
        HttpClient myHttpClientInstance = new HttpClient();
        PaymentClient paymentClient = new PaymentClient(ApiKey, myHttpClientInstance);
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl
        };

        // When: I create a new payment
        PaymentResponse result = await paymentClient.CreatePaymentAsync(paymentRequest);

        // Then: It should still work in the same way
        result.Should().NotBeNull();
        result.Amount.Should().Be(paymentRequest.Amount);
        result.Description.Should().Be(paymentRequest.Description);
        result.RedirectUrl.Should().Be(paymentRequest.RedirectUrl);
    }

    [DefaultRetryFact]
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
        result.Should().NotBeNull();
        result.Id.Should().Be(paymentResponse.Id);
        result.Amount.Should().Be(paymentRequest.Amount);
        result.Description.Should().Be(paymentRequest.Description);
        result.RedirectUrl.Should().Be(paymentRequest.RedirectUrl);
    }

    [DefaultRetryFact]
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
        result.Should().NotBeNull();
        result.Id.Should().Be(paymentResponse.Id);
        result.Amount.Should().Be(paymentRequest.Amount);
        result.Description.Should().Be(paymentRequest.Description);
        result.RedirectUrl.Should().Be(paymentRequest.RedirectUrl);
        resultAmount.Should().Be(responseAmount);
        resultAmount.Should().Be(initialAmount);
    }

    [DefaultRetryFact]
    public async Task CanCreatePointOfSalePayment() {
        // Given
        ListResponse<TerminalResponse> terminals = await _terminalClient.GetTerminalListAsync();
        TerminalResponse terminal = terminals.Items.FirstOrDefault();
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
            response.Should().NotBeNull();
            response.Amount.Should().Be(paymentRequest.Amount);
            response.Description.Should().Be(paymentRequest.Description);
            response.RedirectUrl.Should().Be(paymentRequest.RedirectUrl);
            response.Should().BeOfType<PointOfSalePaymentResponse>();
            PointOfSalePaymentResponse posResponse = (PointOfSalePaymentResponse)response;
            posResponse.Details.TerminalId.Should().Be(paymentRequest.TerminalId);
            posResponse.Details.CardNumber.Should().BeNull();
            posResponse.Details.CardFingerprint.Should().BeNull();
            posResponse.Details.CardAudience.Should().BeNull();
            posResponse.Details.CardLabel.Should().BeNull();
            posResponse.Details.CardCountryCode.Should().BeNull();
            posResponse.Method.Should().Be(PaymentMethod.PointOfSale);
        }
    }

    [DefaultRetryFact(Skip = "We can only test this in debug mode, because we have to set the payment status to authorized")]
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
        captureResponse.Should().NotBeNull();
        paymentResponse.Status.Should().Be(PaymentStatus.Authorized);
        paymentRequest.CaptureMode.Should().Be(CaptureMode.Manual);
        paymentResponse.CaptureBefore.Should().NotBeNull();
    }

    [DefaultRetryFact]
    public async Task CanCreatePaymentWithCaptureDelay() {
        // Given
        PaymentRequest paymentRequest = new PaymentRequest() {
            Amount = new Amount(Currency.EUR, 10m),
            Description = "Description",
            RedirectUrl = DefaultRedirectUrl,
            Method = PaymentMethod.CreditCard,
            CaptureDelay = "2 days"
        };

        // When
        PaymentResponse paymentResponse = await _paymentClient.CreatePaymentAsync(paymentRequest);

        // Then
        paymentResponse.CaptureDelay.Should().Be(paymentRequest.CaptureDelay);
    }

    private async Task<MandateResponse> GetFirstValidMandate() {
        ListResponse<CustomerResponse> customers = await _customerClient.GetCustomerListAsync();

        foreach (CustomerResponse customer in customers.Items) {
            ListResponse<MandateResponse> customerMandates = await _mandateClient.GetMandateListAsync(customer.Id);
            MandateResponse firstValidMandate = customerMandates.Items.FirstOrDefault(x => x.Status == MandateStatus.Valid);
            if (firstValidMandate != null) {
                return firstValidMandate;
            }
        }

        return null;
    }

    public void Dispose()
    {
        _paymentClient?.Dispose();
        _customerClient?.Dispose();
        _mandateClient?.Dispose();
        _terminalClient?.Dispose();
        _captureClient?.Dispose();
    }
}

public class CustomMetadataClass {
    public int OrderId { get; set; }
    public string Description { get; set; }
}
