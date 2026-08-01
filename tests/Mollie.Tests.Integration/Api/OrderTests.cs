using System;
#pragma warning disable CS0618
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Order;
using Mollie.Api.Models.Order.Request;
using Mollie.Api.Models.Order.Request.ManageOrderLines;
using Mollie.Api.Models.Order.Request.PaymentSpecificParameters;
using Mollie.Api.Models.Order.Response;
using Mollie.Api.Models.Payment;
using Mollie.Tests.Integration.Framework;
using Xunit;
using SortDirection = Mollie.Api.Models.SortDirection;

namespace Mollie.Tests.Integration.Api;

public class OrderTests : BaseMollieApiTestClass, IDisposable {
    private readonly IOrderClient _orderClient;

    public OrderTests(IOrderClient orderClient) {
        _orderClient = orderClient;
    }

    [Fact]
    public async Task GetOrderListAsync_WithoutSortOrder_ReturnsOrdersInDescendingOrder()
    {
        // Act
        var result = await _orderClient.GetOrderListAsync();
        var orders = result.Data!;

        // Assert
        result.Success.ShouldBeTrue();
        if (orders.Items.Any())
        {
            orders.Items.Select(x => x.CreatedAt).ShouldBeInOrder(Shouldly.SortDirection.Descending);
        }
    }

    [Fact]
    public async Task GetOrderListAsync_InDescendingOrder_ReturnsOrdersInDescendingOrder()
    {
        // Act
        var result = await _orderClient.GetOrderListAsync(sort: SortDirection.Desc);
        var orders = result.Data!;

        // Assert
        result.Success.ShouldBeTrue();
        if (orders.Items.Any())
        {
            orders.Items.Select(x => x.CreatedAt).ShouldBeInOrder(Shouldly.SortDirection.Descending);
        }
    }

    [Fact]
    public async Task GetOrderListAsync_InAscendingOrder_ReturnsOrdersInAscendingOrder()
    {
        // Act
        var result = await _orderClient.GetOrderListAsync(sort: SortDirection.Asc);
        var orders = result.Data!;

        // Assert
        result.Success.ShouldBeTrue();
        if (orders.Items.Any())
        {
            orders.Items.Select(x => x.CreatedAt).ShouldBeInOrder(Shouldly.SortDirection.Ascending);
        }
    }

    [Fact]
    public async Task CreateOrderAsync_OrderWithRequiredFields_OrderIsCreated() {
        // If: we create a order request with only the required parameters
        OrderRequest orderRequest = CreateOrder();

        // When: We send the order request to Mollie
        var result = await _orderClient.CreateOrderAsync(orderRequest);
        var order = result.Data!;

        // Then: Make sure we get a valid response
        result.Success.ShouldBeTrue();
        order.ShouldNotBeNull();
        order.Amount.ShouldBe(orderRequest.Amount);
        order.OrderNumber.ShouldBe(orderRequest.OrderNumber);
        order.Lines.Count().ShouldBe(orderRequest.Lines.Count());
        order.Links.ShouldNotBeNull();
        OrderLineRequest orderLineRequest = orderRequest.Lines.First();
        OrderLineResponse orderResponseLine = order.Lines.First();
        orderResponseLine.Type.ShouldBe(orderLineRequest.Type);
        orderResponseLine.Links.ImageUrl!.Href.ShouldBe(orderLineRequest.ImageUrl);
        orderResponseLine.Links.ProductUrl!.Href.ShouldBe(orderLineRequest.ProductUrl);
        var expectedMetadataString = order.Lines.First().Metadata;
        orderResponseLine.Metadata.ShouldBe(expectedMetadataString);
    }

    [Fact]
    public async Task CreateOrderAsync_OrderWithExtendedFields_OrderIsCreated() {
        // If: we create a order request
        OrderRequest orderRequest = CreateOrder();
        orderRequest.ConsumerDateOfBirth = new DateTime(1980, 1, 1);
        orderRequest.ExpiresAt = DateTime.Now.AddDays(2);

        // When: We send the order request to Mollie
        var result = await _orderClient.CreateOrderAsync(orderRequest);
        var order = result.Data!;

        // Then: Make sure we get a valid response
        result.Success.ShouldBeTrue();
        order.ShouldNotBeNull();
        order.ConsumerDateOfBirth.ShouldBe(orderRequest.ConsumerDateOfBirth);
        order.ExpiresAt!.Value.Date.ShouldBe(orderRequest.ExpiresAt.Value.Date);
    }

