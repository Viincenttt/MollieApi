
using Mollie.Api.Client;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Response.Specific;
using Mollie.Api.Models.PaymentMethod;
using Mollie.Api.Models.Profile.Response;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using Mollie.Api.Models;
using Mollie.Api.Models.Profile.Request;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class ProfileTests : BaseMollieApiTestClass {
        [Test]
        public async Task GetCurrentProfileAsync_ReturnsCurrentProfile() {
            // Given

            // When: We retrieve the current profile from the mollie API
            ProfileResponse profileResponse = await this._profileClient.GetCurrentProfileAsync();

            // Then: Make sure we get a valid response
            Assert.IsNotNull(profileResponse);
            Assert.IsNotNull(profileResponse.Id);
            Assert.IsNotNull(profileResponse.Email);
            Assert.IsNotNull(profileResponse.Status);
        }

        [Test]
        public async Task EnablePaymentMethodAsync_WhenEnablingPaymentMethodForCurrentProfile_PaymentMethodIsReturned() {
            // Given

            // When: We enable a payment method for the current profile
            PaymentMethodResponse paymentMethodResponse = await this._profileClient.EnablePaymentMethodAsync(PaymentMethod.CreditCard);

            // Then: Make sure a payment method is returned
            Assert.IsNotNull(paymentMethodResponse);
            Assert.AreEqual(PaymentMethod.CreditCard, paymentMethodResponse.Id);
        }

        [Test]
        [Ignore("We can only test this in debug mode, because we need to retrieve a oauth access token to test this method")]
        public async Task EnablePaymentMethodAsync_WhenEnablingPaymentMethodForProfile_PaymentMethodIsReturned() {
            // Given: We retrieve the profile from the API
            ProfileClient profileClient = new ProfileClient("abcde"); // Set access token
            ListResponse<ProfileResponse> allProfiles = await profileClient.GetProfileListAsync();
            if (allProfiles.Items.Count == 0) {
                Assert.Inconclusive("No profiles found. Unable to continue test");
            }
            ProfileResponse profileToTestWith = allProfiles.Items.First();
            

            // When: We enable a payment method for the given profile
            PaymentMethodResponse paymentMethodResponse = await profileClient.EnablePaymentMethodAsync(profileToTestWith.Id, PaymentMethod.Ideal);

            // Then: Make sure a payment method is returned
            Assert.IsNotNull(paymentMethodResponse);
            Assert.AreEqual(PaymentMethod.Ideal, paymentMethodResponse.Id);
        }
        
        [Test]
        [Ignore("We can only test this in debug mode, because we need to retrieve a oauth access token to test this method")]
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
            Assert.IsNotNull(profileResponse);
        }

        [Test]
        [Ignore("Don't disable payment methods, other tests might break")]
        public async Task DisablePaymentMethodAsync_WhenDisablingPaymentMethodForCurrentProfile_NoErrorIsThrown() {
            // Given

            // When: We disable a payment method for the current profile
            await this._profileClient.DisablePaymentMethodAsync(PaymentMethod.CreditCard);

            // Then
        }

        /*
        [Test]
        public async Task EnableGiftCardIssuerAsync_WhenEnablingGiftCardIssuerForCurrentProfile_EnableGiftCardIssuerResponseIsReturned() {
            // Given
            const string issuerToEnable = "festivalcadeau";

            // When: We enable a payment method for the current profile
            EnableGiftCardIssuerResponse giftcardIssuerResponse = await this._profileClient.EnableGiftCardIssuerAsync(issuerToEnable);

            // Then: Make sure a payment method is returned
            Assert.IsNotNull(giftcardIssuerResponse);
            Assert.AreEqual(issuerToEnable, giftcardIssuerResponse.Id);
        }*/

        [Test]
        public async Task DisableGiftCardIssuerAsync_WhenDisablingGiftCardIssuerForCurrentProfile_NoErrorIsThrown() {
            // Given

            // When: We disable a issuer method for the current profile
            await this._profileClient.DisableGiftCardIssuerAsync("festivalcadeau");

            // Then
        }
    }
}
