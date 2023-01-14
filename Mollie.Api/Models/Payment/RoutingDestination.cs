namespace Mollie.Api.Models.Payment
{
    public class RoutingDestination
    {
        /// <summary>
        /// The type of destination. Currently only the destination type organization is supported.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Required for destination type organization.The ID of the connected organization the funds should be routed to, for example org_12345.
        ///
        /// Please note that me or the ID of the current organization are not accepted as an organizationId. After all portions of the total payment amount have been routed, the amount left will be routed to the current organization automatically.
        /// </summary>
        public string OrganizationId { get; set; }
    }
}