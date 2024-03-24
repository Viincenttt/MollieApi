using System;
using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific;

namespace Mollie.Api.Framework.Factories {
    internal class BalanceTransactionFactory {
        public BalanceTransaction Create(string? type) {
            if (string.IsNullOrEmpty(type)) {
                return Activator.CreateInstance<BalanceTransaction>();
            }
            
            switch (type) {
                case BalanceTransactionContextType.Payment:
                case BalanceTransactionContextType.UnauthorizedDirectDebit:
                case BalanceTransactionContextType.FailedPayment:
                case BalanceTransactionContextType.ChargebackReversal:
                case BalanceTransactionContextType.ApplicationFee:
                case BalanceTransactionContextType.SplitPayment:
                    return Activator.CreateInstance<PaymentBalanceTransaction>();
                case BalanceTransactionContextType.Capture:
                    return Activator.CreateInstance<CaptureBalanceTransaction>();
                case BalanceTransactionContextType.Refund:
                case BalanceTransactionContextType.ReturnedRefund:
                case BalanceTransactionContextType.PlatformPaymentRefund:
                    return Activator.CreateInstance<RefundBalanceTransaction>();
                case BalanceTransactionContextType.Chargeback:
                case BalanceTransactionContextType.PlatformPaymentChargeback:
                    return Activator.CreateInstance<ChargebackBalanceTransaction>();
                case BalanceTransactionContextType.OutgoingTransfer:
                case BalanceTransactionContextType.CanceledOutgoingTransfer:
                case BalanceTransactionContextType.ReturnedTransfer:
                    return Activator.CreateInstance<SettlementBalanceTransaction>();
                case BalanceTransactionContextType.InvoiceCompensation:
                    return Activator.CreateInstance<InvoiceBalanceTransaction>();
                default: 
                    return Activator.CreateInstance<BalanceTransaction>();
            }
        }
    }
}