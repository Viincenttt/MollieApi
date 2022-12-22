using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories;

namespace Mollie.Api.Framework.Factories {
    public class BalanceReportResponseFactory {
        public BalanceReportResponse Create(string reportGrouping) {
            switch (reportGrouping) {
                case ReportGrouping.StatusBalances:
                    return new StatusBalanceReportResponse();
                case ReportGrouping.TransactionCategories:
                    return new TransactionCategoriesReportResponse();
                default: 
                    return new BalanceReportResponse();
            }
        }
    }
}