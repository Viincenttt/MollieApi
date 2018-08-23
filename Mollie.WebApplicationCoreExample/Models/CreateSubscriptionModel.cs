using System.ComponentModel.DataAnnotations;
using Mollie.Api.Models;
using Mollie.WebApplicationCoreExample.Framework.Validators;

namespace Mollie.WebApplicationCoreExample.Models {
    public class CreateSubscriptionModel {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        [Range(0.01, 1000, ErrorMessage = "Please enter an amount between 0.01 and 1000")]
        [RegularExpression(@"\d+(\.\d{2})?", ErrorMessage = "Please enter an amount with two decimal places")]
        public decimal Amount { get; set; }

        [Required]
        [StaticStringList(typeof(Currency))]
        public string Currency { get; set; }

        [Range(1, 10)]
        public int? Times { get; set; }

        [Range(1, 20, ErrorMessage = "Please enter a interval number between 1 and 20")]
        [Required]
        [Display(Name = "Interval amount")]
        public int? IntervalAmount { get; set; }

        [Required]
        [Display(Name = "Interval period")]
        public IntervalPeriod IntervalPeriod { get; set; }

        [Required]
        public string Description { get; set; }
    }

    public enum IntervalPeriod {
        Months,
        Weeks,
        Days
    }
}