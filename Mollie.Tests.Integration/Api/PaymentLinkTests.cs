﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Extensions;
using Mollie.Api.Models;
using Mollie.Api.Models.List;
using Mollie.Api.Models.PaymentLink.Request;
using Mollie.Api.Models.PaymentLink.Response;
using Mollie.Tests.Integration.Framework;

namespace Mollie.Tests.Integration.Api; 

public class PaymentLinkTests : BaseMollieApiTestClass {
    private readonly IPaymentLinkClient _paymentLinkClient;

    public PaymentLinkTests() {
        _paymentLinkClient = new PaymentLinkClient(this.ApiKey);
    }
        
    [DefaultRetryFact]
    public async Task CanRetrievePaymentlinkList() {
        // When: Retrieve payment list with default settings
        ListResponse<PaymentLinkResponse> response = await this._paymentLinkClient.GetPaymentLinkListAsync();

        // Then
        response.Should().NotBeNull();
        response.Items.Should().NotBeNull();
    }

    [DefaultRetryFact]
    public async Task CanCreatePaymentLinkAndRetrieveIt() {
        // Given: We create a new payment 
        PaymentLinkRequest paymentLinkRequest = new PaymentLinkRequest() {
            Description = "Test",
            Amount = new Amount(Currency.EUR, 50),
            WebhookUrl = this.DefaultWebhookUrl,
            RedirectUrl = this.DefaultRedirectUrl,
            ExpiresAt = DateTime.Now.AddDays(1)
        };
        var createdPaymentLinkResponse = await this._paymentLinkClient.CreatePaymentLinkAsync(paymentLinkRequest);

        // When: We retrieve it
        var retrievePaymentLinkResponse = await this._paymentLinkClient.GetPaymentLinkAsync(createdPaymentLinkResponse.Id);

        // Then: We expect a list with a single ideal payment     
        var verifyPaymentLinkResponse = new Action<PaymentLinkResponse>(response => {
            var expiresAtWithoutMs = paymentLinkRequest.ExpiresAt.Value.Truncate(TimeSpan.FromSeconds(1));

            response.Amount.Should().Be(paymentLinkRequest.Amount);
            response.ExpiresAt.Should().Be(expiresAtWithoutMs);
            response.Description.Should().Be(paymentLinkRequest.Description);
            response.RedirectUrl.Should().Be(paymentLinkRequest.RedirectUrl);
        });

        verifyPaymentLinkResponse(createdPaymentLinkResponse);
        verifyPaymentLinkResponse(retrievePaymentLinkResponse);
    }
}