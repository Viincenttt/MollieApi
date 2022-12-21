using System;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories;
using NUnit.Framework;

namespace Mollie.Tests.Unit.Framework.Factories {
    [TestFixture]
    public class BalanceReportResponseFactoryTests {
        [TestCase(ReportGrouping.StatusBalances, typeof(StatusBalanceReportResponse))]
        [TestCase(ReportGrouping.TransactionCategories, typeof(TransactionCategoriesReportResponse))]
        [TestCase("unknown grouping", typeof(BalanceReportResponse))]
        public void Create_CreatesExpectedType(string grouping, Type expectedType) {
            // Given
            var factory = new BalanceReportResponseFactory();

            // When
            var result = factory.Create(grouping);

            // Then
            Assert.AreEqual(expectedType, result.GetType());
        }
    }
}