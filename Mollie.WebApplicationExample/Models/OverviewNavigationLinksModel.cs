using Mollie.Api.Models.Url;

namespace Mollie.WebApplicationExample.Models; 

public class OverviewNavigationLinksModel {
    public UrlLink Previous { get; set; }
    public UrlLink Next { get; set; }

    public OverviewNavigationLinksModel(UrlLink previous, UrlLink next) {
        this.Previous = previous;
        this.Next = next;
    }
}