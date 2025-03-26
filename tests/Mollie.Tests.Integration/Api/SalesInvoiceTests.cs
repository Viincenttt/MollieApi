using System;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.SalesInvoice;
using Mollie.Api.Models.SalesInvoice.Request;
using Mollie.Tests.Integration.Framework;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class SalesInvoiceTests : BaseMollieApiTestClass, IDisposable {
    private readonly ISalesInvoiceClient _salesInvoiceClient;

    public SalesInvoiceTests(ISalesInvoiceClient salesInvoiceClient) {
        _salesInvoiceClient = salesInvoiceClient;
    }


    [Fact]
    public async Task CanCreateSalesInvoice() {
        // Given
        var request = new SalesInvoiceRequest {
            Currency = Currency.EUR,
            Status = "issued",
            PaymentTerm = "7 days",
            Lines = new[] {
                new SalesInvoiceLine() {
                    Description = "Test product",
                    Quantity = 1,
                    VatRate = "21.00",
                    UnitPrice = new Amount(Currency.EUR, 50m)
                }
            },
            RecipientIdentifier = Guid.NewGuid().ToString(),
            Recipient = new Recipient {
                Type = "consumer",
                Email = "example@example.com",
                FamilyName = "Smit",
                GivenName = "Jan",
                StreetAndNumber = "Keizersgracht 313",
                PostalCode = "1234AB",
                City = "Amsterdam",
                Country = "NL",
                Locale = Locale.nl_NL
            }
        };

        // When
        var response = await _salesInvoiceClient.CreateSalesInvoiceAsync(request);

        // Then
        response.ShouldNotBeNull();
    }

    public void Dispose() {
        _salesInvoiceClient.Dispose();
    }
}
