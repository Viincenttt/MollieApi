using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplicationCoreExample.Models {
    public class PaymentRequestModel {
        [Required]
        public string Currency { get; set; }

        [Required]
        public string Amount { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [Url]
        public string RedirectUrl { get; set; }
    }
}