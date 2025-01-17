using System;
using Mollie.Api.Client;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Models.Capture.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Settlement.Response;
using RichardSzalay.MockHttp;
using Xunit;

namespace Mollie.Tests.Unit.Client {
    public class SettlementClientTests : BaseClientTests {
        [Fact]
        public async Task ListSettlementCaptures_DefaultBehaviour_ResponseIsParsed() {
            // Given: We request a list of captures
            string expectedUrl = $"{BaseMollieClient.ApiEndPoint}settlements/{defaultSettlementId}/captures";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, defaultCaptureListJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            SettlementClient settlementClient = new SettlementClient("api-key", httpClient);

            // When: We make the request
            ListResponse<CaptureResponse> listCaptureResponse = await settlementClient.GetSettlementCaptureListAsync(defaultSettlementId);

            // Then: Response should be parsed
            mockHttp.VerifyNoOutstandingExpectation();
            listCaptureResponse.ShouldNotBeNull();
            listCaptureResponse.Count.ShouldBe(1);
            listCaptureResponse.Links.Self.Href.ShouldBe("https://api.mollie.com/v2/settlements/stl_jDk30akdN/captures?limit=50");
            listCaptureResponse.Links.Self.Type.ShouldBe("application/hal+json");
            listCaptureResponse.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/v2/settlements-api/list-settlement-captures");
            listCaptureResponse.Links.Documentation.Type.ShouldBe("text/html");
            CaptureResponse captureResponse = listCaptureResponse.Items.First();
            captureResponse.PaymentId.ShouldBe(defaultPaymentId);
            captureResponse.ShipmentId.ShouldBe(defaultShipmentId);
            captureResponse.SettlementId.ShouldBe(defaultSettlementId);
            captureResponse.Amount.Value.ShouldBe(defaultAmountValue);
            captureResponse.Amount.Currency.ShouldBe(defaultAmountCurrency);
            captureResponse.Links.ShouldNotBeNull();
            captureResponse.Links.Self.Href.ShouldBe($"https://api.mollie.com/v2/payments/{defaultPaymentId}/captures/cpt_4qqhO89gsT");
            captureResponse.Links.Self.Type.ShouldBe("application/hal+json");
            captureResponse.Links.Payment.Href.ShouldBe($"https://api.mollie.com/v2/payments/{defaultPaymentId}");
            captureResponse.Links.Payment.Type.ShouldBe("application/hal+json");
            captureResponse.Links.Shipment.Href.ShouldBe($"https://api.mollie.com/v2/orders/ord_8wmqcHMN4U/shipments/shp_3wmsgCJN4U");
            captureResponse.Links.Shipment.Type.ShouldBe("application/hal+json");
            captureResponse.Links.Settlement.Href.ShouldBe($"https://api.mollie.com/v2/settlements/stl_jDk30akdN");
            captureResponse.Links.Settlement.Type.ShouldBe("application/hal+json");
            captureResponse.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/v2/captures-api/get-capture");
            captureResponse.Links.Documentation.Type.ShouldBe("text/html");
        }

