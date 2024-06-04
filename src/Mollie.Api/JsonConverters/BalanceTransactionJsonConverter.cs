using System;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Newtonsoft.Json.Linq;

namespace Mollie.Api.JsonConverters {
    internal class BalanceTransactionJsonConverter : JsonCreationConverter<BalanceTransactionResponse> {
        private readonly BalanceTransactionFactory _balanceTransactionFactory;

        public BalanceTransactionJsonConverter(BalanceTransactionFactory balanceTransactionFactory) {
            _balanceTransactionFactory = balanceTransactionFactory;
        }

        protected override BalanceTransactionResponse Create(Type objectType, JObject jObject) {
            string? transactionType = GetTransactionType(jObject);

            return _balanceTransactionFactory.Create(transactionType);
        }

        private string? GetTransactionType(JObject jObject) {
            if (FieldExists("type", jObject)) {
                string? typeValue = (string?)jObject["type"];
                if (!string.IsNullOrEmpty(typeValue)) {
                    return typeValue;
                }
            }

            return null;
        }
    }
}
