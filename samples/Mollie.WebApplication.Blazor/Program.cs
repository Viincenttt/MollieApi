using Mollie.Api;
using Mollie.Api.AspNet;
using Mollie.Api.Framework;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMollieApi(options => {
    options.ApiKey = builder.Configuration["Mollie:ApiKey"]!;
    options.RetryPolicy = MollieHttpRetryPolicies.TransientHttpErrorRetryPolicy();
});
builder.Services.AddMollieWebhook(options => {
    options.Secret = builder.Configuration["Mollie:WebhookSecret"]!;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
