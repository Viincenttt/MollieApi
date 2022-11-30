using System;
using System.Linq;
using System.Threading.Tasks;
using Mollie.Tests.Integration.Framework;
using NUnit.Framework;

namespace Mollie.Tests.Integration.Api {
    [TestFixture]
    public class BalanceTests : BaseMollieApiTestClass {
        [Test][RetryOnApiRateLimitFailure(NumberOfRetries)]
        public async Task GetPrimaryBalanceAsync_IsParsedCorrectly() {
            // When: We retrieve the primary balance from the Mollie API
            var result = await this._balanceClient.GetPrimaryBalanceAsync();

            // Then: Make sure we can parse the result
            Assert.IsNotNull(result);
            Assert.AreEqual("balance", result.Resource);
            Assert.IsNotNull(result.Currency);
            Assert.IsNotNull(result.Id);
            Assert.AreEqual("https://docs.mollie.com/reference/v2/balances-api/get-primary-balance", result.Links.Documentation.Href);
            Assert.AreEqual($"https://api.mollie.com/v2/balances/{result.Id}", result.Links.Self.Href);
            Assert.IsNotNull(result.Currency);
            Assert.IsNotNull(result.TransferFrequency);
            Assert.IsNotNull(result.AvailableAmount);
            Assert.IsNotNull(result.PendingAmount);
            Assert.IsNotNull(result.TransferThreshold);
        }
        
        [Test][RetryOnApiRateLimitFailure(NumberOfRetries)]
        public async Task GetBalanceAsync_IsParsedCorrectly() {
            // Given: We get a balance id from the list balances endpoint
            var balanceList = await this._balanceClient.ListBalancesAsync();
            if (balanceList.Count == 0) {
                Assert.Fail("No balance found to retrieve");
            }
            var firstBalance = balanceList.Items.First();

            // When: We retrieve a specific balance from the Mollie API
            var result = await this._balanceClient.GetBalanceAsync(firstBalance.Id);

            // Then: Make sure we can parse the result
            Assert.IsNotNull(result);
            Assert.AreEqual("balance", result.Resource);
            Assert.AreEqual(firstBalance.AvailableAmount, result.AvailableAmount);
            Assert.AreEqual(firstBalance.Id, result.Id);
            Assert.AreEqual("https://docs.mollie.com/reference/v2/balances-api/get-balance", result.Links.Documentation.Href);
            Assert.AreEqual($"https://api.mollie.com/v2/balances/{result.Id}", result.Links.Self.Href);
            Assert.AreEqual(firstBalance.Currency, result.Currency);
            Assert.AreEqual(firstBalance.TransferFrequency, result.TransferFrequency);
            Assert.AreEqual(firstBalance.AvailableAmount, result.AvailableAmount);
            Assert.AreEqual(firstBalance.PendingAmount, result.PendingAmount);
            Assert.AreEqual(firstBalance.TransferThreshold, result.TransferThreshold);
        }
        
        [Test][RetryOnApiRateLimitFailure(NumberOfRetries)]
        public async Task ListBalancesAsync_IsParsedCorrectly() {
            // When: We retrieve the list of balances
            var result = await this._balanceClient.ListBalancesAsync();

            // Then: Make sure we can parse the result
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Count, result.Items.Count);
        }
        
        [Test][RetryOnApiRateLimitFailure(NumberOfRetries)]
        public async Task GetBalanceReportAsync_IsParsedCorrectly() {
            // Given: We retrieve the primary balance
            var from = new DateTime(2022, 11, 1);
            var until = new DateTime(2022, 11, 30);
            var primaryBalance = await this._balanceClient.GetPrimaryBalanceAsync();
            
            // When: We retrieve the primary balance report
            var result = await this._balanceClient.GetBalanceReportAsync(
                balanceId: primaryBalance.Id,
                from: from, 
                until: until);

            // Then: Make sure we can parse the result
            Assert.IsNotNull(result);
            Assert.AreEqual("balance-report", result.Resource);
            Assert.AreEqual(primaryBalance.Id, result.BalanceId);
            Assert.AreEqual(from, result.From);
            Assert.AreEqual(until, result.Until);
        }
    }
}