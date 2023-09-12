using Newtonsoft.Json;

namespace Mollie.Api.Models.ClientLink.Response
{
    public class ClientLinkResponse
    {
        public string Id { get; set; }
        
        public string Resource { get; set; }
        
        [JsonProperty("_links")]
        public ClientLinkResponseLinks Links { get; set; }
    }
}