using Mollie.Api.Framework;
using Mollie.Api.Models.Payment.Response;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Framework {
    [TestFixture]
    public class JsonConverterServiceTests {
        [Test]
        public void CanDeserializeJsonMetadata() {
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
            Assert.AreEqual(metadataJson, payments.Metadata);
        }

        [Test]
        public void CanDeserializeStringMetadataValue() {
            // Given: A JSON metadata value
            JsonConverterService jsonConverterService = new JsonConverterService();
            string metadataJson = "This is my metadata";
            string paymentJson = @"{""metadata"":""" + metadataJson + @"""}";

            // When: We deserialize the JSON
            PaymentResponse payments = jsonConverterService.Deserialize<PaymentResponse>(paymentJson);

            // Then: 
            Assert.AreEqual(metadataJson, payments.Metadata);
        }

        [Test]
        public void CanDeserializeNullMetadataValue() {
            // Given: A JSON metadata value
            JsonConverterService jsonConverterService = new JsonConverterService();
            string metadataJson = @"null";
            string paymentJson = @"{""metadata"":" + metadataJson + "}";

            // When: We deserialize the JSON
            PaymentResponse payments = jsonConverterService.Deserialize<PaymentResponse>(paymentJson);

            // Then: 
            Assert.AreEqual(null, payments.Metadata);
        }
    }
}
