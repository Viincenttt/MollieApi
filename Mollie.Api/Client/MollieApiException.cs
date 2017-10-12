using System;
using Mollie.Api.Models;
using Newtonsoft.Json;

namespace Mollie.Api.Client {
    public class MollieApiException : Exception {
        public MollieErrorMessage Details { get; set; }

        public MollieApiException(string json) : base(CreateExceptionMessage(ParseErrorJsonResponse(json))) {
            this.Details = ParseErrorJsonResponse(json);
        }

        private static string CreateExceptionMessage(MollieErrorMessage error) {
            if (!String.IsNullOrEmpty(error.Field)) {
                return $"Error occured in field: {error.Field} - {error.Message}";
            }

            return error.Message;
        }

        private static MollieErrorMessage ParseErrorJsonResponse(string json) {
            dynamic jsonObject = JsonConvert.DeserializeObject<dynamic>(json);

            return new MollieErrorMessage() {
                Message = jsonObject.error.message,
                Field = jsonObject.error.field,
                Type = jsonObject.error.type,
            };
        }
    }
}
