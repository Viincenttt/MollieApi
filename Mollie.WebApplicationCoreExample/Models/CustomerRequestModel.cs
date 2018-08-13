using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplicationCoreExample.Models
{
    public class CustomerRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Locale { get; set; }
    }
}