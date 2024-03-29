using System;

namespace Mollie.Api.Client.Abstract
{
    public interface IBaseMollieClient : IDisposable
    {
        IDisposable WithIdempotencyKey(string value);
    }
}