using Mollie.Api.JsonConverters;
using Newtonsoft.Json;

namespace Mollie.Api.Models.Capture.Request {
    public class CaptureRequest {
        /// <summary>
        /// The amount to capture.
        /// </summary>
        public Amount Amount { get; set; }
        
        /// <summary>
        /// The description of the capture you are creating.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// Provide any data you like, for example a string or a JSON object. We will save the data alongside the capture.
        /// Whenever you fetch the capture with our API, we will also include the metadata. You can use up to
        /// approximately 1kB.
        /// </summary>
        [JsonConverter(typeof(RawJsonConverter))]
        public string Metadata { get; set; }
        
        public void SetMetadata(object metadataObj, JsonSerializerSettings jsonSerializerSettings = null) {
            this.Metadata = JsonConvert.SerializeObject(metadataObj, jsonSerializerSettings);
        }
        
        public override string ToString() {
            return $"Amount: {this.Amount} Description: {this.Description}";
        }
    }
}