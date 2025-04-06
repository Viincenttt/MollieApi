using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.SalesInvoice;
using Mollie.Api.Models.SalesInvoice.Request;
using Mollie.Api.Models.SalesInvoice.Response;
using Mollie.Api.Models.Url;
using Mollie.Tests.Integration.Framework;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Integration.Api;

[Trait("TestCategory", "LocalIntegrationTests")]
public class SalesInvoiceTests : BaseMollieApiTestClass, IDisposable {
    private readonly ISalesInvoiceClient _salesInvoiceClient;

    public SalesInvoiceTests(ISalesInvoiceClient salesInvoiceClient) {
        _salesInvoiceClient = salesInvoiceClient;
    }

    [Fact]
    public async Task CreateSalesInvoiceAsync_WithRequiredFields_SalesInvoiceIsCreated() {
        // Given: We create a new sales invoice
        var request = CreateSalesInvoiceRequest();

        // When
        var response = await _salesInvoiceClient.CreateSalesInvoiceAsync(request);

        // Then
        AssertSalesInvoice(request, response);
    }

    [Fact]
    public async Task GetSalesInvoiceListAsync_NoParameters_SalesInvoiceListIsRetrieved() {
        // When: Retrieve sales invoice list with default settings
        ListResponse<SalesInvoiceResponse> response = await _salesInvoiceClient.GetSalesInvoiceListAsync();

        // Then
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
        response.Links.ShouldNotBeNull();
        response.Links.Self.Href.ShouldEndWith("sales-invoices");
    }

    [Fact]
    public async Task GetSalesInvoiceListAsync_WithObjectUrlLink_SalesInvoiceListIsRetrieved() {
        // When: Retrieve sales invoice list with object URL link
        var urlObjectLink = new UrlObjectLink<ListResponse<SalesInvoiceResponse>> {
            Href = "https://api.mollie.com/v2/sales-invoices",
            Type = "application/hal+json"
        };
        ListResponse<SalesInvoiceResponse> response = await _salesInvoiceClient.GetSalesInvoiceListAsync(urlObjectLink);

        // Then
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
        response.Links.ShouldNotBeNull();
        response.Links.Self.Href.ShouldEndWith("sales-invoices");
    }

    [Fact]
    public async Task GetSalesInvoiceListAsync_WithMaximumNumberOfItems_MaximumNumberOfSalesInvoicesIsReturned() {
        // Given: Number of sales invoices requested is 5
        int numberOfSalesInvoices = 5;

        // When: Retrieve 5 sales invoices
        ListResponse<SalesInvoiceResponse> response = await _salesInvoiceClient.GetSalesInvoiceListAsync(null, numberOfSalesInvoices);

        // Then
        response.Items.Count.ShouldBeLessThanOrEqualTo(numberOfSalesInvoices);
    }

    [Fact]
    public async Task GetSalesInvoiceAsync_SalesInvoiceCanBeRetrieved() {
        // Given: We create a new sales invoice
        var salesInvoiceRequest = CreateSalesInvoiceRequest();
        var createdSalesInvoice = await _salesInvoiceClient.CreateSalesInvoiceAsync(salesInvoiceRequest);

        // When: We retrieve the sales invoice
        var retrievedSalesInvoice = await _salesInvoiceClient.GetSalesInvoiceAsync(createdSalesInvoice.Id);

        // Then: The retrieved sales invoice should match the created one
        AssertSalesInvoice(salesInvoiceRequest, retrievedSalesInvoice);
    }

    [Fact]
    public async Task GetSalesInvoiceAsync_WithObjectUrlLink_SalesInvoiceCanBeRetrieved() {
        // Given: We create a new sales invoice
        var salesInvoiceRequest = CreateSalesInvoiceRequest();
        var createdSalesInvoice = await _salesInvoiceClient.CreateSalesInvoiceAsync(salesInvoiceRequest);

        // When: We retrieve the sales invoice
        var retrievedSalesInvoice = await _salesInvoiceClient.GetSalesInvoiceAsync(createdSalesInvoice.Links.Self);

        // Then: The retrieved sales invoice should match the created one
        AssertSalesInvoice(salesInvoiceRequest, retrievedSalesInvoice);
    }

