using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Models.Customer;
using Mollie.WebApplicationExample.Models;
using Mollie.WebApplicationExample.Services.Customer;

namespace Mollie.WebApplicationExample.Controllers; 

public class CustomerController : Controller {
    private readonly ICustomerOverviewClient _customerOverviewClient;
    private readonly ICustomerStorageClient _customerStorageClient;

    public CustomerController(ICustomerOverviewClient customerOverviewClient, ICustomerStorageClient customerStorageClient) {
        this._customerOverviewClient = customerOverviewClient;
        this._customerStorageClient = customerStorageClient;
    }

    [HttpGet]
    public async Task<ViewResult> Index() {
        OverviewModel<CustomerResponse> model = await this._customerOverviewClient.GetList();
        return this.View(model);
    }

    [HttpGet]
    public async Task<ViewResult> ApiUrl([FromQuery]string url) {
        OverviewModel<CustomerResponse> model = await this._customerOverviewClient.GetListByUrl(url);
        return this.View(nameof(this.Index), model);
    }

    [HttpGet]
    public ViewResult Create() {
        CreateCustomerModel model = new CreateCustomerModel();
        return this.View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCustomerModel model) {
        if (!this.ModelState.IsValid) {
            return this.View();
        }

        await this._customerStorageClient.Create(model);
        return this.RedirectToAction(nameof(this.Index));
    }
}