using System.Threading.Tasks;
using System.Web.Mvc;
using Mollie.Api.Client;
using Mollie.WebApplicationExample.Infrastructure;

namespace Mollie.WebApplicationExample.Controllers {
    public class RefundController : Controller {
        private readonly MollieClient _mollieClient;

        public RefundController() {
            this._mollieClient = new MollieClient(AppSettings.MollieApiKey);
        }

        [HttpPost]
        public async Task<ActionResult> Refund(string paymentId) {
            await this._mollieClient.CreateRefund(paymentId);

            return this.RedirectToAction("Detail", "Payment", new { id = paymentId });
        }
    }
}