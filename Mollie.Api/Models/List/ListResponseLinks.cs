using Mollie.Api.Models.Url;

namespace Mollie.Api.Models.List {
    /// <summary>
    /// Links to help navigate through the lists of objects, based on the given offset.
    /// </summary>
    public class ListResponseLinks<T> {
        /// <summary>
        /// The URL to the current set of payments.
        /// </summary>
        public UrlObjectLink<ListResponse<T>> Self { get; set; }

        /// <summary>
        /// The previous set of objects, if available.
        /// </summary>
        public UrlObjectLink<ListResponse<T>> Previous { get; set; }

        /// <summary>
        /// The next set of objects, if available.
        /// </summary>
        public UrlObjectLink<ListResponse<T>> Next { get; set; }

        /// <summary>
        /// The URL to the payments list endpoint documentation.
        /// </summary>
        public UrlLink Documentation { get; set; }
    }
}