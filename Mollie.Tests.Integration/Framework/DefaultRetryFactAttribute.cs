using Mollie.Api.Client;
using xRetry;

namespace Mollie.Tests.Integration.Framework; 

public class DefaultRetryFactAttribute : RetryFactAttribute {
    public DefaultRetryFactAttribute(int maxRetries = 2, int delayBetweenRetriesMs = 10000)
        : base(maxRetries, delayBetweenRetriesMs) {
    }
}

public class DefaultRetryTheoryAttribute : RetryTheoryAttribute {
    public DefaultRetryTheoryAttribute(int maxRetries = 2, int delayBetweenRetriesMs = 10000)
        : base(maxRetries, delayBetweenRetriesMs) {
    }
}