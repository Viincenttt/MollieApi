using AutoMapper;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models.Mandate;
using Mollie.WebApplicationExample.Models;

namespace Mollie.WebApplicationExample.Services.Mandate; 

public class MandateOverviewClient : OverviewClientBase<MandateResponse>, IMandateOverviewClient {
    private readonly IMandateClient _mandateClient;

    public MandateOverviewClient(IMapper mapper, IMandateClient mandateClient) : base (mapper) {
        this._mandateClient = mandateClient;
    }

    public async Task<OverviewModel<MandateResponse>> GetList(string customerId) {
        return this.Map(await this._mandateClient.GetMandateListAsync(customerId));
    }

    public async Task<OverviewModel<MandateResponse>> GetListByUrl(string url) {
        return this.Map(await this._mandateClient.GetMandateListAsync(this.CreateUrlObject(url)));
    }
}