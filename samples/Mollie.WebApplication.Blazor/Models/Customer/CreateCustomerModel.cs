using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplication.Blazor.Models.Customer;

public class CreateCustomerModel {
    [Required]
    public required string Name { get; set; }

    [EmailAddress]
    public required string Email { get; set; }
}
