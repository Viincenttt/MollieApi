using System.ComponentModel.DataAnnotations;

namespace Mollie.WebApplicationCoreExample.Models {
    public class CreateCustomerModel {
        [Required]
        public string Name { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}