using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplication.Blazor.Models.Order;

public class CreateOrderBillingAddressModel {
    [Required]
    public required string GivenName { get; set; }

    [Required]
    public required string FamilyName { get; set; }

    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    public required string StreetAndNumber { get; set; }

    [Required]
    public required string City { get; set; }

    [Required]
    [MaxLength(2)]
    public required string Country { get; set; }

    [Required]
    public required string PostalCode { get; set; }
}
