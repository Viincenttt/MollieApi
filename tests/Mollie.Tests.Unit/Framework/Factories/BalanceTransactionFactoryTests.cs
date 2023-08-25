using System;
using FluentAssertions;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific;
using Xunit;

namespace Mollie.Tests.Unit.Framework.Factories {
    public class BalanceTransactionFactoryTests {
        [Theory]
        [InlineData(BalanceTransactionContextType.Payment, typeof(PaymentBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.Capture, typeof(CaptureBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.UnauthorizedDirectDebit, typeof(PaymentBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.FailedPayment, typeof(PaymentBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.Refund, typeof(RefundBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.ReturnedRefund, typeof(RefundBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.Chargeback, typeof(ChargebackBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.ChargebackReversal, typeof(PaymentBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.OutgoingTransfer, typeof(SettlementBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.CanceledOutgoingTransfer, typeof(SettlementBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.ReturnedTransfer, typeof(SettlementBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.InvoiceCompensation, typeof(InvoiceBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.BalanceCorrection, typeof(BalanceTransaction))]
        [InlineData(BalanceTransactionContextType.ApplicationFee, typeof(PaymentBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.SplitPayment, typeof(PaymentBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.PlatformPaymentRefund, typeof(RefundBalanceTransaction))]
        [InlineData(BalanceTransactionContextType.PlatformPaymentChargeback, typeof(ChargebackBalanceTransaction))]
        [InlineData("UnknownType", typeof(BalanceTransaction))]
        public void Create_CreatesTypeBasedOnType(string type, Type expectedType) {
            // Given
            var sut = new BalanceTransactionFactory();

            // When
            var result = sut.Create(type);

            // Then
            result.Should().BeOfType(expectedType);
        }
    }
}