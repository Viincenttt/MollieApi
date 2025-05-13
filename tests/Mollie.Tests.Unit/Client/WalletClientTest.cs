using System;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Mollie.Api.Client;
using Mollie.Api.Models.Wallet.Request;
using Xunit;

namespace Mollie.Tests.Unit.Client;

public class WalletClientTest : BaseClientTests {
    private const string defaultApplePayPaymentSessionResponse = @"{
        ""epochTimestamp"": 1555507053169,
        ""expiresAt"": 1555510653169,
        ""merchantSessionIdentifier"": ""SSH2EAF8AFAEAA94DEEA898162A5DAFD36E_916523AAED1343F5BC5815E12BEE9250AFFDC1A17C46B0DE5A943F0F94927C24"",
        ""nonce"": ""0206b8db"",
        ""merchantIdentifier"": ""BD62FEB196874511C22DB28A9E14A89E3534C93194F73EA417EC566368D391EB"",
        ""domainName"": ""pay.example.org"",
        ""displayName"": ""Chuck Norris's Store"",
        ""signature"": ""308006092a864886f7...8cc030ad3000000000000""
    }";

    [Fact]
    public async Task RequestApplePayPaymentSessionAsync_ResponseIsDeserializedInExpectedFormat() {
        // Arrange
        var request = new ApplePayPaymentSessionRequest() {
            Domain = "pay.mywebshop.com",
            ValidationUrl = "https://apple-pay-gateway-cert.apple.com/paymentservices/paymentSession"
        };
        var mockHttp = CreateMockHttpMessageHandler(
            HttpMethod.Post,
            $"{BaseMollieClient.DefaultBaseApiEndPoint}wallets/applepay/sessions",
            defaultApplePayPaymentSessionResponse);
        using var walletClient = new WalletClient("abcde", mockHttp.ToHttpClient());

        // Act
        var response = await walletClient.RequestApplePayPaymentSessionAsync(request);

        // Assert
        response.EpochTimestamp.ShouldBe(DateTimeOffset.FromUnixTimeMilliseconds(1555507053169).UtcDateTime);
        response.ExpiresAt.ShouldBe(DateTimeOffset.FromUnixTimeMilliseconds(1555510653169).UtcDateTime);
        response.MerchantSessionIdentifier.ShouldBe("SSH2EAF8AFAEAA94DEEA898162A5DAFD36E_916523AAED1343F5BC5815E12BEE9250AFFDC1A17C46B0DE5A943F0F94927C24");
        response.Nonce.ShouldBe("0206b8db");
        response.MerchantIdentifier.ShouldBe("BD62FEB196874511C22DB28A9E14A89E3534C93194F73EA417EC566368D391EB");
        response.DomainName.ShouldBe("pay.example.org");
        response.DisplayName.ShouldBe("Chuck Norris's Store");
        response.Signature.ShouldBe("308006092a864886f7...8cc030ad3000000000000");
    }
}
