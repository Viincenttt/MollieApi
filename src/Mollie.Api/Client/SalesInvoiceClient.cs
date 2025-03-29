﻿using System.Net.Http;
using System.Threading.Tasks;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Framework.Authentication.Abstract;
using Mollie.Api.Models.SalesInvoice.Request;
using Mollie.Api.Models.SalesInvoice.Response;

namespace Mollie.Api.Client;

public class SalesInvoiceClient : BaseMollieClient, ISalesInvoiceClient {
    public SalesInvoiceClient(string apiKey, HttpClient? httpClient = null)
        : base(apiKey, httpClient) { }

    public SalesInvoiceClient(IMollieSecretManager mollieSecretManager, HttpClient? httpClient = null)
        : base(mollieSecretManager, httpClient) { }

    public async Task<SalesInvoiceResponse> CreateSalesInvoiceAsync(SalesInvoiceRequest salesInvoiceRequest) {
        return await PostAsync<SalesInvoiceResponse>($"sales-invoices", salesInvoiceRequest).ConfigureAwait(false);
    }
}
