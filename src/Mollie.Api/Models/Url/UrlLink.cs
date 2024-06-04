namespace Mollie.Api.Models.Url {
    public record UrlLink {
        public required string Href { get; set; }
        public required string Type { get; set; }

        public override string ToString() {
            return $"{Type} - {Href}";
        }
    }
}
