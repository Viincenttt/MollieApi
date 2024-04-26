namespace Mollie.Api.Models.Order.Request.PaymentSpecificParameters {
    public record BillieSpecificParameters : PaymentSpecificParameters {
        /// <summary>
        /// Billie is a B2B payment method, thus it requires some extra information to identify the business
        /// that is creating the order. It is recommended to include these parameters as part of the create
        /// order request for a seamless flow, otherwise the customer will be asked to fill the missing fields
        /// at the Billie’s checkout page.
        /// </summary>
        public CompanyObject? Company { get; set; }
    }
}