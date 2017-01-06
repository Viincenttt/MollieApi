using System;
using Mollie.Api.Client;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentMethod;

namespace Mollie.SimpleExample {
    class Program {
        static void Main(string[] args) {
            string apiKey = "{your_api_test_key}";
            PaymentClient paymentClient = new PaymentClient(apiKey);
            PaymentMethodClient paymentMethodClient = new PaymentMethodClient(apiKey);

            OutputAndWait("Press any key to create a new payment");
            OutputNewPayment(paymentClient);
            OutputAndWait("Press any key to retrieve a list of payments");
            OutputPaymentList(paymentClient);
            OutputAndWait("Press any key to retrieve a list of payment methods");
            OutputPaymentMethods(paymentMethodClient);
            OutputAndWait("Example completed");
        }

        static void OutputNewPayment(PaymentClient paymentClient) {
            Console.WriteLine("Creating a payment");
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = 100,
                Description = "Test payment of the example project",
                RedirectUrl = "http://google.com"
            };

            PaymentResponse paymentResponse = paymentClient.CreatePaymentAsync(paymentRequest).Result;
            Console.WriteLine("Payment created");
            Console.WriteLine("");
            Console.WriteLine($"Payment can be paid on the following URL: {paymentResponse.Links.PaymentUrl}");
        }

        static void OutputPaymentList(PaymentClient paymentClient) {
            Console.WriteLine("Outputting the first 2 payments");
            ListResponse<PaymentResponse> paymentList = paymentClient.GetPaymentListAsync(0, 2).Result;
            foreach (PaymentResponse paymentResponse in paymentList.Data) {
                Console.WriteLine($"Payment Id: { paymentResponse.Id } - Payment method: { paymentResponse.Method }");
            }
        }

        static void OutputPaymentMethods(PaymentMethodClient paymentMethodClient) {
            Console.WriteLine("Ouputting all payment methods");
            ListResponse<PaymentMethodResponse> paymentMethodList = paymentMethodClient.GetPaymentMethodListAsync(0, 100).Result;
            foreach (PaymentMethodResponse paymentMethodResponse in paymentMethodList.Data) {
                Console.WriteLine($"Payment method description: { paymentMethodResponse.Description }");
            }
        }

        static void OutputAndWait(string output) {
            Console.WriteLine(output);
            Console.ReadKey();
        }
    }
}
