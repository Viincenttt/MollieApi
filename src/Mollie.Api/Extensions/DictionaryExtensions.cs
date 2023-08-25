using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Mollie.Tests.Integration")]
namespace Mollie.Api.Extensions {
    public static class DictionaryExtensions {
        public static string ToQueryString(this IDictionary<string, string> parameters) {
            if (!parameters.Any()) {
                return string.Empty;
            }

            return "?" + string.Join("&", parameters.Select(x => $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}"));
        }

        public static void AddValueIfNotNullOrEmpty(this IDictionary<string, string> dictionary, string key, string value) {
            if (!string.IsNullOrEmpty(value)) {
                dictionary.Add(key, value);
            }
        }

        public static void AddValueIfTrue(this IDictionary<string, string> dictionary, string key, bool value) {
            if (value) {
                dictionary.Add(key, bool.TrueString.ToLower());
            }
        }
        
        public static void AddValueIfTrue(this IDictionary<string, string> dictionary, string key, bool? value) {
            if (value == true) {
                dictionary.Add(key, bool.TrueString.ToLower());
            }
        }
    }
}