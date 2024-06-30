using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplication.Blazor.Models.Mandate;

public class CreateMandateModel {
    [Required]
    public required string ConsumerName { get; set; }

    [Required]
    public required string ConsumerAccount { get; set; }

    [Required]
    public required string ConsumerBic { get; set; }
}
