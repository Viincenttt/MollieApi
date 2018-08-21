using Mollie.Api.Models.Url;

namespace Mollie.WebApplicationCoreExample.Models {
    public class OverviewNavigationLinksModel {
        public UrlLink Previous { get; set; }
        public UrlLink Next { get; set; }

        public OverviewNavigationLinksModel(UrlLink previous, UrlLink next) {
            this.Previous = previous;
            this.Next = next;
        }
    }
}