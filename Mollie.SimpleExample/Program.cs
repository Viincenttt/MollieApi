using System;
using Mollie.Api.Client;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;

namespace Mollie.SimpleExample {
    class Program {
        static void Main(string[] args) {
            MollieApi mollieApi = new MollieApi("{Your API key}");

            CreatePayment(mollieApi);

            Console.WriteLine("Press any key to retrieve a list of payments");
            Console.ReadKey();

            GetPaymentList(mollieApi);

            Console.WriteLine("Press any key to retrieve a list of payments");
            Console.ReadKey();
        }

        static void CreatePayment(MollieApi mollieApi) {
            Console.WriteLine("Creating a payment");
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = 100,
                Description = "Test payment of the example project",
                RedirectUrl = "http://google.com"
            };

            PaymentResponse paymentResponse = mollieApi.CreatePayment(paymentRequest).Result;
            Console.WriteLine("Payment created");
            Console.WriteLine($"Payment can be paid on the following URL: {paymentResponse.Links.PaymentUrl}");
        }

        static void GetPaymentList(MollieApi mollieApi) {
            Console.WriteLine("Outputting the first 2 payments");
            ListResponse<PaymentResponse> paymentList = mollieApi.GetPaymentList(0, 2).Result;
            foreach (PaymentResponse paymentResponse in paymentList.Data) {
                Console.WriteLine($"Payment Id: { paymentResponse.Id } - Payment method: { paymentResponse.Method }");
            }
        }
    }
}
