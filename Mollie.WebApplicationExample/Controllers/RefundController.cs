using System.Threading.Tasks;
using System.Web.Mvc;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.WebApplicationExample.Infrastructure;

namespace Mollie.WebApplicationExample.Controllers {
    public class RefundController : Controller {
        private readonly IRefundClient _refundClient;

        public RefundController() {
            this._refundClient = new RefundClient(AppSettings.MollieApiKey);
        }

        [HttpPost]
        public async Task<ActionResult> Refund(string paymentId) {
            await this._refundClient.CreateRefundAsync(paymentId);

            return this.RedirectToAction("Detail", "Payment", new { id = paymentId });
        }
    }
}