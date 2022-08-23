using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Extensions
{
    public static class DateFormatExtension
    {
        public static string FullDateAndTimeStringWithUnderScore(this DateTime dateTime)
        {
            return $"{dateTime.Year}_{dateTime.Month}_{dateTime.Day}_{dateTime.Hour}_{dateTime.Minute}_{dateTime.Second}_{dateTime.Millisecond}";
        }
    }
}
