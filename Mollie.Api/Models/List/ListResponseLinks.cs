namespace Mollie.Api.Models.List {
    /// <summary>
    /// Links to help navigate through the lists of objects, based on the given offset.
    /// </summary>
    public class ListResponseLinks {
        /// <summary>
        /// The URL to the current set of payments.
        /// </summary>
        public string Self { get; set; }

        /// <summary>
        /// The previous set of objects, if available.
        /// </summary>
        public string Previous { get; set; }

        /// <summary>
        /// The next set of objects, if available.
        /// </summary>
        public string Next { get; set; }

        /// <summary>
        /// The URL to the payments list endpoint documentation.
        /// </summary>
        public string Documentation { get; set; }
    }
}