using System.Collections.Generic;
using Mollie.Api.Models.Chargeback;

namespace Mollie.Api.Models.List.Specific {
    public interface IListData<T> where T : IResponseObject {
        List<T> Items { get; set; }
    }
}