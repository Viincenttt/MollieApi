using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.SalesInvoice;
using Mollie.Api.Models.SalesInvoice.Request;
using Shouldly;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class SalesInvoiceClientTests : BaseClientTests {
    [Fact]
    public async Task CreateSalesInvoiceAsync_ShouldReturnSalesInvoiceResponse() {
        // Given: We create a new sales invoice
        var request = new SalesInvoiceRequest {
            Currency = Currency.EUR,
            Status = SalesInvoiceStatus.Draft,
            PaymentTerm = PaymentTerm.Days30,
            RecipientIdentifier = "123532354",
            Recipient = new Recipient {
                Type = RecipientType.Consumer,
                Email = "given.family@mollie.com",
                StreetAndNumber = "Street 1",
                PostalCode = "1000 AA",
                City = "Amsterdam",
                Country = "NL",
                Locale = "nl_NL",
                GivenName = "Given",
                FamilyName = "Family"
            },
            Lines = [
                new SalesInvoiceLine {
                    Description = "LEGO 4440 Forest Police Station",
                    Quantity = 1,
                    VatRate = "21.00",
                    UnitPrice = new Amount("89.00", "EUR")
                }
            ]
        };
        string expectedUrl = $"{BaseMollieClient.DefaultBaseApiEndPoint}sales-invoices";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Post, expectedUrl, DefaultSalesInvoiceClientResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var salesInvoiceClient = new SalesInvoiceClient("api-key", httpClient);

        // When: We create the new invoice
        var result = await salesInvoiceClient.CreateSalesInvoiceAsync(request);

        // Then: We should get a valid response
        mockHttp.VerifyNoOutstandingExpectation();
        result.Id.ShouldBe("invoice_4Y0eZitmBnQ6IDoMqZQKh");
        result.Status.ShouldBe(SalesInvoiceStatus.Draft);
        result.Currency.ShouldBe(Currency.EUR);
        result.Lines.ShouldNotBeEmpty();
        var orderLine = result.Lines.Single();
        orderLine.Description.ShouldBe("LEGO 4440 Forest Police Station");
        orderLine.Quantity.ShouldBe(1);
        orderLine.VatRate.ShouldBe("21.00");
        orderLine.UnitPrice.Value.ShouldBe("89.00");
        orderLine.UnitPrice.Currency.ShouldBe(Currency.EUR);
        orderLine.Discount.ShouldBeNull();
        result.AmountDue.Value.ShouldBe("107.69");
        result.AmountDue.Currency.ShouldBe(Currency.EUR);
        result.DiscountedSubtotalAmount.Value.ShouldBe("89.00");
        result.DiscountedSubtotalAmount.Currency.ShouldBe(Currency.EUR);
    }

    [Fact]
    public async Task GetSalesInvoiceAsync_ShouldReturnSalesInvoiceResponse() {
        // Given: A sales invoice ID
        const string salesInvoiceId = "invoice_4Y0eZitmBnQ6IDoMqZQKh";
        string expectedUrl = $"{BaseMollieClient.DefaultBaseApiEndPoint}sales-invoices/{salesInvoiceId}";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, DefaultSalesInvoiceClientResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var salesInvoiceClient = new SalesInvoiceClient("api-key", httpClient);

        // When: We retrieve the sales invoice
        var result = await salesInvoiceClient.GetSalesInvoiceAsync(salesInvoiceId);

        // Then: The response should match the expected data
        mockHttp.VerifyNoOutstandingExpectation();
        result.Id.ShouldBe(salesInvoiceId);
        result.Status.ShouldBe(SalesInvoiceStatus.Draft);
        result.Currency.ShouldBe(Currency.EUR);
        result.Lines.ShouldNotBeEmpty();
        var orderLine = result.Lines.Single();
        orderLine.Description.ShouldBe("LEGO 4440 Forest Police Station");
        orderLine.Quantity.ShouldBe(1);
        orderLine.VatRate.ShouldBe("21.00");
        orderLine.UnitPrice.Value.ShouldBe("89.00");
        orderLine.UnitPrice.Currency.ShouldBe(Currency.EUR);
        orderLine.Discount.ShouldBeNull();
        result.AmountDue.Value.ShouldBe("107.69");
        result.AmountDue.Currency.ShouldBe(Currency.EUR);
        result.DiscountedSubtotalAmount.Value.ShouldBe("89.00");
        result.DiscountedSubtotalAmount.Currency.ShouldBe(Currency.EUR);
    }

    [Fact]
    public async Task UpdateSalesInvoiceAsync_ShouldReturnUpdatedSalesInvoiceResponse() {
        // Given: A sales invoice ID and update request
        const string salesInvoiceId = "invoice_4Y0eZitmBnQ6IDoMqZQKh";
        var updateRequest = new SalesInvoiceUpdateRequest {
            Memo = "Updated memo"
        };
        string expectedUrl = $"{BaseMollieClient.DefaultBaseApiEndPoint}sales-invoices/{salesInvoiceId}";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Patch, expectedUrl, DefaultSalesInvoiceClientResponse);
        HttpClient httpClient = mockHttp.ToHttpClient();
        var salesInvoiceClient = new SalesInvoiceClient("api-key", httpClient);

        // When: We update the sales invoice
        await salesInvoiceClient.UpdateSalesInvoiceAsync(salesInvoiceId, updateRequest);

        // Then: The update sales invoice endpoint should be called
        mockHttp.VerifyNoOutstandingExpectation();
    }

    [Fact]
    public async Task DeleteSalesInvoiceAsync_ShouldNotThrowException() {
        // Given: A sales invoice ID
        const string salesInvoiceId = "invoice_4Y0eZitmBnQ6IDoMqZQKh";
        string expectedUrl = $"{BaseMollieClient.DefaultBaseApiEndPoint}sales-invoices/{salesInvoiceId}";
        var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Delete, expectedUrl, "{}");
        HttpClient httpClient = mockHttp.ToHttpClient();
        var salesInvoiceClient = new SalesInvoiceClient("api-key", httpClient);

        // When: We delete the sales invoice
        await salesInvoiceClient.DeleteSalesInvoiceAsync(salesInvoiceId);

        // Then: No exception should be thrown
        mockHttp.VerifyNoOutstandingExpectation();
    }

    private const string DefaultSalesInvoiceClientResponse = @"{
  ""resource"": ""sales-invoice"",
  ""id"": ""invoice_4Y0eZitmBnQ6IDoMqZQKh"",
  ""profileId"": ""pfl_QkEhN94Ba"",
  ""invoiceNumber"": null,
  ""currency"": ""EUR"",
  ""status"": ""draft"",
  ""vatScheme"": ""standard"",
  ""paymentTerm"": ""30 days"",
  ""recipientIdentifier"": ""123532354"",
  ""recipient"": {
    ""type"": ""consumer"",
    ""title"": null,
    ""givenName"": ""Given"",
    ""familyName"": ""Family"",
    ""email"": ""given.family@mollie.com"",
    ""phone"": null,
    ""streetAndNumber"": ""Street 1"",
    ""streetAdditional"": null,
    ""postalCode"": ""1000 AA"",
    ""city"": ""Amsterdam"",
    ""region"": null,
    ""country"": ""NL"",
    ""locale"": ""nl_NL""
  },
  ""lines"": [
    {
      ""description"": ""LEGO 4440 Forest Police Station"",
      ""quantity"": 1,
      ""vatRate"": ""21.00"",
      ""unitPrice"": {
        ""value"": ""89.00"",
        ""currency"": ""EUR""
      },
      ""discount"": null
    }
  ],
  ""discount"": null,
  ""amountDue"": {
    ""value"": ""107.69"",
    ""currency"": ""EUR""
  },
  ""subtotalAmount"": {
    ""value"": ""89.00"",
    ""currency"": ""EUR""
  },
  ""totalAmount"": {
    ""value"": ""107.69"",
    ""currency"": ""EUR""
  },
  ""totalVatAmount"": {
    ""value"": ""18.69"",
    ""currency"": ""EUR""
  },
  ""discountedSubtotalAmount"": {
    ""value"": ""89.00"",
    ""currency"": ""EUR""
  },
  ""createdAt"": ""2024-10-03T10:47:38.457381+00:00"",
  ""issuedAt"": null,
  ""dueAt"": null,
  ""memo"": null,
  ""metadata"": [],
  ""_links"": {
    ""self"": {
      ""href"": ""..."",
      ""type"": ""application/hal+json""
    },
    ""invoicePayment"": {
      ""href"": ""..."",
      ""type"": ""application/hal+json""
    },
    ""pdfLink"": {
      ""href"": ""..."",
      ""type"": ""application/hal+json""
    },
    ""documentation"": {
      ""href"": ""..."",
      ""type"": ""text/html""
    }
  }
}";
}
