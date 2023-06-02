using System.ComponentModel.DataAnnotations;
using Mollie.WebApplication.Blazor.Framework.Validators;

namespace Mollie.WebApplication.Blazor.Models.Order; 

public class CreateOrderLineModel {
    [Required]
    public string Name { get; set; }

    [Required]
    [Range(1, 100, ErrorMessage = "Please enter a quantity between 0.01 and 1000")]
    public int Quantity { get; set; }
    
    [Required]
    [Range(0.01, 10000, ErrorMessage = "Please enter a unit price between 0.01 and 10000")]
    [DecimalPlaces(2)]
    public decimal UnitPrice { get; set; }
    
    [Required]
    [Range(0.01, 10000, ErrorMessage = "Please enter a total amount between 0.01 and 10000")]
    [DecimalPlaces(2)]
    public decimal TotalAmount { get; set; }
    
    [Range(0.01, 100, ErrorMessage = "Please enter a vat rate between 0.01 and 100")]
    [DecimalPlaces(2)]
    public decimal VatRate { get; set; }
    
    [Required]
    [Range(0.01, 10000, ErrorMessage = "Please enter a vat amount between 0.01 and 10000")]
    [DecimalPlaces(2)]
    public decimal VatAmount { get; set; }
}