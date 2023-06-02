using Mollie.Api.Client;
using xRetry;

namespace Mollie.Tests.Integration.Framework; 

public class DefaultRetryFactAttribute : RetryFactAttribute {
    public DefaultRetryFactAttribute(int maxRetries = 1, int delayBetweenRetriesMs = 20000)
        : base(maxRetries, delayBetweenRetriesMs, typeof(MollieApiException)) {
    }
}

public class DefaultRetryTheoryAttribute : RetryTheoryAttribute {
    public DefaultRetryTheoryAttribute(int maxRetries = 1, int delayBetweenRetriesMs = 20000)
        : base(maxRetries, delayBetweenRetriesMs, typeof(MollieApiException)) {
    }
}