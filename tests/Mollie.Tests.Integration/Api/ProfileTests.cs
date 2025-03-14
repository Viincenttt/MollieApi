﻿
using System;
using Mollie.Api.Client;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Profile.Response;
using Mollie.Tests.Integration.Framework;
using System.Linq;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.List.Response;
using Mollie.Api.Models.PaymentMethod.Response;
using Mollie.Api.Models.Profile.Request;
using Xunit;

namespace Mollie.Tests.Integration.Api;

public class ProfileTests : BaseMollieApiTestClass, IDisposable {
    private readonly IProfileClient _profileClient;

    public ProfileTests(IProfileClient profileClient) {
        _profileClient = profileClient;
    }

    [Fact]
    public async Task GetCurrentProfileAsync_ReturnsCurrentProfile() {
        // Given

        // When: We retrieve the current profile from the mollie API
        ProfileResponse profileResponse = await _profileClient.GetCurrentProfileAsync();

        // Then: Make sure we get a valid response
        profileResponse.ShouldNotBeNull();
        profileResponse.Id.ShouldNotBeNullOrEmpty();
        profileResponse.Email.ShouldNotBeNullOrEmpty();
        profileResponse.Status.ShouldNotBeNullOrEmpty();
    }

    [Fact]
    public async Task EnablePaymentMethodAsync_WhenEnablingPaymentMethodForCurrentProfile_PaymentMethodIsReturned() {
        // Given

        // When: We enable a payment method for the current profile
        PaymentMethodResponse paymentMethodResponse = await _profileClient.EnablePaymentMethodAsync(PaymentMethod.CreditCard);

        // Then: Make sure a payment method is returned
        paymentMethodResponse.ShouldNotBeNull();
        paymentMethodResponse.Id.ShouldBe(PaymentMethod.CreditCard);
    }

    [Fact(Skip = "We can only test this in debug mode, because we need to retrieve a oauth access token to test this method")]
    public async Task EnablePaymentMethodAsync_WhenEnablingPaymentMethodForProfile_PaymentMethodIsReturned() {
        // Given: We retrieve the profile from the API
        ProfileClient profileClient = new ProfileClient("abcde"); // Set access token
        ListResponse<ProfileResponse> allProfiles = await profileClient.GetProfileListAsync();
        if (allProfiles.Items.Count > 0) {
            ProfileResponse profileToTestWith = allProfiles.Items.First();

            // When: We enable a payment method for the given profile
            PaymentMethodResponse paymentMethodResponse = await profileClient.EnablePaymentMethodAsync(profileToTestWith.Id, PaymentMethod.Ideal);

            // Then: Make sure a payment method is returned
            paymentMethodResponse.ShouldNotBeNull();
            paymentMethodResponse.Id.ShouldBeNullOrEmpty();
        }
    }

    [Fact(Skip = "We can only test this in debug mode, because we need to retrieve a oauth access token to test this method")]
    public async Task CreateProfileAsync_WithDefaultParameters_CreatesProfile() {
        // Given
        ProfileRequest profileRequest = new ProfileRequest {
            Email = "test@test.com",
            Mode = Mode.Test,
            Name = "testuser",
            BusinessCategory = "PET_SHOPS",
            Website = "http://github.com",
            Phone = "+31600000000"
        };
        ProfileClient profileClient = new ProfileClient("accesstoken"); // Set access token

        // When: We create a new profile
        ProfileResponse profileResponse = await profileClient.CreateProfileAsync(profileRequest);

        // Then: Make sure the profile that is created matched the profile request
        profileResponse.ShouldNotBeNull();
    }

    [Fact(Skip = "Don't disable payment methods, other tests might break")]
    public async Task DisablePaymentMethodAsync_WhenDisablingPaymentMethodForCurrentProfile_NoErrorIsThrown() {
        // Given

        // When: We disable a payment method for the current profile
        await _profileClient.DisablePaymentMethodAsync(PaymentMethod.CreditCard);

        // Then
    }

    [Fact]
    public async Task DisableGiftCardIssuerAsync_WhenDisablingGiftCardIssuerForCurrentProfile_NoErrorIsThrown() {
        // Given

        // When: We disable a issuer method for the current profile
        await _profileClient.DisableGiftCardIssuerAsync("festivalcadeau");

        // Then
    }

    public void Dispose()
    {
        _profileClient?.Dispose();
    }
}
