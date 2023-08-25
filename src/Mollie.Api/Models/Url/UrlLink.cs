namespace Mollie.Api.Models.Url {
    public class UrlLink {
        public string Href { get; set; }
        public string Type { get; set; }

        public override string ToString() {
            return $"{this.Type} - {this.Href}";
        }
    }
}