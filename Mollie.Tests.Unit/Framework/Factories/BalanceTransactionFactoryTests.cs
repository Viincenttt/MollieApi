using System;
using Mollie.Api.Framework.Factories;
using Mollie.Api.Models.Balance.Response.BalanceTransaction;
using Mollie.Api.Models.Balance.Response.BalanceTransaction.Specific;
using NUnit.Framework;

namespace Mollie.Tests.Unit.Framework.Factories {
    [TestFixture]
    public class BalanceTransactionFactoryTests {
        [TestCase(BalanceTransactionContextType.Payment, typeof(PaymentBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.Capture, typeof(CaptureBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.UnauthorizedDirectDebit, typeof(PaymentBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.FailedPayment, typeof(PaymentBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.Refund, typeof(RefundBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.ReturnedRefund, typeof(RefundBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.Chargeback, typeof(ChargebackBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.ChargebackReversal, typeof(PaymentBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.OutgoingTransfer, typeof(SettlementBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.CanceledOutgoingTransfer, typeof(SettlementBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.ReturnedTransfer, typeof(SettlementBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.InvoiceCompensation, typeof(InvoiceBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.BalanceCorrection, typeof(BalanceTransaction))]
        [TestCase(BalanceTransactionContextType.ApplicationFee, typeof(PaymentBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.SplitPayment, typeof(PaymentBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.PlatformPaymentRefund, typeof(RefundBalanceTransaction))]
        [TestCase(BalanceTransactionContextType.PlatformPaymentChargeback, typeof(ChargebackBalanceTransaction))]
        [TestCase("UnknownType", typeof(BalanceTransaction))]
        public void Create_CreatesTypeBasedOnType(string type, Type expectedType) {
            // Given
            var sut = new BalanceTransactionFactory();
            
            // When
            var result = sut.Create(type);

            // Then
            Assert.AreEqual(expectedType, result.GetType());
        }
    }
}