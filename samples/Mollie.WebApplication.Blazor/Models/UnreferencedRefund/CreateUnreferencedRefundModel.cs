using System.ComponentModel.DataAnnotations;
using Mollie.Api.Models;
using Mollie.WebApplication.Blazor.Framework.Validators;

namespace Mollie.WebApplication.Blazor.Models.UnreferencedRefund;

public class CreateUnreferencedRefundModel {
    [Required]
    public required string TerminalId { get; set; }

    [Required]
    [Range(0.01, 1000, ErrorMessage = "Please enter an amount between 0.01 and 1000")]
    [DecimalPlaces(2)]
    public required decimal Amount { get; set; }

    [Required]
    [StaticStringList(typeof(Currency))]
    public required string Currency { get; set; }

    [Required]
    [MaxLength(255)]
    public required string Description { get; set; }

    public string? Metadata { get; set; }
}
