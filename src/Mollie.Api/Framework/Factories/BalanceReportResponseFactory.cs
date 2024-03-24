using System;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.StatusBalance;
using Mollie.Api.Models.Balance.Response.BalanceReport.Specific.TransactionCategories;

namespace Mollie.Api.Framework.Factories {
    public class BalanceReportResponseFactory {
        public BalanceReportResponse Create(string reportGrouping) {
            switch (reportGrouping) {
                case ReportGrouping.StatusBalances:
                    return Activator.CreateInstance<StatusBalanceReportResponse>();
                case ReportGrouping.TransactionCategories:
                    return Activator.CreateInstance<TransactionCategoriesReportResponse>();
                default: 
                    return Activator.CreateInstance<BalanceReportResponse>();
            }
        }
    }
}