﻿using System.Text.Json;
using System.Text.Json.Serialization;
using Mollie.Api.JsonConverters;

namespace Mollie.Api.Models.Capture.Request {
    public record CaptureRequest {
        /// <summary>
        /// The amount to capture.
        /// </summary>
        public Amount? Amount { get; set; }

        /// <summary>
        /// The description of the capture you are creating.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Provide any data you like, for example a string or a JSON object. We will save the data alongside the capture.
        /// Whenever you fetch the capture with our API, we will also include the metadata. You can use up to
        /// approximately 1kB.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string? Metadata { get; set; }

        /// <summary>
        ///	Oauth only - Optional – Set this to true to make this capture for a test payment
        /// </summary>
        public bool? Testmode { get; set; }

        public void SetMetadata(object metadataObj, JsonSerializerOptions? jsonSerializerOptions = null) {
            Metadata = JsonSerializer.Serialize(metadataObj, jsonSerializerOptions);
        }

        public override string ToString() {
            return $"Amount: {Amount} Description: {Description}";
        }
    }
}
