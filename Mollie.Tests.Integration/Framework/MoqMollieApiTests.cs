using System;
using Mollie.Api.Models.Customer;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Url;

namespace Mollie.Tests.Integration.Framework {
    public class MoqMollieApiTests : BaseMollieApiTestClass {
        [TestCase]
        public async Task MoqMollieClient() {
            var mollieClientMock = new Mock<IMollieClient>();
            var paymentResponse = new PaymentResponse { Id = "dummy_payment", Links = new PaymentResponseLinks() { Checkout = new UrlLink() { Href = "http://localhost/mollietest"} } };
            var customerResponse = new CustomerResponse { Id = "dummy_customer" };

            mollieClientMock.Setup(x => x.CreatePaymentAsync(It.IsAny<PaymentRequest>())).Returns(() => Task.FromResult(paymentResponse));
            mollieClientMock.Setup(x => x.GetPaymentAsync(It.IsAny<string>(), It.IsAny<bool>())).Returns(() => Task.FromResult(paymentResponse));
            mollieClientMock.Setup(x => x.CreateCustomerAsync(It.IsAny<CustomerRequest>())).Returns(() => Task.FromResult(customerResponse));
            mollieClientMock.Setup(x => x.GetCustomerAsync(It.IsAny<string>())).Returns(() => Task.FromResult(customerResponse));

            Assert.AreEqual(paymentResponse, await mollieClientMock.Object.CreatePaymentAsync(null));
            Assert.AreEqual(paymentResponse, await mollieClientMock.Object.GetPaymentAsync(null));
            Assert.AreEqual(customerResponse, await mollieClientMock.Object.CreateCustomerAsync(null));
            Assert.AreEqual(customerResponse, await mollieClientMock.Object.GetCustomerAsync(String.Empty));
        }
    }
}
