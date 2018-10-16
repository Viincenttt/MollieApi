using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Mollie.Api.Extensions
{
    public static class RequestExtensions
    {
        public static IDictionary<string, string> ToDictionary<T>(this T request)
        {
            var requestTypeProperties = request.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public);


            string ElementSelector(PropertyInfo prop) => prop.PropertyType.GetTypeInfo().IsEnum
                ? (prop.GetValue(request, null) as Enum).StringValueOf()
                : prop.GetValue(request, null)?.ToString();

            return requestTypeProperties.ToDictionary(KeySelector, ElementSelector);
        }

        private static string  KeySelector(PropertyInfo prop)
        {
            return Regex.Replace(prop.Name, @"(\w)([A-Z])", "$1_$2").ToLower();
        }

        public static string ToSortedQueryString(this IDictionary<string, string> parameters)
        {
            var sortedParameters = parameters.ToList();

            sortedParameters.Sort((x1, x2) => string.Compare(x1.Key, x2.Key, StringComparison.Ordinal));

            return string.Join("&", sortedParameters.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={ (!string.IsNullOrEmpty(kvp.Value) ? Uri.EscapeDataString(kvp.Value) : string.Empty)}"));
        }

        public static string StringValueOf(this Enum value)
        {
            var fieldInfo = value.GetType().GetField(value.ToString());

            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Length > 0 ? attributes[0].Description : value.ToString();
        }
    }
}
