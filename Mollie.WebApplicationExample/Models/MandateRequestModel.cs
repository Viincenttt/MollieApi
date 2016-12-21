using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Mollie.WebApplicationExample.Models
{
    public class MandateRequestModel
    {
        [Required]
        public string CustomerId { get; set; }

        [Required]
        public string ConsumerName { get; set; }

        [Required]
        public string ConsumerAccount { get; set; }

        public string ConsumerBic { get; set; }

        public DateTime? SignatureDate { get; set; }

        public string MandateReference { get; set; }
    }
}