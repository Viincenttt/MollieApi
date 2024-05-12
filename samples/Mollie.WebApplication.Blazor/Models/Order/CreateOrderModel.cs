using System.ComponentModel.DataAnnotations;
using Mollie.Api.Models;
using Mollie.WebApplication.Blazor.Framework.Validators;

namespace Mollie.WebApplication.Blazor.Models.Order;

public class CreateOrderModel {
    [Required]
    public string? OrderNumber { get; set; }

    [Required]
    public string? Locale { get; set; }

    [Required]
    public decimal? Amount { get; set; }

    [Required]
    [StaticStringList(typeof(Currency))]
    public required string Currency { get; set; }

    [Required]
    [Url]
    public required string RedirectUrl { get; set; }

    public List<CreateOrderLineModel>? Lines { get; set; } = new();

    public required CreateOrderBillingAddressModel BillingAddress { get; set; }
}
