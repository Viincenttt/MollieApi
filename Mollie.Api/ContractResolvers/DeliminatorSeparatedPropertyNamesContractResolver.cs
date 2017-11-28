using Newtonsoft.Json.Serialization;

namespace Mollie.Api.ContractResolvers {
    public class DeliminatorSeparatedPropertyNamesContractResolver : DefaultContractResolver {
        private readonly string _separator;

        protected DeliminatorSeparatedPropertyNamesContractResolver(char separator) {
            this._separator = separator.ToString();
        }

        protected override string ResolvePropertyName(string propertyName) {
            for (var j = propertyName.Length - 1; j > 0; j--)
                if (j > 0 && char.IsUpper(propertyName[j]) || j > 0 && char.IsNumber(propertyName[j]) &&
                    !char.IsNumber(propertyName[j - 1]))
                    propertyName = propertyName.Insert(j, this._separator);
            return propertyName.ToLower();
        }
    }
}