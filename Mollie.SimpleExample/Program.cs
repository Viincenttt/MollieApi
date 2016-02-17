using System;
using Mollie.Api.Client;
using Mollie.Api.Models.List;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Mollie.Api.Models.PaymentMethod;

namespace Mollie.SimpleExample {
    class Program {
        static void Main(string[] args) {
            MollieApi mollieApi = new MollieApi("{API key}");

            Console.WriteLine("Press any key to create a new payment");
            Console.ReadKey();

            OutputNewPayment(mollieApi);

            Console.WriteLine("Press any key to retrieve a list of payments");
            Console.ReadKey();

            OutputPaymentList(mollieApi);

            Console.WriteLine("Press any key to retrieve a list of payment methods");
            Console.ReadKey();

            OutputPaymentMethods(mollieApi);

            Console.Read();
        }

        static void OutputNewPayment(MollieApi mollieApi) {
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

        static void OutputPaymentList(MollieApi mollieApi) {
            Console.WriteLine("Outputting the first 2 payments");
            ListResponse<PaymentResponse> paymentList = mollieApi.GetPaymentList(0, 2).Result;
            foreach (PaymentResponse paymentResponse in paymentList.Data) {
                Console.WriteLine($"Payment Id: { paymentResponse.Id } - Payment method: { paymentResponse.Method }");
            }
        }

        static void OutputPaymentMethods(MollieApi mollieApi) {
            Console.WriteLine("Ouputting all payment methods");
            ListResponse<PaymentMethodResponse> paymentMethodList = mollieApi.GetPaymentMethodList(0, 100).Result;
            foreach (PaymentMethodResponse paymentMethodResponse in paymentMethodList.Data) {
                Console.WriteLine($"Payment method description: { paymentMethodResponse.Description }");
            }
        }
    }
}
