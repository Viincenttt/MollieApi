using System;
using Shouldly;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific;
using Xunit;

namespace Mollie.Tests.Unit.Framework.Factories {
    public class BalanceTransactionFactoryTests {
        [Theory]
        [InlineData(BalanceTransactionContextType.Payment, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.Capture, typeof(CaptureBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.UnauthorizedDirectDebit, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.FailedPayment, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.Refund, typeof(RefundBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ReturnedRefund, typeof(RefundBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.Chargeback, typeof(ChargebackBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ChargebackReversal, typeof(ChargebackBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.OutgoingTransfer, typeof(SettlementBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.CanceledOutgoingTransfer, typeof(SettlementBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ReturnedTransfer, typeof(SettlementBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.InvoiceCompensation, typeof(InvoiceBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.BalanceCorrection, typeof(BalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ApplicationFee, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.SplitPayment, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.PlatformPaymentRefund, typeof(RefundBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.PlatformPaymentChargeback, typeof(ChargebackBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.CaptureCommission, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.CaptureRollingReserveRelease, typeof(CaptureBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.RefundCompensation, typeof(RefundBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ReturnedRefundCompensation, typeof(RefundBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ChargebackCompensation, typeof(ChargebackBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ReversedChargebackCompensation, typeof(ChargebackBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ReturnedPlatformPaymentRefund, typeof(RefundBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ReversedPlatformPaymentChargeback, typeof(ChargebackBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.PaymentCommission, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ReimbursementFee, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.FailedPaymentFee, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.PaymentFee, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ManagedFee, typeof(ManagedFeeBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ReturnedManagedFee, typeof(ManagedFeeBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.PostPaymentSplitPayment, typeof(PaymentBalanceTransactionResponse))]
        [InlineData("UnknownType", typeof(BalanceTransactionResponse))]
        public void Create_CreatesTypeBasedOnType(string type, Type expectedType) {
            // Given
            var sut = new BalanceTransactionFactory();

            // When
            var result = sut.Create(type);

            // Then
            result.ShouldBeOfType(expectedType);
        }
    }
}
