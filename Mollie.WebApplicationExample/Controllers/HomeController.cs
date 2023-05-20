using Microsoft.AspNetCore.Mvc;
namespace Mollie.WebApplicationExample.Controllers;

public class HomeController : Controller {
    public IActionResult Index() {
        return View();
    }
}