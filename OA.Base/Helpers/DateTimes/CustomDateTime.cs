using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using OA.Base.Enums;

namespace OA.Base.Helpers.DateTimes
{
    public class CustomDateTime : ICustomDateTime
    {
        public DateTime GetCurrentDateTime(int hour)
        {
            return DateTime.UtcNow.AddHours(hour);
        }
        public DateTime CustomDateTimeFormat(int hour)
        {
            return DateTime.ParseExact(string.Format("{0:yyyy-MM-dd hh: mm:ss t}", DateTime.UtcNow.AddHours(hour)), "yyyy-MM-dd hh: mm:ss t", null);
        }
        public DateTime GetLastDateOfDayOfWeek(DateTime currentDate)
        {
            DayOfWeek CurrentDayOfWeek = currentDate.DayOfWeek;
            return currentDate.AddDays(5 - (int)CurrentDayOfWeek);
        }
    }
}
