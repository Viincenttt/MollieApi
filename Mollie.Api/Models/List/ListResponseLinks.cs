namespace Mollie.Api.Models.List {
    /// <summary>
    /// Links to help navigate through the lists of objects, based on the given offset.
    /// </summary>
    public class ListResponseLinks {
        /// <summary>
        /// The previous set of objects, if available.
        /// </summary>
        public string Previous { get; set; }
        /// <summary>
        /// The next set of objects, if available.
        /// </summary>
        public string Next { get; set; }
        /// <summary>
        /// The first set of objects, if available.
        /// </summary>
        public string First { get; set; }
        /// <summary>
        /// The last set of objects, if available.
        /// </summary>
        public string Last { get; set; }
    }
}
