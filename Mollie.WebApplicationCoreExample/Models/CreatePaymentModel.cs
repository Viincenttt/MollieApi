using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplicationCoreExample.Models {
    public class CreatePaymentModel {
        [Required]
        [Range(0.01, 1000, ErrorMessage = "Please enter a number between 0.01 and 1000")]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string Description { get; set; }
    }
}