    [Fact]
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
        var result = await _orderClient.CreateOrderAsync(orderRequest);
        var order = result.Data!;

        // Then: Make sure we get a valid response
        result.Success.ShouldBeTrue();
        order.ShouldNotBeNull();
        order.Amount.ShouldBe(orderRequest.Amount);
        order.OrderNumber.ShouldBe(orderRequest.OrderNumber);
        order.Lines.Count().ShouldBe(orderRequest.Lines.Count());
        order.Links.ShouldNotBeNull();
        OrderLineRequest orderLineRequest = orderRequest.Lines.First();
        OrderLineResponse orderResponseLine = order.Lines.First();
        orderResponseLine.Type.ShouldBe(orderLineRequest.Type);
        orderResponseLine.Links.ImageUrl!.Href.ShouldBe(orderLineRequest.ImageUrl);
        orderResponseLine.Links.ProductUrl!.Href.ShouldBe(orderLineRequest.ProductUrl);
        var expectedMetadataString = order.Lines.First().Metadata;
        orderResponseLine.Metadata.ShouldBe(expectedMetadataString);
    }

    [Fact]
    public async Task CreateOrderAsync_WithMultiplePaymentMethods_OrderIsCreated() {
        // When: we create a order request and specify multiple payment methods
        OrderRequest orderRequest = CreateOrder();
        orderRequest.Methods = new List<string>() {
            PaymentMethod.Ideal,
            PaymentMethod.CreditCard,
            PaymentMethod.DirectDebit
        };

        // When: We send the order request to Mollie
        var result = await _orderClient.CreateOrderAsync(orderRequest);
        var order = result.Data!;

        // Then: Make sure we get a valid response
        result.Success.ShouldBeTrue();
        order.ShouldNotBeNull();
        order.Amount.ShouldBe(orderRequest.Amount);
        order.OrderNumber.ShouldBe(orderRequest.OrderNumber);
    }

    [Fact]
    public async Task CreateOrderAsync_WithSinglePaymentMethod_OrderIsCreated() {
        // When: we create a order request and specify a single payment method
        OrderRequest orderRequest = CreateOrder();
        orderRequest.Method = PaymentMethod.CreditCard;

        // When: We send the order request to Mollie
        var result = await _orderClient.CreateOrderAsync(orderRequest);
        var order = result.Data!;

        // Then: Make sure we get a valid response
        orderRequest.Method.ShouldBe(PaymentMethod.CreditCard);
        orderRequest.Methods!.First().ShouldBe(PaymentMethod.CreditCard);
        result.Success.ShouldBeTrue();
        order.ShouldNotBeNull();
        order.Amount.ShouldBe(orderRequest.Amount);
        order.OrderNumber.ShouldBe(orderRequest.OrderNumber);
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

    [Theory]
    [MemberData(nameof(PaymentSpecificParameters))]
    public async Task CreateOrderAsync_WithPaymentSpecificParameters_OrderIsCreated(
        OrderPaymentParameters paymentSpecificParameters) {

        // If: we create a order request with payment specific parameters
        OrderRequest orderRequest = CreateOrder();
        orderRequest.BillingAddress!.Country = "DE"; // Billie only works in Germany
        orderRequest.BillingAddress.OrganizationName = "Mollie"; // Billie requires a organization name
        orderRequest.Payment = paymentSpecificParameters;

        // When: We send the order request to Mollie
        var result = await _orderClient.CreateOrderAsync(orderRequest);
        var order = result.Data!;

        // Then: Make sure we get a valid response
        result.Success.ShouldBeTrue();
        order.ShouldNotBeNull();
        order.Amount.ShouldBe(orderRequest.Amount);
        order.OrderNumber.ShouldBe(orderRequest.OrderNumber);
    }

    [Fact]
    public async Task GetOrderAsync_OrderIsCreated_OrderCanBeRetrieved() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        var createResult = await _orderClient.CreateOrderAsync(orderRequest);
        var createdOrder = createResult.Data!;

        // When: We attempt to retrieve the order
        var getResult = await _orderClient.GetOrderAsync(createdOrder.Id);
        var retrievedOrder = getResult.Data!;

        // Then: Make sure we get a valid response
        createResult.Success.ShouldBeTrue();
        getResult.Success.ShouldBeTrue();
        retrievedOrder.ShouldNotBeNull();
        retrievedOrder.Id.ShouldBe(createdOrder.Id);
    }

    [Fact]
    public async Task GetOrderAsync_WithUrlObject_OrderCanBeRetrieved() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        var createResult = await _orderClient.CreateOrderAsync(orderRequest);
        var createdOrder = createResult.Data!;

        // When: We attempt to retrieve the order
        var getResult = await _orderClient.GetOrderAsync(createdOrder.Links.Self);
        var retrievedOrder = getResult.Data!;

        // Then: Make sure we get a valid response
        createResult.Success.ShouldBeTrue();
        getResult.Success.ShouldBeTrue();
        retrievedOrder.ShouldNotBeNull();
        retrievedOrder.Id.ShouldBe(createdOrder.Id);
    }

    [Fact]
    public async Task GetOrderAsync_WithIncludeParameters_OrderIsRetrievedWithEmbeddedData() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        var createResult = await _orderClient.CreateOrderAsync(orderRequest);
        var createdOrder = createResult.Data!;

        // When: We attempt to retrieve the order and add the include parameters
        var getResult = await _orderClient.GetOrderAsync(createdOrder.Id, embedPayments: true, embedShipments: true, embedRefunds: true);
        var retrievedOrder = getResult.Data!;

        // Then: Make sure we get a valid response
        createResult.Success.ShouldBeTrue();
        getResult.Success.ShouldBeTrue();
        retrievedOrder.ShouldNotBeNull();
        retrievedOrder.Id.ShouldBe(createdOrder.Id);
        retrievedOrder.Embedded.ShouldNotBeNull();
        retrievedOrder.Embedded!.Payments.ShouldNotBeNull();
        retrievedOrder.Embedded.Shipments.ShouldNotBeNull();
        retrievedOrder.Embedded.Refunds.ShouldNotBeNull();
    }

    [Fact]
    public async Task UpdateOrderAsync_OrderIsUpdated_OrderIsUpdated() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        var createResult = await _orderClient.CreateOrderAsync(orderRequest);
        var createdOrder = createResult.Data!;

        // When: We attempt to update the order
        OrderUpdateRequest orderUpdateRequest = new() {
            OrderNumber = "1337",
            BillingAddress = createdOrder.BillingAddress
        };
        var updateResult = await _orderClient.UpdateOrderAsync(createdOrder.Id, orderUpdateRequest);
        var updatedOrder = updateResult.Data!;

        // Then: Make sure the order is updated
        createResult.Success.ShouldBeTrue();
        updateResult.Success.ShouldBeTrue();
        updatedOrder.OrderNumber.ShouldBe(orderUpdateRequest.OrderNumber);
    }

    [Fact(Skip = "Broken - Reported to Mollie: https://discordapp.com/channels/1037712581407817839/1180467187677401198/1180467187677401198")]
    public async Task UpdateOrderLinesAsync_WhenOrderLineIsUpdated_UpdatedPropertiesCanBeRetrieved() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        var createResult = await _orderClient.CreateOrderAsync(orderRequest);
        var createdOrder = createResult.Data!;

        // When: We update the order line
        OrderLineUpdateRequest updateRequest = new() {
            Name = "A fluffy bear"
        };
        var updateResult = await _orderClient.UpdateOrderLinesAsync(createdOrder.Id, createdOrder.Lines.First().Id, updateRequest);
        var updatedOrder = updateResult.Data!;

        // Then: The name of the order line should be updated
        createResult.Success.ShouldBeTrue();
        updateResult.Success.ShouldBeTrue();
        updatedOrder.Lines.First().Name.ShouldBe(updateRequest.Name);
    }

    [Fact]
    public async Task ManageOrderLinesAsync_AddOperation_OrderLineIsAdded() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        var createResult = await _orderClient.CreateOrderAsync(orderRequest);
        var createdOrder = createResult.Data!;

        // When: We use the manager order lines endpoint to add a order line
        ManageOrderLinesAddOperationData newOrderLineRequest = new() {
            Name = "LEGO Batman mobile",
            Type = OrderLineDetailsType.Physical,
            Category = VoucherCategory.Gift,
            Quantity = 1,
            UnitPrice = new Amount(Currency.EUR, 100.00m),
            TotalAmount = new Amount(Currency.EUR, 100.00m),
            VatRate = 21.00m,
            VatAmount = new Amount(Currency.EUR, 17.36m),
            ImageUrl = "http://www.google.com/legobatmanimage",
            ProductUrl = "http://www.mollie.nl/legobatmanproduct",
            Metadata = "{\"is_lego_awesome\":\"fosho\"}",
        };
        ManageOrderLinesRequest manageOrderLinesRequest = new() {
            Operations = new List<ManageOrderLinesOperation> {
                new ManageOrderLinesAddOperation {
                    Data = newOrderLineRequest
                }
            }
        };
        var manageResult = await _orderClient.ManageOrderLinesAsync(createdOrder.Id, manageOrderLinesRequest);
        var updatedOrder = manageResult.Data!;

        // Then: The order line should be added
        createResult.Success.ShouldBeTrue();
        manageResult.Success.ShouldBeTrue();
        updatedOrder.Lines.Count().ShouldBe(2);
        var addedOrderLineRequest = updatedOrder.Lines.SingleOrDefault(line => line.Name == newOrderLineRequest.Name);
        addedOrderLineRequest.ShouldNotBeNull();
        addedOrderLineRequest!.Type.ShouldBe(newOrderLineRequest.Type);
        addedOrderLineRequest.Quantity.ShouldBe(newOrderLineRequest.Quantity);
        addedOrderLineRequest.UnitPrice.ShouldBe(newOrderLineRequest.UnitPrice);
        addedOrderLineRequest.TotalAmount.ShouldBe(newOrderLineRequest.TotalAmount);
        addedOrderLineRequest.VatRate.ShouldBe(newOrderLineRequest.VatRate);
        addedOrderLineRequest.VatAmount.ShouldBe(newOrderLineRequest.VatAmount);
        var newMetaData = addedOrderLineRequest.Metadata!
            .Replace(Environment.NewLine, "")
            .Replace(" ", "");
        newMetaData.ShouldBe(newOrderLineRequest.Metadata);
    }

    [Fact]
    public async Task ManageOrderLinesAsync_UpdateOperation_OrderLineIsUpdated() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        var createResult = await _orderClient.CreateOrderAsync(orderRequest);
        var createdOrder = createResult.Data!;

        // When: We use the manager order lines endpoint to update a order line
        ManageOrderLinesUpdateOperationData orderLineUpdateRequest = new() {
            Id = createdOrder.Lines.First().Id,
            Name = "LEGO Batman mobile",
            Quantity = 1,
            UnitPrice = new Amount(Currency.EUR, 100.00m),
            TotalAmount = new Amount(Currency.EUR, 90.00m),
            VatRate = 21.00m,
            VatAmount = new Amount(Currency.EUR, 15.62m),
            ImageUrl = "http://www.google.com/legobatmanimage",
            ProductUrl = "http://www.mollie.nl/legobatmanproduct",
            Metadata = "{\"is_lego_awesome\":\"fosho\"}",
            Sku = "Sku",
            DiscountAmount = new Amount(Currency.EUR, 10m)
        };
        ManageOrderLinesRequest manageOrderLinesRequest = new() {
            Operations = new List<ManageOrderLinesOperation> {
                new ManageOrderLinesUpdateOperation {
                    Data = orderLineUpdateRequest
                }
            }
        };
        var manageResult = await _orderClient.ManageOrderLinesAsync(createdOrder.Id, manageOrderLinesRequest);
        var updatedOrder = manageResult.Data!;

        // Then: The order line should be updated
        createResult.Success.ShouldBeTrue();
        manageResult.Success.ShouldBeTrue();
        updatedOrder.Lines.Count().ShouldBe(1);
        var addedOrderLineRequest = updatedOrder.Lines.SingleOrDefault(line => line.Name == orderLineUpdateRequest.Name);
        addedOrderLineRequest.ShouldNotBeNull();
        addedOrderLineRequest!.Quantity.ShouldBe(orderLineUpdateRequest.Quantity.Value);
        addedOrderLineRequest.UnitPrice.ShouldBe(orderLineUpdateRequest.UnitPrice);
        addedOrderLineRequest.TotalAmount.ShouldBe(orderLineUpdateRequest.TotalAmount);
        addedOrderLineRequest.VatRate.ShouldBe(orderLineUpdateRequest.VatRate.Value);
        addedOrderLineRequest.VatAmount.ShouldBe(orderLineUpdateRequest.VatAmount);
        addedOrderLineRequest.Metadata!
            .Replace(Environment.NewLine, "")
            .Replace(" ", "")
            .ShouldBe(orderLineUpdateRequest.Metadata);
    }

    [Fact]
    public async Task ManageOrderLinesAsync_CancelOperation_OrderLineIsCanceled() {
        // If: we create a new order
        OrderRequest orderRequest = CreateOrder();
        var createResult = await _orderClient.CreateOrderAsync(orderRequest);
        var createdOrder = createResult.Data!;

        // When: We use the manager order lines endpoint to cancel a order line
        ManagerOrderLinesCancelOperationData orderLineCancelRequest = new() {
            Id = createdOrder.Lines.First().Id,
            Quantity = 1
        };
        ManageOrderLinesRequest manageOrderLinesRequest = new() {
            Operations = new List<ManageOrderLinesOperation> {
                new ManageOrderLinesCancelOperation {
                    Data = orderLineCancelRequest
                }
            }
        };
        var manageResult = await _orderClient.ManageOrderLinesAsync(createdOrder.Id, manageOrderLinesRequest);
        var updatedOrder = manageResult.Data!;

        // Then: The order line should be canceled
        createResult.Success.ShouldBeTrue();
        manageResult.Success.ShouldBeTrue();
        updatedOrder.Lines.Count().ShouldBe(1);
        var updatedOrderLineRequest = updatedOrder.Lines.Single();
        updatedOrderLineRequest.Status.ShouldBe(OrderStatus.Canceled);
    }

    [Fact]
    public async Task GetOrderListAsync_NoParameters_OrderListIsRetrieved() {
        // When: Retrieve orders list with default settings
        var result = await _orderClient.GetOrderListAsync();
        var response = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetOrderListAsync_WithMaximumNumberOfItems_MaximumNumberOfOrdersIsReturned() {
        // If: Number of orders requested is 5
        int numberOfOrders = 5;

        // When: Retrieve 5 orders
        var result = await _orderClient.GetOrderListAsync(null, numberOfOrders);
        var response = result.Data!;

        // Then
        result.Success.ShouldBeTrue();
        response.Items.Count.ShouldBeLessThanOrEqualTo(numberOfOrders);
    }

    private OrderRequest CreateOrder() {
        return new OrderRequest() {
            Amount = new Amount(Currency.EUR, 100.00m),
            OrderNumber = "16738",
            Lines = new List<OrderLineRequest> {
                new() {
                    Name = "A box of chocolates",
                    Type = OrderLineDetailsType.Physical,
                    Category = VoucherCategory.Gift,
                    Quantity = 1,
                    UnitPrice = new Amount(Currency.EUR, 100.00m),
                    TotalAmount = new Amount(Currency.EUR, 100.00m),
                    VatRate = 21.00m,
                    VatAmount = new Amount(Currency.EUR, 17.36m),
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
