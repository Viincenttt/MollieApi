using System.Text.Json.Serialization;

namespace Mollie.Api.Models.Error {
    public record MollieErrorMessage {
        public int Status { get; set; }
        public required string Title { get; set; }
        public required string Detail { get; set; }

        /// <summary>
        /// The errors that are returned by the Connect client have a different format for some reason
        /// In order to use the same object, we just map private properties to the public properties
        /// that are used by the public api
        /// </summary>
        [JsonPropertyName("error")]
        [JsonInclude]
        private string Error { init { Title = value; } }

        [JsonPropertyName("error_description")]
        [JsonInclude]
        private string ErrorDescription { init { Detail = value; } }

        public override string ToString() {
            return $"{Title} - {Detail}";
        }
    }
}
