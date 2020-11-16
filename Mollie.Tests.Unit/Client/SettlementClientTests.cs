using Mollie.Api.Client;
using Mollie.Api.Models.Capture;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Settlement;
using NUnit.Framework;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mollie.Tests.Unit.Client {
    [TestFixture]
    public class SettlementClientTests : BaseClientTests {
        private const string defaultSettlementId = "tr_Agfg241g";
        private const string defaultPaymentId = "tr_WDqYK6vllg";
        private const string defaultShipmentId = "shp_3wmsgCJN4U";
        private const string defaultAmountValue = "1027.99";
        private const string defaultAmountCurrency = "EUR";

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
                ""createdAt"": ""2018-08-02T09:29:56+00:00""
            }}
        ]
    }},
    ""count"": 1
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
                ]
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

        [Test]
        public async Task ListSettlementCaptures_DefaultBehaviour_ResponseIsParsed() {
            // Given: We request a list of captures
            string expectedUrl = $"{BaseMollieClient.ApiEndPoint}settlements/{defaultSettlementId}/captures";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, defaultCaptureListJsonResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            SettlementsClient settlementsClient = new SettlementsClient("api-key", httpClient);

            // When: We make the request
            ListResponse<CaptureResponse> listCaptureResponse = await settlementsClient.ListSettlementCapturesAsync(defaultSettlementId);

            // Then: Response should be parsed
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(listCaptureResponse);
            Assert.AreEqual(1, listCaptureResponse.Count);
            CaptureResponse captureResponse = listCaptureResponse.Items.First();
            Assert.AreEqual(defaultPaymentId, captureResponse.PaymentId);
            Assert.AreEqual(defaultShipmentId, captureResponse.ShipmentId);
            Assert.AreEqual(defaultSettlementId, captureResponse.SettlementId);
            Assert.AreEqual(defaultAmountValue, captureResponse.Amount.Value);
            Assert.AreEqual(defaultAmountCurrency, captureResponse.Amount.Currency);
        }

        [Test]
        public async Task GetOpenSettlement_DefaultBehaviour_ResponseIsParsed() {
            // Given: We request a list of captures
            string expectedUrl = $"{BaseMollieClient.ApiEndPoint}settlements/open";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, defaultGetSettlementResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            SettlementsClient settlementsClient = new SettlementsClient("api-key", httpClient);

            // When: We make the request
            SettlementResponse settlementResponse = await settlementsClient.GetOpenSettlement();

            // Then: Response should be parsed
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(settlementResponse);
            Assert.AreEqual(defaultAmountValue, settlementResponse.Amount.Value);
            Assert.AreEqual(defaultAmountCurrency, settlementResponse.Amount.Currency);
            Assert.AreEqual(1, settlementResponse.Periods.Count);
        }

        [Test]
        public async Task GetOpenSettlement_ResponseWithEmptyPeriods_ResponseIsParsed() {
            // Given: We request a list of captures
            string expectedUrl = $"{BaseMollieClient.ApiEndPoint}settlements/open";
            var mockHttp = this.CreateMockHttpMessageHandler(HttpMethod.Get, expectedUrl, emptyPeriodsSettlementResponse);
            HttpClient httpClient = mockHttp.ToHttpClient();
            SettlementsClient settlementsClient = new SettlementsClient("api-key", httpClient);

            // When: We make the request
            SettlementResponse settlementResponse = await settlementsClient.GetOpenSettlement();

            // Then: Response should be parsed
            mockHttp.VerifyNoOutstandingExpectation();
            Assert.IsNotNull(settlementResponse);
            Assert.AreEqual(0, settlementResponse.Periods.Count);
        }
    }    
}
