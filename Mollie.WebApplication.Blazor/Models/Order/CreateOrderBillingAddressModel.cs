﻿using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplication.Blazor.Models.Order; 

public class CreateOrderBillingAddressModel {
    [Required]
    public string GivenName { get; set; }
    
    [Required]
    public string FamilyName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    public string StreetAndNumber { get; set; }
    
    [Required]
    public string City { get; set; }
    
    [Required]
    [MaxLength(2)]
    public string Country { get; set; }
    
    [Required]
    public string PostalCode { get; set; }
}