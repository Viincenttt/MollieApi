using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplication.Blazor.Models.Customer;

public class CreateCustomerModel {
    [Required]
    public string Name { get; set; }

    [EmailAddress]
    public string Email { get; set; }
}
