using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.Order;
using Mollie.Api.Models.Payment;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class OrderTests : BaseMollieApiTestClass {
        [Test]
        public async Task CanCreateOrderWithOnlyRequiredFields() {
            // If: we create a order request with only the required parameters
            OrderRequest orderRequest = new OrderRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                OrderNumber = "16738",
                Lines = new List<OrderLineDetails>() {
                    new OrderLineDetails() {
                        Name = "A box of chocolates",
                        Quantity = 1,
                        UnitPrice = new Amount(Currency.EUR, "100.00"),
                        TotalAmount = new Amount(Currency.EUR, "100.00"),
                        VatRate = "21.00",
                        VatAmount = new Amount(Currency.EUR, "17.36")
                    }
                },
                BillingAddress = new OrderAddressDetails() {
                    GivenName = "John",
                    FamilyName = "Smit",
                    Email = "johnsmit@gmail.com",
                    City = "Rotterdam",
                    Country = "NL",
                    PostalCode = "0000AA",
                    Region = "Zuid-Holland",
                    StreetAndNumber = "Coolsingel 1"
                },
                RedirectUrl = "http://www.google.nl",
                Locale = Locale.nl_NL
            };

            // When: We send the order request to Mollie
            OrderResponse result = await this._orderClient.CreateOrderAsync(orderRequest);

            // Then: Make sure we get a valid response
            Assert.IsNotNull(result);
            Assert.AreEqual(orderRequest.Amount.Value, result.Amount.Value);
            Assert.AreEqual(orderRequest.Amount.Currency, result.Amount.Currency);
            Assert.AreEqual(orderRequest.OrderNumber, result.OrderNumber);
        }
    }
}