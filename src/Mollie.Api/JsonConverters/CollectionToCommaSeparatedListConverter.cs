using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Mollie.Api.JsonConverters;

internal class CollectionToCommaSeparatedListConverter : JsonConverter<IList<string>> {
    public override IList<string> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, IList<string> value, JsonSerializerOptions options) {
        if (value.Count == 0) {
            writer.WriteStringValue(string.Empty);
            return;
        }

        string commaSeparated = string.Join(",", value);
        writer.WriteStringValue(commaSeparated);
    }
}
