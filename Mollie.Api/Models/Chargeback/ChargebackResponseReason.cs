using System;
using System.Collections.Generic;
using System.Text;

namespace Mollie.Api.Models.Chargeback {
    public class ChargebackResponseReason {
        /// <summary>
        /// The reason for the chargeback, these are documented here on Mollie's webiste https://help.mollie.com/hc/en-us/articles/115000309865-Why-did-my-direct-debit-payment-fail-
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// an accompanying note to the code
        /// </summary>
        public string Description { get; set; }
    }
}
