using System.ComponentModel.DataAnnotations;
using Mollie.Api.Models;
using Mollie.WebApplicationExample.Framework.Validators;

namespace Mollie.WebApplicationExample.Models;

public class CreatePaymentModel {
    [Required]
    [Range(0.01, 1000, ErrorMessage = "Please enter an amount between 0.01 and 1000")]
    [DecimalPlaces(2)]
    public decimal Amount { get; set; }

    [Required]
    [StaticStringList(typeof(Currency))]
    public string Currency { get; set; }
    
    [Required]
    [Url]
    public string RedirectUrl { get; set; }

    [Required]
    public string Description { get; set; }
}

