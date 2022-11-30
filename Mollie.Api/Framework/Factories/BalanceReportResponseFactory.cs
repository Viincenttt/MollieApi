using Mollie.Api.Models.Balance.Response;
using Mollie.Api.Models.Balance.Response.Specific;

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