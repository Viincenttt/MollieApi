using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
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
        var mockHttp = this.CreateMockHttpMessageHandler(
            HttpMethod.Post, 
            $"{BaseMollieClient.ApiEndPoint}wallets/applepay/sessions", 
            defaultApplePayPaymentSessionResponse);
        var walletClient = new WalletClient("abcde", mockHttp.ToHttpClient());

        // Act
        var response = await walletClient.RequestApplePayPaymentSessionAsync(request);

        // Assert
        response.EpochTimestamp.Should().Be(1555507053169);
        response.ExpiresAt.Should().Be(1555510653169);
        response.MerchantSessionIdentifier.Should().Be("SSH2EAF8AFAEAA94DEEA898162A5DAFD36E_916523AAED1343F5BC5815E12BEE9250AFFDC1A17C46B0DE5A943F0F94927C24");
        response.Nonce.Should().Be("0206b8db");
        response.MerchantIdentifier.Should().Be("BD62FEB196874511C22DB28A9E14A89E3534C93194F73EA417EC566368D391EB");
        response.DomainName.Should().Be("pay.example.org");
        response.DisplayName.Should().Be("Chuck Norris's Store");
        response.Signature.Should().Be("308006092a864886f7...8cc030ad3000000000000");
    }
}