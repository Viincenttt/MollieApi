using System.Security.Cryptography;
using System.Text;

namespace Mollie.Api.AspNet.Webhooks.Authorization;

public class MollieSignatureValidator
{
    public bool Validate(string headerValue, byte[] bodyBytes, string secret)
    {
        if (string.IsNullOrWhiteSpace(headerValue) || string.IsNullOrWhiteSpace(secret)) {
            return false;
        }

        // Step 1: Remove optional prefix "sha256=" (case-insensitive)
        const string prefix = "sha256=";
        var sig = headerValue.Trim();
        if (sig.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)) {
            sig = sig.Substring(prefix.Length);
        }

        // Step 2: parse header signature bytes
        byte[] signatureBytes;
        try
        {
            signatureBytes = HexToBytes(sig);
        }
        catch
        {
            return false;
        }

        // Step 3: compute HMAC-SHA256 over raw body bytes (secret as UTF8)
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var computed = hmac.ComputeHash(bodyBytes);

        // Step 4: timing-safe compare
        if (computed.Length != signatureBytes.Length) {
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(computed, signatureBytes);
    }

    private static byte[] HexToBytes(string hex)
    {
        var len = hex.Length / 2;
        var bytes = new byte[len];
        for (int i = 0; i < len; i++)
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        return bytes;
    }
}
