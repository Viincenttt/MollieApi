using Mollie.Api.Models.Url;

namespace Mollie.WebApplicationCoreExample.Models {
    public class OverviewNavigationLinks {
        public UrlLink Previous { get; set; }
        public UrlLink Next { get; set; }

        public OverviewNavigationLinks(UrlLink previous, UrlLink next) {
            this.Previous = previous;
            this.Next = next;
        }
    }
}