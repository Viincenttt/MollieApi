using System;
using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific;

namespace Mollie.Api.Framework.Factories {
    internal class BalanceTransactionFactory : ITypeFactory<BalanceTransactionResponse> {
        public BalanceTransactionResponse Create(string? type) {
            if (string.IsNullOrEmpty(type)) {
                return Activator.CreateInstance<BalanceTransactionResponse>();
            }

            switch (type) {
                case BalanceTransactionContextType.Payment:
                case BalanceTransactionContextType.UnauthorizedDirectDebit:
                case BalanceTransactionContextType.FailedPayment:
                case BalanceTransactionContextType.ChargebackReversal:
                case BalanceTransactionContextType.ApplicationFee:
                case BalanceTransactionContextType.SplitPayment:
                    return Activator.CreateInstance<PaymentBalanceTransactionResponse>();
                case BalanceTransactionContextType.Capture:
                    return Activator.CreateInstance<CaptureBalanceTransactionResponse>();
                case BalanceTransactionContextType.Refund:
                case BalanceTransactionContextType.ReturnedRefund:
                case BalanceTransactionContextType.PlatformPaymentRefund:
                    return Activator.CreateInstance<RefundBalanceTransactionResponse>();
                case BalanceTransactionContextType.Chargeback:
                case BalanceTransactionContextType.PlatformPaymentChargeback:
                    return Activator.CreateInstance<ChargebackBalanceTransactionResponse>();
                case BalanceTransactionContextType.OutgoingTransfer:
                case BalanceTransactionContextType.CanceledOutgoingTransfer:
                case BalanceTransactionContextType.ReturnedTransfer:
                    return Activator.CreateInstance<SettlementBalanceTransactionResponse>();
                case BalanceTransactionContextType.InvoiceCompensation:
                    return Activator.CreateInstance<InvoiceBalanceTransactionResponse>();
                default:
                    return Activator.CreateInstance<BalanceTransactionResponse>();
            }
        }
    }
}
