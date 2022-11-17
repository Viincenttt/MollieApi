namespace Mollie.Api.Models.Balance.Response {
    public class BalanceTransferDestination {
        /// <summary>
        /// The default destination of automatic scheduled transfers. Currently only bank-account is supported.
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// The configured bank account number of the beneficiary the balance amount is to be transferred to.
        /// </summary>
        public string BankAccount { get; set; }
        
        /// <summary>
        /// The full name of the beneficiary the balance amount is to be transferred to.
        /// </summary>
        public string BeneficiaryName { get; set; }
    }
}