        [Fact]
        public async Task GetOpenSettlement_DefaultBehaviour_ResponseIsParsed() {
            // Given: We request a list of captures
            string expectedUrl = $"{BaseMollieClient.ApiEndPoint}settlements/open";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, defaultGetSettlementResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            SettlementClient settlementClient = new SettlementClient("api-key", httpClient);

            // When: We make the request
            SettlementResponse settlementResponse = await settlementClient.GetOpenSettlement();

            // Then: Response should be parsed
            mockHttp.VerifyNoOutstandingExpectation();
            settlementResponse.ShouldNotBeNull();
            settlementResponse.Amount.Value.ShouldBe(defaultAmountValue);
            settlementResponse.Amount.Currency.ShouldBe(defaultAmountCurrency);
            settlementResponse.Periods.Count.ShouldBe(1);
            settlementResponse.Periods[2018][4].InvoiceId.ShouldBe(defaultInvoiceId);
            settlementResponse.Periods[2018][4].Revenue.Count.ShouldBe(2);
            settlementResponse.Periods[2018][4].Revenue[0].Description.ShouldBe("iDEAL");
            settlementResponse.Periods[2018][4].Revenue[0].Method.ShouldBe("ideal");
            settlementResponse.Periods[2018][4].Revenue[0].Count.ShouldBe(6);
            settlementResponse.Periods[2018][4].Revenue[0].AmountNet.Value.ShouldBe("86.1000");
            settlementResponse.Periods[2018][4].Revenue[0].AmountNet.Currency.ShouldBe("EUR");
            settlementResponse.Periods[2018][4].Revenue[0].AmountVat.ShouldBeNull();
            settlementResponse.Periods[2018][4].Revenue[0].AmountGross.Value.ShouldBe("86.1000");
            settlementResponse.Periods[2018][4].Revenue[0].AmountGross.Currency.ShouldBe("EUR");
            settlementResponse.Periods[2018][4].Revenue[1].Description.ShouldBe("Refunds iDEAL");
            settlementResponse.Periods[2018][4].Revenue[1].Method.ShouldBe("refund");
            settlementResponse.Periods[2018][4].Revenue[1].Count.ShouldBe(2);
            settlementResponse.Periods[2018][4].Revenue[1].AmountNet.Value.ShouldBe("-43.2000");
            settlementResponse.Periods[2018][4].Revenue[1].AmountNet.Currency.ShouldBe("EUR");
            settlementResponse.Periods[2018][4].Revenue[1].AmountVat.ShouldBeNull();
            settlementResponse.Periods[2018][4].Revenue[1].AmountGross.Value.ShouldBe("43.2000");
            settlementResponse.Periods[2018][4].Revenue[1].AmountGross.Currency.ShouldBe("EUR");
            settlementResponse.Periods[2018][4].Costs.Count.ShouldBe(2);
            settlementResponse.Periods[2018][4].Costs[0].Description.ShouldBe("iDEAL");
            settlementResponse.Periods[2018][4].Costs[0].Method.ShouldBe("ideal");
            settlementResponse.Periods[2018][4].Costs[0].Count.ShouldBe(6);
            settlementResponse.Periods[2018][4].Costs[0].Rate.Fixed.Value.ShouldBe("0.3500");
            settlementResponse.Periods[2018][4].Costs[0].Rate.Fixed.Currency.ShouldBe("EUR");
            settlementResponse.Periods[2018][4].Costs[0].Rate.Percentage.ShouldBeNull();
            settlementResponse.Periods[2018][4].Costs[0].AmountNet.Value.ShouldBe("2.1000");
            settlementResponse.Periods[2018][4].Costs[0].AmountNet.Currency.ShouldBe("EUR");
            settlementResponse.Periods[2018][4].Costs[0].AmountVat.Value.ShouldBe("0.4410");
            settlementResponse.Periods[2018][4].Costs[0].AmountVat.Currency.ShouldBe("EUR");
            settlementResponse.Periods[2018][4].Costs[0].AmountGross.Value.ShouldBe("2.5410");
            settlementResponse.Periods[2018][4].Costs[0].AmountGross.Currency.ShouldBe("EUR");
            settlementResponse.Periods[2018][4].Costs[1].Description.ShouldBe("Refunds iDEAL");
            settlementResponse.Periods[2018][4].Costs[1].Method.ShouldBe("refund");
            settlementResponse.Periods[2018][4].Costs[1].Count.ShouldBe(2);
            settlementResponse.Periods[2018][4].Costs[1].Rate.Fixed.Value.ShouldBe("0.2500");
            settlementResponse.Periods[2018][4].Costs[1].Rate.Fixed.Currency.ShouldBe("EUR");
            settlementResponse.Periods[2018][4].Costs[1].Rate.Percentage.ShouldBeNull();
            settlementResponse.Periods[2018][4].Costs[1].AmountNet.Value.ShouldBe("0.5000");
            settlementResponse.Periods[2018][4].Costs[1].AmountNet.Currency.ShouldBe("EUR");
            settlementResponse.Periods[2018][4].Costs[1].AmountVat.Value.ShouldBe("0.1050");
            settlementResponse.Periods[2018][4].Costs[1].AmountVat.Currency.ShouldBe("EUR");
            settlementResponse.Periods[2018][4].Costs[1].AmountGross.Value.ShouldBe("0.6050");
            settlementResponse.Periods[2018][4].Costs[1].AmountGross.Currency.ShouldBe("EUR");
            settlementResponse.Links.ShouldNotBeNull();
            settlementResponse.Links.Self.Href.ShouldBe("https://api.mollie.com/v2/settlements/open");
            settlementResponse.Links.Self.Type.ShouldBe("application/hal+json");
            settlementResponse.Links.Documentation.Href.ShouldBe("https://docs.mollie.com/reference/v2/settlements-api/get-open-settlement");
            settlementResponse.Links.Documentation.Type.ShouldBe("text/html");
        }

