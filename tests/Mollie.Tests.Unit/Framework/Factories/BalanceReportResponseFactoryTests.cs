using System;
using Shouldly;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories;
using Xunit;

namespace Mollie.Tests.Unit.Framework.Factories {
    public class BalanceReportResponseFactoryTests {
        [Theory]
        [InlineData(ReportGrouping.StatusBalances, typeof(StatusBalanceReportResponse))]
        [InlineData(ReportGrouping.TransactionCategories, typeof(TransactionCategoriesReportResponse))]
        [InlineData("unknown", typeof(BalanceReportResponse))]
        public void Create_CreatesExpectedType(string grouping, Type expectedType) {
            // Given
            var factory = new BalanceReportResponseFactory();

            // When
            var result = factory.Create(grouping);

            // Then
            result.ShouldBeOfType(expectedType);
        }
    }
}
