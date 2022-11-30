using Mollie.Api.Models.Balance.Response;
using Mollie.Api.Models.Balance.Response.Specific;

namespace Mollie.Api.Framework.Factories {
    public class BalanceReportResponseFactory {
        public BalanceReportResponse Create(string reportGrouping) {
            switch (reportGrouping) {
                case "status-balances":
                    return new StatusBalanceReportResponse();
                case "transaction-categories":
                    return new TransactionCategoriesReportResponse();
                default: 
                    return new BalanceReportResponse();
            }
        }
    }
}