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
                case BalanceTransactionContextType.ApplicationFee:
                case BalanceTransactionContextType.SplitPayment:
                case BalanceTransactionContextType.CaptureCommission:
                case BalanceTransactionContextType.PaymentCommission:
                case BalanceTransactionContextType.ReimbursementFee:
                case BalanceTransactionContextType.FailedPaymentFee:
                case BalanceTransactionContextType.PaymentFee:
                case BalanceTransactionContextType.PostPaymentSplitPayment:
                    return Activator.CreateInstance<PaymentBalanceTransactionResponse>();
                case BalanceTransactionContextType.Capture:
                case BalanceTransactionContextType.CaptureRollingReserveRelease:
                    return Activator.CreateInstance<CaptureBalanceTransactionResponse>();
                case BalanceTransactionContextType.Refund:
                case BalanceTransactionContextType.ReturnedRefund:
                case BalanceTransactionContextType.RefundCompensation:
                case BalanceTransactionContextType.ReturnedRefundCompensation:
                case BalanceTransactionContextType.PlatformPaymentRefund:
                case BalanceTransactionContextType.ReturnedPlatformPaymentRefund:
                    return Activator.CreateInstance<RefundBalanceTransactionResponse>();
                case BalanceTransactionContextType.Chargeback:
                case BalanceTransactionContextType.ChargebackReversal:
                case BalanceTransactionContextType.ChargebackCompensation:
                case BalanceTransactionContextType.ReversedChargebackCompensation:
                case BalanceTransactionContextType.PlatformPaymentChargeback:
                case BalanceTransactionContextType.ReversedPlatformPaymentChargeback:
                    return Activator.CreateInstance<ChargebackBalanceTransactionResponse>();
                case BalanceTransactionContextType.ManagedFee:
                case BalanceTransactionContextType.ReturnedManagedFee:
                    return Activator.CreateInstance<ManagedFeeBalanceTransactionResponse>();
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
