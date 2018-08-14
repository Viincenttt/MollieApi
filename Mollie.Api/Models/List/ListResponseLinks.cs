namespace Mollie.Api.Models.List {
    /// <summary>
    /// Links to help navigate through the lists of objects, based on the given offset.
    /// </summary>
    public class ListResponseLinks {
        /// <summary>
        /// The URL to the current set of payments.
        /// </summary>
        public ListResponseLink Self { get; set; }

        /// <summary>
        /// The previous set of objects, if available.
        /// </summary>
        public ListResponseLink Previous { get; set; }

        /// <summary>
        /// The next set of objects, if available.
        /// </summary>
        public ListResponseLink Next { get; set; }

        /// <summary>
        /// The URL to the payments list endpoint documentation.
        /// </summary>
        public ListResponseLink Documentation { get; set; }
    }

    public class ListResponseLink {
        public string Href { get; set; }
        public string Type { get; set; }
    }
}