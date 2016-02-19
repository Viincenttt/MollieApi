using System;
using Mollie.Api.Client;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentMethod;

namespace Mollie.SimpleExample {
    class Program {
        static void Main(string[] args) {
            MollieClient mollieClient = new MollieClient("test_ykDGaYHm2y2crCz8VCsjuCDyUg46VN");

            OutputAndWait("Press any key to create a new payment");
            OutputNewPayment(mollieClient);
            OutputAndWait("Press any key to retrieve a list of payments");
            OutputPaymentList(mollieClient);
            OutputAndWait("Press any key to retrieve a list of payment methods");
            OutputPaymentMethods(mollieClient);
            OutputAndWait("Example completed");
        }

        static void OutputNewPayment(MollieClient mollieClient) {
            Console.WriteLine("Creating a payment");
            PaymentRequest paymentRequest = new PaymentRequest() {
                Amount = 100,
                Description = "Test payment of the example project",
                RedirectUrl = "http://google.com"
            };

            PaymentResponse paymentResponse = mollieClient.CreatePaymentAsync(paymentRequest).Result;
            Console.WriteLine("Payment created");
            Console.WriteLine("");
            Console.WriteLine($"Payment can be paid on the following URL: {paymentResponse.Links.PaymentUrl}");
        }

        static void OutputPaymentList(MollieClient mollieClient) {
            Console.WriteLine("Outputting the first 2 payments");
            ListResponse<PaymentResponse> paymentList = mollieClient.GetPaymentListAsync(0, 2).Result;
            foreach (PaymentResponse paymentResponse in paymentList.Data) {
                Console.WriteLine($"Payment Id: { paymentResponse.Id } - Payment method: { paymentResponse.Method }");
            }
        }

        static void OutputPaymentMethods(MollieClient mollieClient) {
            Console.WriteLine("Ouputting all payment methods");
            ListResponse<PaymentMethodResponse> paymentMethodList = mollieClient.GetPaymentMethodListAsync(0, 100).Result;
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
