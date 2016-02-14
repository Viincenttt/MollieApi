using System;
using Mollie.Api.Models;
using Newtonsoft.Json;

namespace Mollie.Api.Client {
    public class MollieApiException : ApplicationException {
        private const string ExceptionMessage =
            "An error has occured while performing an action on the Mollie Api. Please view the Error property for more information about the exception.";

        public MollieErrorMessage Error { get; set; }

        public MollieApiException(string json) : base(ExceptionMessage) {
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(json);

            this.Error = new MollieErrorMessage() {
                Type = jsonObject.error.type,
                Message = jsonObject.error.message,
                Field = jsonObject.error.field
            };
        }
    }
}
