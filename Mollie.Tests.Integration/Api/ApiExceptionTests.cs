using System;
using System.Linq;
using Mollie.Api.Client;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment.Request;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class ApiExceptionTests : BaseMollieApiTestClass {
        [Test][RetryOnFailure(BaseMollieApiTestClass.NumberOfRetries)]
        public void ShouldThrowMollieApiExceptionWhenInvalidParametersAreGiven() {
            // If: we create a payment request with invalid parameters
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                Description = null,
                RedirectUrl = null
            };
            
            // Then: Send the payment request to the Mollie Api, this should throw a mollie api exception
            AggregateException aggregateException = Assert.Throws<AggregateException>(() => this._paymentClient.CreatePaymentAsync(paymentRequest).Wait());
            MollieApiException mollieApiException = aggregateException.InnerExceptions.FirstOrDefault(x => x.GetType() == typeof(MollieApiException)) as MollieApiException;
            Assert.IsNotNull(mollieApiException);
            Assert.IsNotNull(mollieApiException.Details);
            Assert.True(!String.IsNullOrEmpty(mollieApiException.Details.Detail));
        }
    }
}
