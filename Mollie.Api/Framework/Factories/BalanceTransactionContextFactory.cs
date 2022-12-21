using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific;

namespace Mollie.Api.Framework.Factories {
    public class BalanceTransactionContextFactory {
        public BaseTransactionContext Create(string type) {
            switch (type) {
                case BalanceTransactionContextType.Payment:
                case BalanceTransactionContextType.UnauthorizedDirectDebit:
                case BalanceTransactionContextType.FailedPayment:
                case BalanceTransactionContextType.ChargebackReversal:
                case BalanceTransactionContextType.ApplicationFee:
                case BalanceTransactionContextType.SplitPayment:
                    return new PaymentTransactionContext();
                case BalanceTransactionContextType.Capture:
                    return new CaptureTransactionContext();
                case BalanceTransactionContextType.Refund:
                case BalanceTransactionContextType.ReturnedRefund:
                case BalanceTransactionContextType.PlatformPaymentRefund:
                    return new RefundTransactionContext();
                case BalanceTransactionContextType.Chargeback:
                case BalanceTransactionContextType.PlatformPaymentChargeback:
                    return new ChargebackTransactionContext();
                case BalanceTransactionContextType.OutgoingTransfer:
                case BalanceTransactionContextType.CanceledOutgoingTransfer:
                case BalanceTransactionContextType.ReturnedTransfer:
                    return new SettlementTransactionContext();
                case BalanceTransactionContextType.InvoiceCompensation:
                    return new InvoiceTransactionContext();
                default: 
                    return new BaseTransactionContext();
            }
        }
    }
}