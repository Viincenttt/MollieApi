using System;
using System.Collections.Generic;
using System.Text;

namespace Mollie.Api.Models.Payment.Response.Specific {
    public class BancontactPaymentResponse : PaymentResponse {
        public BancontactPaymentResponseDetails Details { get; set; }
    }

    public class BancontactPaymentResponseDetails {
        public string CardNumber { get; set; }
    }
}
