using System.Collections.Generic;
namespace Mollie.Api.Extensions {
    internal static class ListExtensions {
        public static string ToIncludeParameter(this List<string> includeList) {
            return string.Join(",", includeList);
        }

        public static void AddValueIfTrue(this List<string> list, string valueToAdd, bool valueToCheck) {
            if (valueToCheck) {
                list.Add(valueToAdd);
            }
        }
    }
}

