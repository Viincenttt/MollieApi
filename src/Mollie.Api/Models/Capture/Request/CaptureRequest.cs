namespace Mollie.Api.Models.Capture.Request {
    public class CaptureRequest {
        /// <summary>
        /// The amount to capture.
        /// </summary>
        public Amount Amount { get; set; }
        
        /// <summary>
        /// The description of the capture you are creating.
        /// </summary>
        public string Description { get; set; }
    }
}