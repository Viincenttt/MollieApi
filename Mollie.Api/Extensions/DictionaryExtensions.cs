using System.Collections.Generic;
using System.Linq;

namespace Mollie.Api.Extensions
{
	public static class DictionaryExtensions
	{
		public static string ToQueryString(this Dictionary<string, string> parameters)
		{
			if (!parameters.Any())
				return string.Empty;

			return "?" + string.Join("&", parameters.Select(x => $"{x.Key}={x.Value}"));
		}
	}
}