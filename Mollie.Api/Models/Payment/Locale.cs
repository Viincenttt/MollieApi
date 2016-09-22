using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Mollie.Api.Models.Payment {
    public static class Locale {
        public static string DE = "de";
        public static string EN = "en";
        public static string ES = "es";
        public static string FR = "fr";
        public static string BE = "be";
        public static string BEFR = "be-fr";
        public static string NL = "nl";
    }

    /*
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Locale {
        [EnumMember(Value = "de")]
        DE,
        [EnumMember(Value = "en")]
        EN,
        [EnumMember(Value = "es")]
        ES,
        [EnumMember(Value = "fr")]
        FR,
        [EnumMember(Value = "be")]
        BE,
        [EnumMember(Value = "be-fr")]
        BEFR,
        [EnumMember(Value = "nl")]
        NL
    }*/
}
