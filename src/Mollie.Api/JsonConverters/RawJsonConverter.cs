using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Mollie.Api.JsonConverters {
    internal class RawJsonConverter : JsonConverter {
        public override bool CanConvert(Type objectType) {
            return objectType == typeof(string);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer) {
            string valueToParse = value!.ToString();
            if (IsValidJson(valueToParse)) {
                writer.WriteRawValue(valueToParse);
            }
            else {
                writer.WriteValue(valueToParse);
            }            
        }

        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer) {
            return JToken.Load(reader).ToString();
        }

        /// <summary>
        /// Source: https://stackoverflow.com/questions/14977848/how-to-make-sure-that-string-is-valid-json-using-json-net
        /// </summary>
        private bool IsValidJson(string strInput) {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) ||
                (strInput.StartsWith("[") && strInput.EndsWith("]")))
            {
                try {
                    var obj = JToken.Parse(strInput);
                    return true;
                }
                catch (JsonReaderException) {
                    return false;
                }
                catch (Exception) {
                    return false;
                }
            }
            else {
                return false;
            }
        }
    }
}