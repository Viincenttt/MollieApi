using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplication.Blazor.Models.Mandate; 

public class CreateMandateModel {
    [Required]
    public string ConsumerName { get; set; }
    
    [Required]
    public string ConsumerAccount { get; set; }
}