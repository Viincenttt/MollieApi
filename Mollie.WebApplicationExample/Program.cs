using Mollie.WebApplicationExample.Controllers;
using Mollie.WebApplicationExample.Framework.Middleware;
using Mollie.WebApplicationExample.Services.Customer;
using Mollie.WebApplicationExample.Services.Mandate;
using Mollie.WebApplicationExample.Services.Order;
using Mollie.WebApplicationExample.Services.Payment;
using Mollie.WebApplicationExample.Services.Payment.Refund;
using Mollie.WebApplicationExample.Services.PaymentMethod;
using Mollie.WebApplicationExample.Services.Subscription;
using Mollie.WebApplicationExample.Services.Terminal;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMollieApi(builder.Configuration);
builder.Services.AddScoped<IPaymentOverviewClient, PaymentOverviewClient>();
builder.Services.AddScoped<ICustomerOverviewClient, CustomerOverviewClient>();
builder.Services.AddScoped<ISubscriptionOverviewClient, SubscriptionOverviewClient>();
builder.Services.AddScoped<IMandateOverviewClient, MandateOverviewClient>();
builder.Services.AddScoped<IPaymentMethodOverviewClient, PaymentMethodOverviewClient>();
builder.Services.AddScoped<IOrderOverviewClient, OrderOverviewClient>();
builder.Services.AddScoped<ITerminalOverviewClient, TerminalOverviewClient>();

builder.Services.AddScoped<IPaymentStorageClient, PaymentStorageClient>();
builder.Services.AddScoped<ICustomerStorageClient, CustomerStorageClient>();
builder.Services.AddScoped<ISubscriptionStorageClient, SubscriptionStorageClient>();
builder.Services.AddScoped<IMandateStorageClient, MandateStorageClient>();
builder.Services.AddScoped<IOrderStorageClient, OrderStorageClient>();

builder.Services.AddScoped<IRefundPaymentClient, RefundPaymentClient>();
builder.Services.AddScoped<IRefundPaymentClient, RefundPaymentClient>();

builder.Services.AddAutoMapper(typeof(HomeController));
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();