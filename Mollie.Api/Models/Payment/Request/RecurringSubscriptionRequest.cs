using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mollie.Api.Models.Payment.Request
{
    public class RecurringSubscriptionRequest : PaymentRequest
    {
        /// <summary>
        /// Id of target customer.
        /// </summary>
        public string CustomerId { get; set; }

        /// <summary>
        /// Recurring type required by Mollie. First for first payment. Recurring after first successful payment.
        /// </summary>
        public RecurringType RecurringType { get; set; }

        /// <summary>
        /// Default constructor.
        /// Prefer to get Mandate through iDeal, but other methods are possible too.
        /// </summary>
        public RecurringSubscriptionRequest()
        {
            this.Method = PaymentMethod.Ideal;
        }
    }
}