        [Fact]
        public async Task GetOpenSettlement_ResponseWithEmptyPeriods_ResponseIsParsed() {
            // Given: We request a list of captures
            string expectedUrl = $"{BaseMollieClient.ApiEndPoint}settlements/open";
            var mockHttp = CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, emptyPeriodsSettlementResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            SettlementClient settlementClient = new SettlementClient("api-key", httpClient);

            // When: We make the request
            SettlementResponse settlementResponse = await settlementClient.GetOpenSettlement();

            // Then: Response should be parsed
            mockHttp.VerifyNoOutstandingExpectation();
            settlementResponse.ShouldNotBeNull();
            settlementResponse.Periods.Count.ShouldBe(0);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetSettlementAsync_NoSettlementIdIsGiven_ArgumentExceptionIsThrown(string settlementId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SettlementClient settlementClient = new SettlementClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await settlementClient.GetSettlementAsync(settlementId));

            // Then
            exception.Message.ShouldBe("Required URL argument 'settlementId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetSettlementPaymentsListAsync_NoSettlementIdIsGiven_ArgumentExceptionIsThrown(string settlementId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SettlementClient settlementClient = new SettlementClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await settlementClient.GetSettlementPaymentListAsync(settlementId));

            // Then
            exception.Message.ShouldBe("Required URL argument 'settlementId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetSettlementRefundsListAsync_NoSettlementIdIsGiven_ArgumentExceptionIsThrown(string settlementId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SettlementClient settlementClient = new SettlementClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await settlementClient.GetSettlementRefundListAsync(settlementId));

            // Then
            exception.Message.ShouldBe("Required URL argument 'settlementId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetSettlementChargebacksListAsync_NoSettlementIdIsGiven_ArgumentExceptionIsThrown(string settlementId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SettlementClient settlementClient = new SettlementClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await settlementClient.GetSettlementChargebackListAsync(settlementId));

            // Then
            exception.Message.ShouldBe("Required URL argument 'settlementId' is null or empty");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async Task GetSettlementCapturesListAsync_NoSettlementIdIsGiven_ArgumentExceptionIsThrown(string settlementId) {
            // Arrange
            var mockHttp = new MockHttpMessageHandler();
            HttpClient httpClient = mockHttp.ToHttpClient();
            SettlementClient settlementClient = new SettlementClient("api-key", httpClient);

            // When: We send the request
            var exception = await Assert.ThrowsAsync<ArgumentException>(async () => await settlementClient.GetSettlementCaptureListAsync(settlementId));

            // Then
            exception.Message.ShouldBe("Required URL argument 'settlementId' is null or empty");
        }


        private const string defaultSettlementId = "tr_Agfg241g";
        private const string defaultPaymentId = "tr_WDqYK6vllg";
        private const string defaultShipmentId = "shp_3wmsgCJN4U";
        private const string defaultAmountValue = "1027.99";
        private const string defaultAmountCurrency = "EUR";
        private const string defaultInvoiceId = "inv_FrvewDA3Pr";

        private string defaultCaptureListJsonResponse = $@"{{
    ""_embedded"": {{
        ""captures"": [
            {{
                ""resource"": ""capture"",
                ""id"": ""cpt_4qqhO89gsT"",
                ""mode"": ""live"",
                ""amount"": {{
                    ""value"": ""{defaultAmountValue}"",
                    ""currency"": ""{defaultAmountCurrency}""
                }},
                ""settlementAmount"": {{
                    ""value"": ""1027.99"",
                    ""currency"": ""EUR""
                }},
                ""paymentId"": ""{defaultPaymentId}"",
                ""shipmentId"": ""{defaultShipmentId}"",
                ""settlementId"": ""{defaultSettlementId}"",
                ""createdAt"": ""2018-08-02T09:29:56+00:00"",
                ""_links"": {{
                    ""self"": {{
                        ""href"": ""https://api.mollie.com/v2/payments/tr_WDqYK6vllg/captures/cpt_4qqhO89gsT"",
                        ""type"": ""application/hal+json""
                    }},
                    ""payment"": {{
                        ""href"": ""https://api.mollie.com/v2/payments/tr_WDqYK6vllg"",
                        ""type"": ""application/hal+json""
                    }},
                    ""shipment"": {{
                        ""href"": ""https://api.mollie.com/v2/orders/ord_8wmqcHMN4U/shipments/shp_3wmsgCJN4U"",
                        ""type"": ""application/hal+json""
                    }},
                    ""settlement"": {{
                        ""href"": ""https://api.mollie.com/v2/settlements/stl_jDk30akdN"",
                        ""type"": ""application/hal+json""
                    }},
                    ""documentation"": {{
                        ""href"": ""https://docs.mollie.com/reference/v2/captures-api/get-capture"",
                        ""type"": ""text/html""
                    }}
                }}
            }}
        ]
    }},
    ""count"": 1,
    ""_links"": {{
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/v2/settlements-api/list-settlement-captures"",
            ""type"": ""text/html""
        }},
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/settlements/stl_jDk30akdN/captures?limit=50"",
            ""type"": ""application/hal+json""
        }},
        ""previous"": null,
        ""next"": null
    }}
}}";

        private string defaultGetSettlementResponse = $@"{{
    ""resource"": ""settlement"",
    ""id"": ""open"",
    ""status"": ""open"",
    ""amount"": {{
        ""currency"": ""{defaultAmountCurrency}"",
        ""value"": ""{defaultAmountValue}""
    }},
    ""periods"": {{
        ""2018"": {{
            ""04"": {{
                ""revenue"": [
                    {{
                        ""description"": ""iDEAL"",
                        ""method"": ""ideal"",
                        ""count"": 6,
                        ""amountNet"": {{
                            ""value"": ""86.1000"",
                            ""currency"": ""EUR""
                        }},
                        ""amountVat"": null,
                        ""amountGross"": {{
                            ""value"": ""86.1000"",
                            ""currency"": ""EUR""
                        }}
                    }},
                    {{
                        ""description"": ""Refunds iDEAL"",
                        ""method"": ""refund"",
                        ""count"": 2,
                        ""amountNet"": {{
                            ""value"": ""-43.2000"",
                            ""currency"": ""EUR""
                        }},
                        ""amountVat"": null,
                        ""amountGross"": {{
                            ""value"": ""43.2000"",
                            ""currency"": ""EUR""
                        }}
                    }}
                ],
                ""costs"": [
                    {{
                        ""description"": ""iDEAL"",
                        ""method"": ""ideal"",
                        ""count"": 6,
                        ""rate"": {{
                            ""fixed"": {{
                                ""value"": ""0.3500"",
                                ""currency"": ""EUR""
                            }},
                            ""percentage"": null
                        }},
                        ""amountNet"": {{
                            ""value"": ""2.1000"",
                            ""currency"": ""EUR""
                        }},
                        ""amountVat"": {{
                            ""value"": ""0.4410"",
                            ""currency"": ""EUR""
                        }},
                        ""amountGross"": {{
                            ""value"": ""2.5410"",
                            ""currency"": ""EUR""
                        }}
                    }},
                    {{
                        ""description"": ""Refunds iDEAL"",
                        ""method"": ""refund"",
                        ""count"": 2,
                        ""rate"": {{
                            ""fixed"": {{
                                ""value"": ""0.2500"",
                                ""currency"": ""EUR""
                            }},
                            ""percentage"": null
                        }},
                        ""amountNet"": {{
                            ""value"": ""0.5000"",
                            ""currency"": ""EUR""
                        }},
                        ""amountVat"": {{
                            ""value"": ""0.1050"",
                            ""currency"": ""EUR""
                        }},
                        ""amountGross"": {{
                            ""value"": ""0.6050"",
                            ""currency"": ""EUR""
                        }}
                    }}
                ],
                ""invoiceId"": ""{defaultInvoiceId}""
            }}
        }}
    }},
    ""_links"": {{
        ""self"": {{
            ""href"": ""https://api.mollie.com/v2/settlements/open"",
            ""type"": ""application/hal+json""
        }},
        ""documentation"": {{
            ""href"": ""https://docs.mollie.com/reference/v2/settlements-api/get-open-settlement"",
            ""type"": ""text/html""
        }}
    }}
}}";

        private readonly string emptyPeriodsSettlementResponse = @$"{{
   ""resource"":""settlement"",
   ""id"":""open"",
   ""createdAt"":""2020-11-11T07:10:53+00:00"",
   ""status"":""open"",
   ""amount"":{{
      ""value"":""0.00"",
      ""currency"":""EUR""
   }},
   ""periods"":[

   ],
   ""_links"":{{
      ""self"":{{
         ""href"":""https://api.mollie.com/v2/settlements/open"",
         ""type"":""application/hal+json""
      }},
      ""documentation"":{{
         ""href"":""https://docs.mollie.com/reference/v2/settlements-api/get-open-settlement"",
         ""type"":""text/html""
      }}
   }}
}}";
    }
}
