using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Order;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Order.Request.ManageOrderLines;
using Mollie.Api.Models.Order.Request.PaymentSpecificParameters;
using Mollie.Api.Models.Order.Response;
using Mollie.Api.Models.Payment;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class OrderTests : BaseMollieApiTestClass, IDisposable {
    private readonly IOrderClient _orderClient;

    public OrderTests() {
        _orderClient = new OrderClient(ApiKey);
    }

    [DefaultRetryFact]
    public async Task GetOrderListAsync_WithoutSortOrder_ReturnsOrdersInDescendingOrder()
    {
        // Act
        var orders = await _orderClient.GetOrderListAsync();

        // Assert
        if (orders.Items.Any())
        {
            orders.Items.Should().BeInDescendingOrder(x => x.CreatedAt);
        }
    }

    [DefaultRetryFact]
    public async Task GetOrderListAsync_InDescendingOrder_ReturnsOrdersInDescendingOrder()
    {
        // Act
        var orders = await _orderClient.GetOrderListAsync(sort: SortDirection.Desc);

        // Assert
        if (orders.Items.Any())
        {
            orders.Items.Should().BeInDescendingOrder(x => x.CreatedAt);
        }
    }

    [DefaultRetryFact]
    public async Task GetOrderListAsync_InAscendingOrder_ReturnsOrdersInAscendingOrder()
    {
        // Act
        var orders = await _orderClient.GetOrderListAsync(sort: SortDirection.Asc);

        // Assert
        if (orders.Items.Any())
        {
            orders.Items.Should().BeInAscendingOrder(x => x.CreatedAt);
        }
    }

    [DefaultRetryFact]
    public async Task CreateOrderAsync_OrderWithRequiredFields_OrderIsCreated() {
        // If: we create a order request with only the required parameters
        OrderRequest orderRequest = CreateOrder();

        // When: We send the order request to Mollie
        OrderResponse result = await _orderClient.CreateOrderAsync(orderRequest);

        // Then: Make sure we get a valid response
        result.Should().NotBeNull();
        result.Amount.Should().Be(orderRequest.Amount);
        result.OrderNumber.Should().Be(orderRequest.OrderNumber);
        result.Lines.Should().HaveCount(orderRequest.Lines.Count());
        result.Links.Should().NotBeNull();
        OrderLineRequest orderLineRequest = orderRequest.Lines.First();
        OrderLineResponse orderResponseLine = result.Lines.First();
        orderResponseLine.Type.Should().Be(orderLineRequest.Type);
        orderResponseLine.Links.ImageUrl!.Href.Should().Be(orderLineRequest.ImageUrl);
        orderResponseLine.Links.ProductUrl!.Href.Should().Be(orderLineRequest.ProductUrl);
        var expectedMetadataString = result.Lines.First().Metadata;
        orderResponseLine.Metadata.Should().Be(expectedMetadataString);
    }

    [DefaultRetryFact]
    public async Task CreateOrderAsync_OrderWithApplicationFee_OrderIsCreated() {
        // If: we create a order request with only the required parameters
        OrderRequest orderRequest = CreateOrder() with {
            Payment = new OrderPaymentParameters {
                ApplicationFee = new ApplicationFee {
                    Amount = new Amount(Currency.EUR, 0.25m),
                    Description = "Test"
                }
            }
        };

        // When: We send the order request to Mollie
        OrderResponse result = await _orderClient.CreateOrderAsync(orderRequest);

        // Then: Make sure we get a valid response
        result.Should().NotBeNull();
        result.Amount.Should().Be(orderRequest.Amount);
        result.OrderNumber.Should().Be(orderRequest.OrderNumber);
        result.Lines.Should().HaveCount(orderRequest.Lines.Count());
        result.Links.Should().NotBeNull();
        OrderLineRequest orderLineRequest = orderRequest.Lines.First();
        OrderLineResponse orderResponseLine = result.Lines.First();
        orderResponseLine.Type.Should().Be(orderLineRequest.Type);
        orderResponseLine.Links.ImageUrl!.Href.Should().Be(orderLineRequest.ImageUrl);
        orderResponseLine.Links.ProductUrl!.Href.Should().Be(orderLineRequest.ProductUrl);
        var expectedMetadataString = result.Lines.First().Metadata;
        orderResponseLine.Metadata.Should().Be(expectedMetadataString);
    }

    [DefaultRetryFact]
    public async Task CreateOrderAsync_WithMultiplePaymentMethods_OrderIsCreated() {
        // When: we create a order request and specify multiple payment methods
        OrderRequest orderRequest = CreateOrder();
        orderRequest.Methods = new List<string>() {
            PaymentMethod.Ideal,
            PaymentMethod.CreditCard,
            PaymentMethod.DirectDebit
        };

        // When: We send the order request to Mollie
        OrderResponse result = await _orderClient.CreateOrderAsync(orderRequest);

        // Then: Make sure we get a valid response
        result.Should().NotBeNull();
        result.Amount.Should().Be(orderRequest.Amount);
        result.OrderNumber.Should().Be(orderRequest.OrderNumber);
    }

    [DefaultRetryFact]
    public async Task CreateOrderAsync_WithSinglePaymentMethod_OrderIsCreated() {
        // When: we create a order request and specify a single payment method
        OrderRequest orderRequest = CreateOrder();
        orderRequest.Method = PaymentMethod.CreditCard;

        // When: We send the order request to Mollie
        OrderResponse result = await _orderClient.CreateOrderAsync(orderRequest);

        // Then: Make sure we get a valid response
        orderRequest.Method.Should().Be(PaymentMethod.CreditCard);
        orderRequest.Methods!.First().Should().Be(PaymentMethod.CreditCard);
        result.Should().NotBeNull();
        result.Amount.Should().Be(orderRequest.Amount);
        result.OrderNumber.Should().Be(orderRequest.OrderNumber);
    }

    public static IEnumerable<object[]> PaymentSpecificParameters =>
        new List<object[]>
        {
            new object[] {
                new KlarnaSpecificParameters<object> {
                    ExtraMerchantData = new {
                        payment_history_simple = new[] {
                            new {
                                unique_account_identifier = "Adam Adamsson",
                                paid_before = true
                            }
                        }
                    }
                }
            },
            new object[] {
                new BillieSpecificParameters {
                    Company = new CompanyObject {
                        EntityType = CompanyEntityType.LimitedCompany,
                        RegistrationNumber = "registration-number",
                        VatNumber = "vat-number"
                    }
                }
            },
            new object[] {
                new GiftcardSpecificParameters() {
                    Issuer = "boekenbon",
                    VoucherNumber = "voucher-number",
                    VoucherPin = "1234"
                }
            },
            new object[] {
                new IDealSpecificParameters {
                    Issuer = "ideal_INGBNL2A"
                }
            },
            new object[] {
                new KbcSpecificParameters {
                    Issuer = "ideal_INGBNL2A"
                }
            },
            new object[] {
                new PaySafeCardSpecificParameters {
                    CustomerReference = "customer-reference"
                }
            },
            new object[] {
                new SepaDirectDebitSpecificParameters {
                    ConsumerAccount = "Consumer account"
                }
            }
        };

    [DefaultRetryTheory]
    [MemberData(nameof(PaymentSpecificParameters))]
    public async Task CreateOrderAsync_WithPaymentSpecificParameters_OrderIsCreated(
        OrderPaymentParameters paymentSpecificParameters) {

        // If: we create a order request with payment specific parameters
        OrderRequest orderRequest = CreateOrder();
        orderRequest.BillingAddress!.Country = "DE"; // Billie only works in Germany
        orderRequest.BillingAddress.OrganizationName = "Mollie"; // Billie requires a organization name
        orderRequest.Payment = paymentSpecificParameters;

        // When: We send the order request to Mollie
        OrderResponse result = await _orderClient.CreateOrderAsync(orderRequest);

        // Then: Make sure we get a valid response
        result.Should().NotBeNull();
        result.Amount.Should().Be(orderRequest.Amount);
        result.OrderNumber.Should().Be(orderRequest.OrderNumber);
    }

    [DefaultRetryFact]
    public async Task GetOrderAsync_OrderIsCreated_OrderCanBeRetrieved() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        OrderResponse createdOrder = await _orderClient.CreateOrderAsync(orderRequest);

        // When: We attempt to retrieve the order
        OrderResponse retrievedOrder = await _orderClient.GetOrderAsync(createdOrder.Id);

        // Then: Make sure we get a valid response
        retrievedOrder.Should().NotBeNull();
        retrievedOrder.Id.Should().Be(createdOrder.Id);
    }

    [DefaultRetryFact]
    public async Task GetOrderAsync_WithIncludeParameters_OrderIsRetrievedWithEmbeddedData() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        OrderResponse createdOrder = await _orderClient.CreateOrderAsync(orderRequest);

        // When: We attempt to retrieve the order and add the include parameters
        OrderResponse retrievedOrder = await _orderClient.GetOrderAsync(createdOrder.Id, embedPayments: true, embedShipments: true, embedRefunds: true);

        // Then: Make sure we get a valid response
        retrievedOrder.Should().NotBeNull();
        retrievedOrder.Id.Should().Be(createdOrder.Id);
        retrievedOrder.Embedded.Should().NotBeNull();
        retrievedOrder.Embedded!.Payments.Should().NotBeNull();
        retrievedOrder.Embedded.Shipments.Should().NotBeNull();
        retrievedOrder.Embedded.Refunds.Should().NotBeNull();
    }

    [DefaultRetryFact]
    public async Task UpdateOrderAsync_OrderIsUpdated_OrderIsUpdated() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        OrderResponse createdOrder = await _orderClient.CreateOrderAsync(orderRequest);

        // When: We attempt to update the order
        OrderUpdateRequest orderUpdateRequest = new() {
            OrderNumber = "1337",
            BillingAddress = createdOrder.BillingAddress
        };
        OrderResponse updatedOrder = await _orderClient.UpdateOrderAsync(createdOrder.Id, orderUpdateRequest);

        // Then: Make sure the order is updated
        updatedOrder.OrderNumber.Should().Be(orderUpdateRequest.OrderNumber);
    }

    [DefaultRetryFact(Skip = "Broken - Reported to Mollie: https://discordapp.com/channels/1037712581407817839/1180467187677401198/1180467187677401198")]
    public async Task UpdateOrderLinesAsync_WhenOrderLineIsUpdated_UpdatedPropertiesCanBeRetrieved() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        OrderResponse createdOrder = await _orderClient.CreateOrderAsync(orderRequest);

        // When: We update the order line
        OrderLineUpdateRequest updateRequest = new OrderLineUpdateRequest() {
            Name = "A fluffy bear"
        };
        OrderResponse updatedOrder = await _orderClient.UpdateOrderLinesAsync(createdOrder.Id, createdOrder.Lines.First().Id, updateRequest);

        // Then: The name of the order line should be updated
        updatedOrder.Lines.First().Name.Should().Be(updateRequest.Name);
    }

    [DefaultRetryFact]
    public async Task ManageOrderLinesAsync_AddOperation_OrderLineIsAdded() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        OrderResponse createdOrder = await _orderClient.CreateOrderAsync(orderRequest);

        // When: We use the manager order lines endpoint to add a order line
        ManageOrderLinesAddOperationData newOrderLineRequest = new ManageOrderLinesAddOperationData {
            Name = "LEGO Batman mobile",
            Type = OrderLineDetailsType.Physical,
            Category = VoucherCategory.Gift,
            Quantity = 1,
            UnitPrice = new Amount(Currency.EUR, 100.00m),
            TotalAmount = new Amount(Currency.EUR, 100.00m),
            VatRate = "21.00",
            VatAmount = new Amount(Currency.EUR, 17.36m),
            ImageUrl = "http://www.google.com/legobatmanimage",
            ProductUrl = "http://www.mollie.nl/legobatmanproduct",
            Metadata = "{\"is_lego_awesome\":\"fosho\"}",
        };
        ManageOrderLinesRequest manageOrderLinesRequest = new ManageOrderLinesRequest() {
            Operations = new List<ManageOrderLinesOperation> {
                new ManageOrderLinesAddOperation {
                    Data = newOrderLineRequest
                }
            }
        };
        OrderResponse updatedOrder = await _orderClient.ManageOrderLinesAsync(createdOrder.Id, manageOrderLinesRequest);

        // Then: The order line should be added
        updatedOrder.Lines.Should().HaveCount(2);
        var addedOrderLineRequest = updatedOrder.Lines.SingleOrDefault(line => line.Name == newOrderLineRequest.Name);
        addedOrderLineRequest.Should().NotBeNull();
        addedOrderLineRequest!.Type.Should().Be(newOrderLineRequest.Type);
        addedOrderLineRequest.Quantity.Should().Be(newOrderLineRequest.Quantity);
        addedOrderLineRequest.UnitPrice.Should().Be(newOrderLineRequest.UnitPrice);
        addedOrderLineRequest.TotalAmount.Should().Be(newOrderLineRequest.TotalAmount);
        addedOrderLineRequest.VatRate.Should().Be(newOrderLineRequest.VatRate);
        addedOrderLineRequest.VatAmount.Should().Be(newOrderLineRequest.VatAmount);
        var newMetaData = addedOrderLineRequest.Metadata!
            .Replace(Environment.NewLine, "")
            .Replace(" ", "");
        newMetaData.Should().Be(newOrderLineRequest.Metadata);
    }

    [DefaultRetryFact]
    public async Task ManageOrderLinesAsync_UpdateOperation_OrderLineIsUpdated() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        OrderResponse createdOrder = await _orderClient.CreateOrderAsync(orderRequest);

        // When: We use the manager order lines endpoint to update a order line
        ManageOrderLinesUpdateOperationData orderLineUpdateRequest = new ManageOrderLinesUpdateOperationData {
            Id = createdOrder.Lines.First().Id,
            Name = "LEGO Batman mobile",
            Quantity = 1,
            UnitPrice = new Amount(Currency.EUR, 100.00m),
            TotalAmount = new Amount(Currency.EUR, 90.00m),
            VatRate = "21.00",
            VatAmount = new Amount(Currency.EUR, 15.62m),
            ImageUrl = "http://www.google.com/legobatmanimage",
            ProductUrl = "http://www.mollie.nl/legobatmanproduct",
            Metadata = "{\"is_lego_awesome\":\"fosho\"}",
            Sku = "Sku",
            DiscountAmount = new Amount(Currency.EUR, 10m)
        };
        ManageOrderLinesRequest manageOrderLinesRequest = new ManageOrderLinesRequest() {
            Operations = new List<ManageOrderLinesOperation> {
                new ManageOrderLinesUpdateOperation {
                    Data = orderLineUpdateRequest
                }
            }
        };
        OrderResponse updatedOrder = await _orderClient.ManageOrderLinesAsync(createdOrder.Id, manageOrderLinesRequest);

        // Then: The order line should be updated
        updatedOrder.Lines.Should().HaveCount(1);
        var addedOrderLineRequest = updatedOrder.Lines.SingleOrDefault(line => line.Name == orderLineUpdateRequest.Name);
        addedOrderLineRequest.Should().NotBeNull();
        addedOrderLineRequest!.Quantity.Should().Be(orderLineUpdateRequest.Quantity);
        addedOrderLineRequest.UnitPrice.Should().Be(orderLineUpdateRequest.UnitPrice);
        addedOrderLineRequest.TotalAmount.Should().Be(orderLineUpdateRequest.TotalAmount);
        addedOrderLineRequest.VatRate.Should().Be(orderLineUpdateRequest.VatRate);
        addedOrderLineRequest.VatAmount.Should().Be(orderLineUpdateRequest.VatAmount);
        addedOrderLineRequest.Metadata!
            .Replace(Environment.NewLine, "")
            .Replace(" ", "")
            .Should().Be(orderLineUpdateRequest.Metadata);
    }

    [DefaultRetryFact]
    public async Task ManageOrderLinesAsync_CancelOperation_OrderLineIsCanceled() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        OrderResponse createdOrder = await _orderClient.CreateOrderAsync(orderRequest);

        // When: We use the manager order lines endpoint to cancel a order line
        ManagerOrderLinesCancelOperationData orderLineCancelRequest = new ManagerOrderLinesCancelOperationData {
            Id = createdOrder.Lines.First().Id,
            Quantity = 1
        };
        ManageOrderLinesRequest manageOrderLinesRequest = new ManageOrderLinesRequest() {
            Operations = new List<ManageOrderLinesOperation> {
                new ManageOrderLinesCancelOperation {
                    Data = orderLineCancelRequest
                }
            }
        };
        OrderResponse updatedOrder = await _orderClient.ManageOrderLinesAsync(createdOrder.Id, manageOrderLinesRequest);

        // Then: The order line should be canceled
        updatedOrder.Lines.Should().HaveCount(1);
        var updatedOrderLineRequest = updatedOrder.Lines.Single();
        updatedOrderLineRequest.Status.Should().Be(OrderStatus.Canceled);
    }

    [DefaultRetryFact]
    public async Task GetOrderListAsync_NoParameters_OrderListIsRetrieved() {
        // When: Retrieve payment list with default settings
        ListResponse<OrderResponse> response = await _orderClient.GetOrderListAsync();

        // Then
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }

    [DefaultRetryFact]
    public async Task GetOrderListAsync_WithMaximumNumberOfItems_MaximumNumberOfOrdersIsReturned() {
        // If: Number of orders requested is 5
        int numberOfOrders = 5;

        // When: Retrieve 5 orders
        ListResponse<OrderResponse> response = await _orderClient.GetOrderListAsync(null, numberOfOrders);

        // Then
        response.Items.Should().HaveCountLessThanOrEqualTo(numberOfOrders);
    }

    private OrderRequest CreateOrder() {
        return new OrderRequest() {
            Amount = new Amount(Currency.EUR, "100.00"),
            OrderNumber = "16738",
            Lines = new List<OrderLineRequest>() {
                new() {
                    Name = "A box of chocolates",
                    Type = OrderLineDetailsType.Physical,
                    Category = VoucherCategory.Gift,
                    Quantity = 1,
                    UnitPrice = new Amount(Currency.EUR, "100.00"),
                    TotalAmount = new Amount(Currency.EUR, "100.00"),
                    VatRate = "21.00",
                    VatAmount = new Amount(Currency.EUR, "17.36"),
                    ImageUrl = "http://www.google.com/",
                    ProductUrl = "http://www.mollie.nl/",
                    Metadata =  "{\"order_id\":\"4.40\"}",
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

    public void Dispose()
    {
        _orderClient?.Dispose();
    }
}
