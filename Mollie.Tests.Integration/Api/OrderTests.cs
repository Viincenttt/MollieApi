using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Order;
using Mollie.Api.Models.Order.Request.PaymentSpecificParameters;
using Mollie.Api.Models.Payment;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class OrderTests : BaseMollieApiTestClass {
        [Test]
        public async Task CanCreateOrderWithOnlyRequiredFields() {
            // If: we create a order request with only the required parameters
            OrderRequest orderRequest = this.CreateOrderRequestWithOnlyRequiredFields();

            // When: We send the order request to Mollie
            OrderResponse result = await this._orderClient.CreateOrderAsync(orderRequest);

            // Then: Make sure we get a valid response
            Assert.IsNotNull(result);
            Assert.AreEqual(orderRequest.Amount.Value, result.Amount.Value);
            Assert.AreEqual(orderRequest.Amount.Currency, result.Amount.Currency);
            Assert.AreEqual(orderRequest.OrderNumber, result.OrderNumber);
        }

        [Test]
        public async Task CanCreateOrderWithPaymentSpecificOptions() {
            // If: we create a order request with payment specific parameters
            OrderRequest orderRequest = this.CreateOrderRequestWithOnlyRequiredFields();
            orderRequest.Payment = new IDealSpecificParameters() {
                Issuer = "ideal_INGBNL2A"
            };

            // When: We send the order request to Mollie
            OrderResponse result = await this._orderClient.CreateOrderAsync(orderRequest);

            // Then: Make sure we get a valid response
            Assert.IsNotNull(result);
            Assert.AreEqual(orderRequest.Amount.Value, result.Amount.Value);
            Assert.AreEqual(orderRequest.Amount.Currency, result.Amount.Currency);
            Assert.AreEqual(orderRequest.OrderNumber, result.OrderNumber);
        }

        [Test]
        public async Task CanRetrieveOrderAfterCreationOrder() {
            // If: we create a new order
            OrderRequest orderRequest = this.CreateOrderRequestWithOnlyRequiredFields();
            OrderResponse createdOrder = await this._orderClient.CreateOrderAsync(orderRequest);

            // When: We attempt to retrieve the order
            OrderResponse retrievedOrder = await this._orderClient.GetOrderAsync(createdOrder.Id);

            // Then: Make sure we get a valid response
            Assert.IsNotNull(retrievedOrder);
            Assert.AreEqual(createdOrder.Id, retrievedOrder.Id);
        }

        [Test]
        public async Task CanUpdateExistingOrder() {
            // If: we create a new order
            OrderRequest orderRequest = this.CreateOrderRequestWithOnlyRequiredFields();
            OrderResponse createdOrder = await this._orderClient.CreateOrderAsync(orderRequest);

            // When: We attempt to update the order
            OrderUpdateRequest orderUpdateRequest = new OrderUpdateRequest() {
                OrderNumber = "1337",
                BillingAddress = createdOrder.BillingAddress
            };
            orderUpdateRequest.BillingAddress.City = "Den Haag";
            OrderResponse updatedOrder = await this._orderClient.UpdateOrderAsync(createdOrder.Id, orderUpdateRequest);

            // Then: Make sure the order is updated
            Assert.AreEqual(orderUpdateRequest.OrderNumber, updatedOrder.OrderNumber);
            Assert.AreEqual(orderUpdateRequest.BillingAddress.City, updatedOrder.BillingAddress.City);
        }

        [Test]
        public async Task CanCancelCreatedOrder() {
            // If: we create a new order
            OrderRequest orderRequest = this.CreateOrderRequestWithOnlyRequiredFields();
            OrderResponse createdOrder = await this._orderClient.CreateOrderAsync(orderRequest);

            // When: We attempt to cancel the order and then retrieve it
            await this._orderClient.CancelOrderAsync(createdOrder.Id);
            OrderResponse canceledOrder = await this._orderClient.GetOrderAsync(createdOrder.Id);

            // Then: The order status should be cancelled
            Assert.AreEqual(OrderStatus.Canceled, canceledOrder.Status);
        }

        [Test]
        public async Task CanUpdateOrderLine() {
            // If: we create a new order
            OrderRequest orderRequest = this.CreateOrderRequestWithOnlyRequiredFields();
            OrderResponse createdOrder = await this._orderClient.CreateOrderAsync(orderRequest);

            // When: We update the order line
            OrderLineUpdateRequest updateRequest = new OrderLineUpdateRequest() {
                Name = "A fluffy bear"
            };
            OrderResponse updatedOrder = await this._orderClient.UpdateOrderLinesAsync(createdOrder.Id, createdOrder.Lines.First().Id, updateRequest);

            // Then: The name of the order line should be updated
            Assert.AreEqual(updateRequest.Name, updatedOrder.Lines.First().Name);
        }

        [Test]
        public async Task CanRetrieveOrderList() {
            // When: Retrieve payment list with default settings
            ListResponse<OrderResponse> response = await this._orderClient.GetOrderListAsync();

            // Then
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Items);
        }

        [Test]
        public async Task ListOrdersNeverReturnsMorePaymentsThenTheNumberOfRequestedOrders() {
            // If: Number of orders requested is 5
            int numberOfOrders = 5;

            // When: Retrieve 5 orders
            ListResponse<OrderResponse> response = await this._orderClient.GetOrderListAsync(null, numberOfOrders);

            // Then
            Assert.IsTrue(response.Items.Count <= numberOfOrders);
        }

        private OrderRequest CreateOrderRequestWithOnlyRequiredFields() {
            return new OrderRequest() {
                Amount = new Amount(Currency.EUR, "100.00"),
                OrderNumber = "16738",
                Lines = new List<OrderLineRequest>() {
                    new OrderLineRequest() {
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
        }
    }
}