using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific;

namespace Mollie.Api.Framework.Factories {
    public class BalanceTransactionFactory {
        public BalanceTransaction Create(string type) {
            switch (type) {
                case BalanceTransactionContextType.Payment:
                case BalanceTransactionContextType.UnauthorizedDirectDebit:
                case BalanceTransactionContextType.FailedPayment:
                case BalanceTransactionContextType.ChargebackReversal:
                case BalanceTransactionContextType.ApplicationFee:
                case BalanceTransactionContextType.SplitPayment:
                    return new PaymentBalanceTransaction();
                case BalanceTransactionContextType.Capture:
                    return new CaptureBalanceTransaction();
                case BalanceTransactionContextType.Refund:
                case BalanceTransactionContextType.ReturnedRefund:
                case BalanceTransactionContextType.PlatformPaymentRefund:
                    return new RefundBalanceTransaction();
                case BalanceTransactionContextType.Chargeback:
                case BalanceTransactionContextType.PlatformPaymentChargeback:
                    return new ChargebackBalanceTransaction();
                case BalanceTransactionContextType.OutgoingTransfer:
                case BalanceTransactionContextType.CanceledOutgoingTransfer:
                case BalanceTransactionContextType.ReturnedTransfer:
                    return new SettlementBalanceTransaction();
                case BalanceTransactionContextType.InvoiceCompensation:
                    return new InvoiceBalanceTransaction();
                default: 
                    return new BalanceTransaction();
            }
        }
    }
}