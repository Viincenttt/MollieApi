using Mollie.Api.Models.Permission;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mollie.Api.Models.List.Specific {
    public class PermissionListData : IListData<PermissionResponse> {
        [JsonProperty("permissions")]
        public List<PermissionResponse> Items { get; set; }
    }
}