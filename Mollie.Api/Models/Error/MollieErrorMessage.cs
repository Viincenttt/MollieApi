using Newtonsoft.Json;

namespace Mollie.Api.Models.Error {
    public class MollieErrorMessage {
        public int Status { get; set; }
        public string Title { get; set; }
        public string Detail { get; set; }

        /// <summary>
        /// The errors that are returned by the Connect client have a different format for some reason
        /// In order to use the same object, we just map private properties to the public properties
        /// that are used by the public api
        /// </summary>
        [JsonProperty("error")]
        private string Error { set { Title = value; } }

        [JsonProperty("error_description")]
        private string ErrorDescription { set { Detail = value; } }


        public override string ToString() {
            return $"{this.Title} - {this.Detail}";
        }
    }
}