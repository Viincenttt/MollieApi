namespace Mollie.Api.Models.Url {
    public record UrlLink {
        public required string Href { get; init; }
        public required string Type { get; init; }

        public override string ToString() {
            return $"{Type} - {Href}";
        }
    }
}