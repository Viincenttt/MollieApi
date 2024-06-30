using System;
using Mollie.Api.Models.Error;

namespace Mollie.Api.Client {
    public class MollieApiException : Exception {
        public MollieErrorMessage Details { get; set; }

        public MollieApiException(MollieErrorMessage details) : base(details.ToString()) {
            Details = details;
        }
    }
}
