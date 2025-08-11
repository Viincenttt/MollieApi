using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Mollie.Api.AspNet.Webhooks.Options;

namespace Mollie.Api.AspNet.Webhooks.Authorization;

public class MollieSignatureValidator
{
    private readonly MollieWebhookOptions _options;

    public MollieSignatureValidator(MollieWebhookOptions options) {
        _options = options;

        if (options == null) {
            throw new ArgumentNullException(nameof(options));
        }

        if (string.IsNullOrWhiteSpace(options.Secret)) {
            throw new ArgumentException("Webhook signing secret cannot be null or empty.", nameof(MollieWebhookOptions.Secret));
        }
    }

    /// <summary>
    /// Validates the Mollie HMAC webhook signature in the given HTTP request.
    /// </summary>
    /// <param name="request">Incoming HTTP request.</param>
    /// <returns>True if the signature is valid; otherwise false.</returns>
    public async Task<bool> Validate(HttpRequest request) {
        if (!request.Headers.TryGetValue("X-Mollie-Signature", out var headerValues))
        {
            return false;
        }

        var headerValue = headerValues.FirstOrDefault();
        if (string.IsNullOrWhiteSpace(headerValue))
        {
            return false;
        }

        var bodyBytes = await GetRequestBodyBytes(request);
        var isValid = ValidateHmacSignature(headerValue, bodyBytes, _options.Secret);

        return isValid;
    }

    private bool ValidateHmacSignature(string headerValue, byte[] bodyBytes, string secret)
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
        for (int i = 0; i < len; i++) {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }
        return bytes;
    }

    private async Task<byte[]> GetRequestBodyBytes(HttpRequest request)
    {
        request.EnableBuffering();

        request.Body.Position = 0;
        using var ms = new MemoryStream();

        await request.Body.CopyToAsync(ms);
        var bodyBytes = ms.ToArray();
        request.Body.Seek(0, SeekOrigin.Begin);

        return bodyBytes;
    }
}
