using System.Net.Http;
using Mollie.Api.Client.Abstract;

namespace Mollie.Api.Client {
    public class OrderClient : BaseMollieClient, IOrderClient {
        public OrderClient(string apiKey, HttpClient httpClient = null) : base(apiKey, httpClient) {
        }
    }
}