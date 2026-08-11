using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using Mollie.Api.Models.Error;

namespace Mollie.Api.Models {
    /// <summary>
    /// Represents the outcome of a Mollie API request that does not return a payload.
    /// </summary>
    public record MollieResult {
        /// <summary>
        /// Indicates whether the request was successful.
        /// </summary>
        public virtual bool Success { get; init; }

        /// <summary>
        /// The request body that was sent, if any.
        /// </summary>
        public string? RequestBody { get; init; }

        /// <summary>
        /// The full URL the request was sent to.
        /// </summary>
        public string? RequestUrl { get; init; } = string.Empty;

        /// <summary>
        /// The HTTP method used for the request.
        /// </summary>
        public HttpMethod HttpMethod { get; init; } = HttpMethod.Get;

        /// <summary>
        /// The HTTP status code returned by the API, if available.
        /// </summary>
        public HttpStatusCode? HttpStatusCode { get; init; }

        /// <summary>
        /// The raw response body returned by the API, if any.
        /// </summary>
        public string? ResponseBody { get; init; }

        /// <summary>
        /// The error details returned by the API when the request was not successful.
        /// </summary>
        public MollieErrorMessage? Error { get; init; }
    }

    /// <summary>
    /// Represents the outcome of a Mollie API request that returns a payload of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the data returned by the API.</typeparam>
    public record MollieResult<T> : MollieResult {
        /// <summary>
        /// The deserialized data returned by the API, when the request was successful.
        /// </summary>
        public T? Data { get; init; }

        /// <summary>
        /// Indicates whether the request was successful.
        /// </summary>
        [MemberNotNullWhen(true, nameof(Data))]
        public override bool Success { get; init; }
    }
}
