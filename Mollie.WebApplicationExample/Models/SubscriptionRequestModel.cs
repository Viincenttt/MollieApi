using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplicationExample.Models {
    public class SubscriptionRequestModel {
        public string CustomerId { get; set; }

        [Required]
        [Range(1, 1000)]
        public decimal Amount { get; set; }

        [Required]
        public string Description { get; set; }
    }
}