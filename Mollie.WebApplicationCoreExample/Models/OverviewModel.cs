using System.Collections.Generic;

namespace Mollie.WebApplicationCoreExample.Models {
    public class OverviewModel<T> {
        public List<T> Items { get; set; }
        public OverviewNavigationLinks Navigation { get; set; }
    }
}