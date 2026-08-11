using System;

namespace Mollie.Api.Extensions {
    internal static class DateTimeExtensions {
        public static DateTimeOffset Truncate(this DateTimeOffset dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) {
                return dateTime;
            }

            if (dateTime == DateTimeOffset.MinValue || dateTime == DateTimeOffset.MaxValue) {
                return dateTime;
            }

            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }
    }
}
