using System;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Customer.Response;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.Mandate.Response;
using Mollie.Api.Models.Subscription.Request;
using Mollie.Api.Models.Subscription.Response;
using Mollie.Tests.Integration.Framework;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class SubscriptionTests : BaseMollieApiTestClass, IDisposable {
    private readonly ISubscriptionClient _subscriptionClient;
    private readonly ICustomerClient _customerClient;
    private readonly IMandateClient _mandateClient;

    public SubscriptionTests(
        ISubscriptionClient subscriptionClient,
        ICustomerClient customerClient,
        IMandateClient mandateClient) {
        _subscriptionClient = subscriptionClient;
        _customerClient = customerClient;
        _mandateClient = mandateClient;
    }

    [Fact]
    public async Task CanRetrieveSubscriptionList() {
        // Given
        string? customerId = await GetFirstCustomerWithValidMandate();

        // When: Retrieve subscription list with default settings
        if (customerId != null) {
            ListResponse<SubscriptionResponse> response = await _subscriptionClient.GetSubscriptionListAsync(customerId);

            // Then
            response.ShouldNotBeNull();
            response.Items.ShouldNotBeNull();
        }
    }

    [Fact]
    public async Task CanRetrieveAllSubscriptionList() {
        // Given

        // When: Retrieve subscription list with default settings
        ListResponse<SubscriptionResponse> response = await _subscriptionClient.GetAllSubscriptionList();

        // Then
        response.ShouldNotBeNull();
        response.Items.ShouldNotBeNull();
    }

    [Fact]
    public async Task ListSubscriptionsNeverReturnsMoreCustomersThenTheNumberOfRequestedSubscriptions() {
        // Given: Number of customers requested is 5
        string? customerId = await GetFirstCustomerWithValidMandate();
        if (customerId != null) {
            int numberOfSubscriptions = 5;

            // When: Retrieve 5 subscriptions
            ListResponse<SubscriptionResponse> response = await _subscriptionClient.GetSubscriptionListAsync(customerId, null, numberOfSubscriptions);

            // Then
            response.Items.Count.ShouldBeLessThanOrEqualTo(numberOfSubscriptions);
        }
    }

    [Fact]
    public async Task CanCreateSubscription() {
        // Given
        string? customerId = await GetFirstCustomerWithValidMandate();
        if (customerId != null) {
            var subscriptionRequest = new SubscriptionRequest {
                Amount = new Amount(Currency.EUR, "100.00"),
                Times = 5,
                Interval = "1 month",
                Description = $"Subscription {Guid.NewGuid()}", // Subscriptions must have a unique name
                WebhookUrl = "http://www.google.nl",
                StartDate = DateTime.Now.AddDays(1)
            };

            // When
            SubscriptionResponse subscriptionResponse = await _subscriptionClient.CreateSubscriptionAsync(customerId, subscriptionRequest);

            // Then
            subscriptionResponse.Amount.ShouldBe(subscriptionRequest.Amount);
            subscriptionResponse.Times.ShouldBe(subscriptionRequest.Times);
            subscriptionResponse.Interval.ShouldBe(subscriptionRequest.Interval);
            subscriptionResponse.Description.ShouldBe(subscriptionRequest.Description);
            subscriptionResponse.WebhookUrl.ShouldBe(subscriptionRequest.WebhookUrl);
            subscriptionResponse.StartDate.ShouldBe(subscriptionRequest.StartDate.Value.Date);
        }
    }

    [Fact]
    public async Task CanUpdateSubscription() {
        // Given
        var activeSubscription = await GetActiveSubscription();

        // When
        if (activeSubscription != null) {
            var customerId = activeSubscription.CustomerId;
            SubscriptionUpdateRequest request = new () {
                Description = $"Updated subscription {Guid.NewGuid()}"
            };
            SubscriptionResponse response = await _subscriptionClient.UpdateSubscriptionAsync(customerId, activeSubscription.Id, request);

            // Then
            response.Description.ShouldBe(request.Description);
        }
    }

    [Fact]
    public async Task CanCancelSubscription() {
        // Given: We have a customer with a mandate
        string? customerId = await GetFirstCustomerWithValidMandate();
        if (customerId != null) {
            ListResponse<SubscriptionResponse> subscriptions = await _subscriptionClient.GetSubscriptionListAsync(customerId);

            // When: That customer has a subscription that we can cancel
            SubscriptionResponse? subscriptionToCancel = subscriptions.Items
                .FirstOrDefault(s => s.Status != SubscriptionStatus.Canceled);
            if (subscriptionToCancel != null) {
                await _subscriptionClient.CancelSubscriptionAsync(customerId, subscriptionToCancel.Id);

                // Then: Make sure its canceled after one second
                await Task.Delay(TimeSpan.FromSeconds(1));
                SubscriptionResponse cancelledSubscription = await _subscriptionClient.GetSubscriptionAsync(customerId, subscriptionToCancel.Id);
                cancelledSubscription.Status.ShouldBe(SubscriptionStatus.Canceled);
            }
        }
    }

    [Fact]
    public async Task CanCreateSubscriptionWithMetaData() {
        // If: We create a subscription with meta data
        string json = "{\"order_id\":\"4.40\"}";
        string? customerId = await GetFirstCustomerWithValidMandate();
        if (customerId != null) {
            SubscriptionRequest subscriptionRequest = new SubscriptionRequest {
                Amount = new Amount(Currency.EUR, "100.00"),
                Times = 5,
                Interval = "1 month",
                Description = $"Subscription {Guid.NewGuid()}", // Subscriptions must have a unique name
                WebhookUrl = "http://www.google.nl",
                StartDate = DateTime.Now.AddDays(1),
                Metadata = json
            };

            // When We send the subscription request to Mollie
            SubscriptionResponse result = await _subscriptionClient.CreateSubscriptionAsync(customerId, subscriptionRequest);

            // Then: Make sure we get the same json result as metadata
            IsJsonResultEqual(result.Metadata, json).ShouldBeTrue();
        }
    }

    private async Task<string?> GetFirstCustomerWithValidMandate() {
        ListResponse<CustomerResponse> customers = await _customerClient.GetCustomerListAsync();

        foreach (CustomerResponse customer in customers.Items) {
            ListResponse<MandateResponse> mandates = await _mandateClient.GetMandateListAsync(customer.Id);
            if (mandates.Items.Any(x => x.Status == MandateStatus.Valid)) {
                return customer.Id;
            }
        }

        return null;
    }

    private async Task<SubscriptionResponse?> GetActiveSubscription() {
        ListResponse<CustomerResponse> customers = await _customerClient.GetCustomerListAsync();

        foreach (CustomerResponse customer in customers.Items.OrderByDescending(x => x.CreatedAt)) {
            ListResponse<SubscriptionResponse> subscriptions = await _subscriptionClient.GetSubscriptionListAsync(customer.Id);
            var activeSubscription = subscriptions.Items.FirstOrDefault(x => x.Status == SubscriptionStatus.Active);
            if (activeSubscription != null) {
                return activeSubscription;
            }
        }

        return null;
    }

    public void Dispose()
    {
        _subscriptionClient.Dispose();
        _customerClient.Dispose();
        _mandateClient.Dispose();
    }
}
