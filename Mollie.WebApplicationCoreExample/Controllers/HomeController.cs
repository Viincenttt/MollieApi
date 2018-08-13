using Microsoft.AspNetCore.Mvc;

namespace Mollie.WebApplicationCoreExample.Controllers {
    public class HomeController : Controller {
        public IActionResult Index() {
            return this.View();
        }
    }
}