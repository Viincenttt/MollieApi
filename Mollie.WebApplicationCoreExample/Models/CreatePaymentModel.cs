using System.ComponentModel.DataAnnotations;
using Mollie.Api.Models;
using Mollie.WebApplicationCoreExample.Framework.Validators;

namespace Mollie.WebApplicationCoreExample.Models
{
    public class CreatePaymentModel
    {
        [Required]
        [Range(0.01, 1000, ErrorMessage = "Please enter an amount between 0.01 and 1000")]
        [DecimalPlaces(2)]
        public decimal Amount { get; set; }

        [Required]
        [StaticStringList(typeof(Currency))]
        public string Currency { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
