using System;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Balance.Response.BalanceReport;
using Newtonsoft.Json.Linq;

namespace Mollie.Api.JsonConverters {
    internal class BalanceReportResponseJsonConverter : JsonCreationConverter<BalanceReportResponse> {
        private readonly BalanceReportResponseFactory _balanceReportResponseFactory;

        public BalanceReportResponseJsonConverter(BalanceReportResponseFactory balanceReportResponseFactory) {
            _balanceReportResponseFactory = balanceReportResponseFactory;
        }

        protected override BalanceReportResponse Create(Type objectType, JObject jObject) {
            string reportGrouping = this.GetBalanceReportGrouping(jObject);

            return this._balanceReportResponseFactory.Create(reportGrouping);
        }
        
        private string GetBalanceReportGrouping(JObject jObject) {
            if (this.FieldExists("grouping", jObject)) {
                string reportGroupingValue = (string) jObject["grouping"];
                if (!string.IsNullOrEmpty(reportGroupingValue)) {
                    return reportGroupingValue;
                }
            }

            return null;
        }
    }
}