using Shouldly;
using Mollie.Api.Framework;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Xunit;

namespace Mollie.Tests.Unit.Framework {
    public class JsonConverterServiceTests {
        [Fact]
        public void Serialize_JsonData_IsSerialized() {
            // Given: A JSON metadata value
            JsonConverterService jsonConverterService = new JsonConverterService();
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = "Description",
                RedirectUrl = "http://www.mollie.com",
                Metadata = "{\"firstName\":\"John\",\"lastName\":\"Doe\"}",
            };
            string expectedJsonValue = "{\"amount\":{\"currency\":\"EUR\",\"value\":\"100.00\"},\"description\":\"Description\",\"redirectUrl\":\"http://www.mollie.com\",\"metadata\":{\"firstName\":\"John\",\"lastName\":\"Doe\"}}";

            // When: We serialize the JSON
            string jsonValue = jsonConverterService.Serialize(paymentRequest);

            // Then:
            jsonValue.ShouldBe(expectedJsonValue);
        }

        [Fact]
        public void Deserialize_JsonData_IsDeserialized() {
            // Given: A JSON metadata value
            JsonConverterService jsonConverterService = new JsonConverterService();
            string metadataJson = @"{
  ""ReferenceNumber"": null,
  ""OrderID"": null,
  ""UserID"": ""534721""
}";
            string paymentJson = @"{""metadata"":" + metadataJson + "}";

            // When: We deserialize the JSON
            PaymentResponse payments = jsonConverterService.Deserialize<PaymentResponse>(paymentJson);

            // Then:
            payments.Metadata.ShouldBe(metadataJson);
        }

        [Fact]
        public void Deserialize_StringData_IsDeserialized() {
            // Given: A JSON metadata value
            JsonConverterService jsonConverterService = new JsonConverterService();
            string metadataJson = "This is my metadata";
            string paymentJson = @"{""metadata"":""" + metadataJson + @"""}";

            // When: We deserialize the JSON
            PaymentResponse payments = jsonConverterService.Deserialize<PaymentResponse>(paymentJson);

            // Then:
            payments.Metadata.ShouldBe(metadataJson);
        }

        [Fact]
        public void Deserialize_JsonDataWithNullValues_IsDeserialized() {
            // Given: A JSON metadata value
            JsonConverterService jsonConverterService = new JsonConverterService();
            string metadataJson = @"null";
            string paymentJson = @"{""metadata"":" + metadataJson + "}";

            // When: We deserialize the JSON
            PaymentResponse payments = jsonConverterService.Deserialize<PaymentResponse>(paymentJson);

            // Then:
            payments.Metadata.ShouldBeNull();
        }
    }
}
