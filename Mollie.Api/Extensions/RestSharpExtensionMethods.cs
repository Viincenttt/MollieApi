using System.Net;
using RestSharp;

namespace Mollie.Api.Extensions {
    public static class RestSharpExtensionMethods {
        public static bool IsSuccessful(this IRestResponse response) {
            return response.StatusCode.IsScuccessStatusCode()
                && response.ResponseStatus == ResponseStatus.Completed;
        }

        public static bool IsScuccessStatusCode(this HttpStatusCode responseCode) {
            int numericResponse = (int)responseCode;
            return numericResponse >= 200
                && numericResponse <= 399;
        }
    }
}
