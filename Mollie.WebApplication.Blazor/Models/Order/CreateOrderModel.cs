using System.ComponentModel.DataAnnotations;
using Mollie.Api.Models;
using Mollie.WebApplication.Blazor.Framework.Validators;

namespace Mollie.WebApplication.Blazor.Models.Order; 

public class CreateOrderModel {
    [Required]
    public string OrderNumber { get; set; }
    
    [Required]
    public string Locale { get; set; }
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