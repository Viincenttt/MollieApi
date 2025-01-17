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
        [InlineData(BalanceTransactionContextType.ChargebackReversal, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.OutgoingTransfer, typeof(SettlementBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.CanceledOutgoingTransfer, typeof(SettlementBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ReturnedTransfer, typeof(SettlementBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.InvoiceCompensation, typeof(InvoiceBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.BalanceCorrection, typeof(BalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.ApplicationFee, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.SplitPayment, typeof(PaymentBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.PlatformPaymentRefund, typeof(RefundBalanceTransactionResponse))]
        [InlineData(BalanceTransactionContextType.PlatformPaymentChargeback, typeof(ChargebackBalanceTransactionResponse))]
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
