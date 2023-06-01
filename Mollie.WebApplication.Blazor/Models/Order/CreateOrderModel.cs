using System.ComponentModel.DataAnnotations;
using Mollie.Api.Models;
using Mollie.WebApplication.Blazor.Framework.Validators;

namespace Mollie.WebApplication.Blazor.Models.Order; 

public class CreateOrderModel {
    [Required]
    public string OrderNumber { get; set; }
    
    [Required]
    public string Locale { get; set; }
    
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
    
    public List<CreateOrderLineModel> Lines { get; set; }
    
    public CreateOrderBillingAddressModel BillingAddress { get; set; }
}