using System;
using Mollie.Api.Models;
using Mollie.Api.Models.Error;

namespace Mollie.Api.Client {
    /// <summary>
    /// The exception that is thrown by <see cref="MollieResult.EnsureSuccess"/> when the Mollie API returned an error.
    /// Client methods never throw this exception themselves, they return a <see cref="MollieResult"/> instead.
    /// </summary>
    public class MollieApiException : Exception {
        /// <summary>
        /// The error details returned by the Mollie API.
        /// </summary>
        public MollieErrorMessage Details { get; }

        /// <summary>
        /// The unsuccessful result this exception was created from. Contains the request and response details,
        /// such as the HTTP status code and the raw response body.
        /// </summary>
        public MollieResult Result { get; }

        public MollieApiException(MollieErrorMessage details, MollieResult result) : base(details.ToString()) {
            Details = details;
            Result = result;
        }
    }
}
