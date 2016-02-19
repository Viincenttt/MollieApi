using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Mollie.WebApplicationExample.Infrastructure {
    public static class AppSettings {
        private static NameValueCollection Settings;

        public static void Initialize(NameValueCollection appSettings) {
            Settings = appSettings;
        }

        public static string MollieApiKey => GetSetting<string>("MollieApiKey");

        private static T GetSetting<T>(string key, T defaultValue = default(T)) {
            string value = Settings[key];

            if (value == null) {
                return defaultValue;
            }

            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }
    }
}