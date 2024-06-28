using System;
using System.Net;
using Mollie.Api.Models.Error;
using Newtonsoft.Json;

namespace Mollie.Api.Client {
    public class MollieApiException : Exception {
        public MollieErrorMessage Details { get; set; }

        public MollieApiException(HttpStatusCode httpStatusCode, string responseBody)
            : base(ParseErrorMessage(httpStatusCode, responseBody).ToString()){
            Details = ParseErrorMessage(httpStatusCode, responseBody);
        }

        private static MollieErrorMessage ParseErrorMessage(HttpStatusCode httpStatusCode, string responseBody) {
            try {
                return JsonConvert.DeserializeObject<MollieErrorMessage>(responseBody)!;
            }
            catch (JsonReaderException) {
                return new MollieErrorMessage {
                    Title = "Unknown error",
                    Status = (int)httpStatusCode,
                    Detail = responseBody
                };
            }
        }
    }
}
