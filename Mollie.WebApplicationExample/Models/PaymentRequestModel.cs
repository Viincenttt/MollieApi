using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplicationExample.Models {
    public class PaymentRequestModel {
        [Required]
        [Range(1, 1000)]
        public decimal Amount { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Url]
        public string RedirectUrl { get; set; }
    }
}