using System;
using System.Linq;
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
        response.Id.ShouldNotBeNullOrEmpty();
        response.Resource.ShouldBe("sales-invoice");
        response.InvoiceNumber.ShouldStartWith("I-");
        response.ProfileId.ShouldStartWith("pfl_");
        response.Currency.ShouldBe(request.Currency);
        response.Status.ShouldBe(request.Status);
        response.PaymentTerm.ShouldBe(request.PaymentTerm);
        response.Lines.ShouldNotBeNull();
        response.Lines.ShouldHaveSingleItem();
        response.Lines.ShouldContain(l =>
            l.Description == "Test product" &&
            l.Quantity == 1 &&
            l.VatRate == "21" && // TODO: Report this to Mollie, should be "21.00"
            l.UnitPrice == 50m);
        response.Recipient.ShouldNotBeNull();
        response.Recipient.Type.ShouldBeNull(); // TODO: Report to Mollie, should be consumer
        response.Recipient.Email.ShouldBe(request.Recipient.Email);
        response.Recipient.FamilyName.ShouldBe(request.Recipient.FamilyName);
        response.Recipient.GivenName.ShouldBe(request.Recipient.GivenName);
        response.Recipient.StreetAndNumber.ShouldBe(request.Recipient.StreetAndNumber);
        response.Recipient.PostalCode.ShouldBe(request.Recipient.PostalCode);
        response.Recipient.City.ShouldBe(request.Recipient.City);
        response.Recipient.Country.ShouldBe(request.Recipient.Country);
        response.Recipient.Locale.ShouldBeNull(); // TODO: Report to Mollie, should be nl_NL
        response.RecipientIdentifier.ShouldBe(request.RecipientIdentifier);
    }

    public void Dispose() {
        _salesInvoiceClient.Dispose();
    }
}
