using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class InvoiceClientTests : BaseClientTests
{
    [Fact]
    public async Task GetInvoiceAsync_DefaultBehaviour_ResponseIsParsed()
    {
        // Given
        const string invoiceId = "inv_xBEbP9rvAq";
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.ApiEndPoint}invoices/{invoiceId}")
            .Respond("application/json", defaultInvoice);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using InvoiceClient invoiceClient = new InvoiceClient("access_abcde", httpClient);

        // When
        var result = await invoiceClient.GetInvoiceAsync(invoiceId);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.ShouldNotBeNull();
        result.Resource.ShouldBe("invoice");
        result.Id.ShouldBe(invoiceId);
        result.Reference.ShouldBe("2016.10000");
        result.VatNumber.ShouldBe("NL001234567B01");
        result.Status.ShouldBe("open");
    }

    [Theory]
    [InlineData(null, null, null, null, "")]
    [InlineData("my-reference", null, null, null, "?reference=my-reference")]
    [InlineData("my-reference", 2023, null, null, "?reference=my-reference&year=2023")]
    [InlineData("my-reference", 2023, "abcde", null, "?reference=my-reference&year=2023&from=abcde")]
    [InlineData("my-reference", 2023, "abcde", 10, "?reference=my-reference&year=2023&from=abcde&limit=10")]
    public async Task GetInvoiceListAsync_WithVariousParameters_QueryStringMatchesExpectedValue(
        string reference, int? year, string from, int? limit, string expectedQueryString) {
        // Given: We retrieve a list of customers
        var mockHttp = new MockHttpMessageHandler();
        mockHttp.When($"{BaseMollieClient.ApiEndPoint}invoices{expectedQueryString}")
            .Respond("application/json", defaultInvoiceList);
        HttpClient httpClient = mockHttp.ToHttpClient();
        using InvoiceClient invoiceClient = new InvoiceClient("access_abcde", httpClient);

        // When: We send the request
        var result = await invoiceClient.GetInvoiceListAsync(
            reference, year, from, limit);

        // Then
        mockHttp.VerifyNoOutstandingExpectation();
        result.ShouldNotBeNull();
    }

    private const string defaultInvoice = @"{
    ""resource"": ""invoice"",
    ""id"": ""inv_xBEbP9rvAq"",
    ""reference"": ""2016.10000"",
    ""vatNumber"": ""NL001234567B01"",
    ""status"": ""open"",
    ""issuedAt"": ""2016-08-31"",
    ""dueAt"": ""2016-09-14"",
    ""netAmount"": {
        ""value"": ""45.00"",
        ""currency"": ""EUR""
    },
    ""vatAmount"": {
        ""value"": ""9.45"",
        ""currency"": ""EUR""
    },
    ""grossAmount"": {
        ""value"": ""54.45"",
        ""currency"": ""EUR""
    },
    ""lines"":[
        {
            ""period"": ""2016-09"",
            ""description"": ""iDEAL transactiekosten"",
            ""count"": 100,
            ""vatPercentage"": 21,
            ""amount"": {
                ""value"": ""45.00"",
                ""currency"": ""EUR""
            }
        }
    ],
    ""_links"": {
        ""self"": {
             ""href"": ""https://api.mollie.com/v2/invoices/inv_xBEbP9rvAq"",
             ""type"": ""application/hal+json""
        },
        ""pdf"": {
             ""href"": ""https://www.mollie.com/merchant/download/invoice/xBEbP9rvAq/2ab44d60b35b1d06090bba955fa2c602"",
             ""type"": ""application/pdf"",
             ""expiresAt"": ""2018-11-09T14:10:36+00:00""
        }
    }
}";

    private const string defaultInvoiceList = @"{
    ""count"": 5,
    ""_embedded"": {
        ""invoices"": [
            {
                ""resource"": ""invoice"",
                ""id"": ""inv_xBEbP9rvAq"",
                ""reference"": ""2016.10000"",
                ""vatNumber"": ""NL001234567B01"",
                ""status"": ""open"",
                ""issuedAt"": ""2016-08-31"",
                ""dueAt"": ""2016-09-14"",
                ""netAmount"": {
                    ""value"": ""45.00"",
                    ""currency"": ""EUR""
                },
                ""vatAmount"": {
                    ""value"": ""9.45"",
                    ""currency"": ""EUR""
                },
                ""grossAmount"": {
                    ""value"": ""54.45"",
                    ""currency"": ""EUR""
                },
                ""lines"":[
                    {
                        ""period"": ""2016-09"",
                        ""description"": ""iDEAL transactiekosten"",
                        ""count"": 100,
                        ""vatPercentage"": 21,
                        ""amount"": {
                            ""value"": ""45.00"",
                            ""currency"": ""EUR""
                        }
                    }
                ],
                ""_links"": {
                    ""self"": {
                         ""href"": ""https://api.mollie.com/v2/invoices/inv_xBEbP9rvAq"",
                         ""type"": ""application/hal+json""
                    },
                    ""pdf"": {
                         ""href"": ""https://www.mollie.com/merchant/download/invoice/xBEbP9rvAq/2ab44d60b35955fa2c602"",
                         ""type"": ""application/pdf"",
                         ""expiresAt"": ""2018-11-09T14:10:36+00:00""
                    }
                }
            },
            { },
            { },
            { },
            { }
        ]
    },
    ""_links"": {
        ""self"": {
            ""href"": ""https://api.mollie.com/v2/invoices?limit=5"",
            ""type"": ""application/hal+json""
        },
        ""previous"": null,
        ""next"": {
            ""href"": ""https://api.mollie.com/v2/invoices?from=inv_xBEbP9rvAq&limit=5"",
            ""type"": ""application/hal+json""
        },
        ""documentation"": {
            ""href"": ""https://docs.mollie.com/reference/v2/invoices-api/list-invoices"",
            ""type"": ""text/html""
        }
    }
}";
}
