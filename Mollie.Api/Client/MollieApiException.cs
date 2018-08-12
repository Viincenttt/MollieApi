using System;
using Mollie.Api.Models.Error;
using Newtonsoft.Json;

namespace Mollie.Api.Client {
    public class MollieApiException : Exception {
        public MollieErrorMessage Details { get; set; }

        public MollieApiException(string json) : base("Exception occured while communicating with Mollie API. View the Details property for more information") {
            this.Details = JsonConvert.DeserializeObject<MollieErrorMessage>(json);
        }
    }
}