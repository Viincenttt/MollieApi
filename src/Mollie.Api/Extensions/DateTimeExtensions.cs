using System;

namespace Mollie.Api.Extensions {
    internal static class DateTimeExtensions {
        public static DateTime Truncate(this DateTime dateTime, TimeSpan timeSpan)
        {
            if (timeSpan == TimeSpan.Zero) {
                return dateTime;
            }

            if (dateTime == DateTime.MinValue || dateTime == DateTime.MaxValue) {
                return dateTime;
            } 
            
            return dateTime.AddTicks(-(dateTime.Ticks % timeSpan.Ticks));
        }
    }
}