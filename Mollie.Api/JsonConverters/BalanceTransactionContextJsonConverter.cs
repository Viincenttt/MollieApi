using System;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific;
using Newtonsoft.Json.Linq;

namespace Mollie.Api.JsonConverters {
    public class BalanceTransactionContextJsonConverter : JsonCreationConverter<BaseTransactionContext> {
        private readonly BalanceTransactionContextFactory _balanceTransactionContextFactory;

        public BalanceTransactionContextJsonConverter(BalanceTransactionContextFactory balanceTransactionContextFactory) {
            _balanceTransactionContextFactory = balanceTransactionContextFactory;
        }

        protected override BaseTransactionContext Create(Type objectType, JObject jObject) {
            string transactionType = this.GetTransactionType(jObject);

            return this._balanceTransactionContextFactory.Create(transactionType);
        }
        
        private string GetTransactionType(JObject jObject) {
            if (this.FieldExists("type", jObject)) {
                string typeValue = (string) jObject["type"];
                if (!string.IsNullOrEmpty(typeValue)) {
                    return typeValue;
                }
            }

            return null;
        }
    }
}