    [Fact]
    public async Task UpdateSalesInvoiceAsync_UpdatesSalesInvoice() {
        // Given: We create a new sales invoice
        var salesInvoiceRequest = CreateSalesInvoiceRequest();
        var createdSalesInvoice = await _salesInvoiceClient.CreateSalesInvoiceAsync(salesInvoiceRequest);

        // When: We update the sales invoice
        var updatedSalesInvoiceRequest = new SalesInvoiceUpdateRequest {
            Memo = "Updated memo"
        };
        var updatedSalesInvoice = await _salesInvoiceClient.UpdateSalesInvoiceAsync(createdSalesInvoice.Id, updatedSalesInvoiceRequest);

        // Then: The updated sales invoice should match the updated request
        updatedSalesInvoice.ShouldNotBeNull();
        updatedSalesInvoice.Memo.ShouldBe(updatedSalesInvoiceRequest.Memo);
        updatedSalesInvoice.Lines!.Count().ShouldBe(1);
    }

    [Fact]
    public async Task DeleteSalesInvoiceAsync_DeletesSalesInvoice() {
        // If: We retrieve a list of sales invoices
        ListResponse<SalesInvoiceResponse> response = await _salesInvoiceClient.GetSalesInvoiceListAsync();

        // When: We delete one of the sales invoices in the list
        var salesInvoiceToDelete = response.Items.FirstOrDefault(x => x.Status == SalesInvoiceStatus.Draft);
        if (salesInvoiceToDelete != null) {
            await _salesInvoiceClient.DeleteSalesInvoiceAsync(salesInvoiceToDelete.Id);

            // Then: Make sure the sales invoice is deleted
            MollieApiException apiException = await Assert.ThrowsAsync<MollieApiException>(() =>
                _salesInvoiceClient.GetSalesInvoiceAsync(salesInvoiceToDelete.Id));
            apiException.Details.Status.ShouldBe((int)HttpStatusCode.NotFound);
        }
    }

    private SalesInvoiceRequest CreateSalesInvoiceRequest() {
        return new SalesInvoiceRequest {
            Currency = Currency.EUR,
            Status = SalesInvoiceStatus.Draft,
            PaymentTerm = PaymentTerm.Days30,
            VatMode = VatMode.Exclusive,
            VatScheme = VatScheme.Standard,
            Lines = new[] {
                new SalesInvoiceLine {
                    Description = "Lego Batman",
                    Quantity = 1,
                    VatRate = "21.00",
                    UnitPrice = new Amount(Currency.EUR, 50m)
                }
            },
            RecipientIdentifier = Guid.NewGuid().ToString(),
            Recipient = new Recipient {
                Type = RecipientType.Consumer,
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
    }

    private void AssertSalesInvoice(SalesInvoiceRequest request, SalesInvoiceResponse response) {
        response.ShouldNotBeNull();
        response.Id.ShouldNotBeNullOrEmpty();
        response.Resource.ShouldBe("sales-invoice");
        response.ProfileId.ShouldStartWith("pfl_");
        response.Currency.ShouldBe(request.Currency);
        response.Status.ShouldBe(request.Status);
        response.PaymentTerm.ShouldBe(request.PaymentTerm);
        response.Lines.ShouldNotBeNull();
        response.Lines.ShouldHaveSingleItem();
        //response.Lines.Single().ShouldBeEquivalentTo(response.Lines.Single());
        response.Lines.ShouldContain(l =>
            l.Description == "Lego Batman" &&
            l.Quantity == 1 &&
            l.VatRate == "21" && // TODO: Report this to Mollie, should be "21.00"
            l.UnitPrice == 50m);
        response.Recipient.ShouldNotBeNull();
        response.Recipient.Email.ShouldBe(request.Recipient.Email);
        response.Recipient.FamilyName.ShouldBe(request.Recipient.FamilyName);
        response.Recipient.GivenName.ShouldBe(request.Recipient.GivenName);
        response.Recipient.StreetAndNumber.ShouldBe(request.Recipient.StreetAndNumber);
        response.Recipient.PostalCode.ShouldBe(request.Recipient.PostalCode);
        response.Recipient.City.ShouldBe(request.Recipient.City);
        response.Recipient.Country.ShouldBe(request.Recipient.Country);
        response.RecipientIdentifier.ShouldBe(request.RecipientIdentifier);
        response.Recipient.Type.ShouldBeNull(); // TODO: Report to Mollie, should be consumer
        response.Recipient.Locale.ShouldBeNull(); // TODO: Report to Mollie, should be nl_NL
    }

    public void Dispose() {
        _salesInvoiceClient.Dispose();
    }
}
