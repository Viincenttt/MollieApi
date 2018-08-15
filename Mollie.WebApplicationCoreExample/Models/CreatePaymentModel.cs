using System.ComponentModel.DataAnnotations;
using Mollie.Api.Models.Payment.Request;

namespace Mollie.WebApplicationCoreExample.Models {
    public class CreatePaymentModel {
        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string Description { get; set; }
    }
}