namespace Mollie.Api.Models.Balance.Response {
    public class BalanceTransferDestination {
        /// <summary>
        /// The default destination of automatic scheduled transfers. Currently only bank-account is supported.
        /// </summary>
        public required string Type { get; set; }

        /// <summary>
        /// The configured bank account number of the beneficiary the balance amount is to be transferred to.
        /// </summary>
        public required string BankAccount { get; set; }

        /// <summary>
        /// The full name of the beneficiary the balance amount is to be transferred to.
        /// </summary>
        public required string BeneficiaryName { get; set; }

        public override string ToString() {
            return $"{Type} - {BankAccount} - {BeneficiaryName}";
        }
    }
}
