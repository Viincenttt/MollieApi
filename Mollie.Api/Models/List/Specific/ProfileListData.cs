using Mollie.Api.Models.Profile.Response;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List.Specific {
    public class ProfileListData : IListData<ProfileResponse> {
        [JsonProperty("profiles")]
        public List<ProfileResponse> Items { get; set; }
    }
}