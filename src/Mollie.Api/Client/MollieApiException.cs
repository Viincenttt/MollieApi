using System;
using Mollie.Api.Models.Error;
using Newtonsoft.Json;

namespace Mollie.Api.Client {
    public class MollieApiException : Exception {
        public MollieErrorMessage Details { get; set; }

        public MollieApiException(string json) : base(ParseErrorMessage(json).ToString()){
            this.Details = ParseErrorMessage(json);
        }

        private static MollieErrorMessage ParseErrorMessage(string json) {
            return JsonConvert.DeserializeObject<MollieErrorMessage>(json)!;
        }
    }
}