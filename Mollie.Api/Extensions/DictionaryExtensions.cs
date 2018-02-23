using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Mollie.Api.Extensions {
    internal static class DictionaryExtensions {
        public static string ToQueryString(this Dictionary<string, string> parameters) {
            if (!parameters.Any())
                return string.Empty;

            return "?" + string.Join("&", parameters.Select(x => $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}"));
        }
    }
}