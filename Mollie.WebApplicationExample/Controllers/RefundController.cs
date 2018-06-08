using System.Threading.Tasks;
using System.Web.Mvc;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Refund;
using Mollie.WebApplicationExample.Infrastructure;

namespace Mollie.WebApplicationExample.Controllers {
    public class RefundController : Controller {
        private readonly IRefundClient _refundClient;

        public RefundController() {
            this._refundClient = new RefundClient(AppSettings.MollieApiKey);
        }

        [HttpPost]
        public async Task<ActionResult> Refund(string paymentId, string currency, string amount) {
            await this._refundClient.CreateRefundAsync(paymentId, new RefundRequest() { Amount = new Amount(currency, amount) } );

            return this.RedirectToAction("Detail", "Payment", new { id = paymentId });
        }
    }
}