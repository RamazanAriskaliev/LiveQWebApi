using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiveQ.Api.Helpers
{
    public class DateConverter
    {
        public static DateTime ToDateTime(string timestamp)
        {
            long _timestamp = long.Parse(timestamp);
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(_timestamp).ToLocalTime();
            return dateTime;
        }
        public static string ToUnixTime(DateTime datetime)
        {
            DateTime sTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
            long result = (long)(datetime - sTime).TotalSeconds;
            return result.ToString();
        }
    }
}
