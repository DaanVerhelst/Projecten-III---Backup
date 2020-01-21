using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KolveniershofAPI.Model.Extensions
{
    public static class DateExtensions{
        public static DateTime[] GeefWeek(this DateTime dt) {
            int startDiff = DayOfWeek.Monday - dt.DayOfWeek;
            int endDiff = DayOfWeek.Friday - dt.DayOfWeek;

            //Get a start and the end date (monday, friday)
            DateTime monday = dt.AddDays(startDiff).Date;
            DateTime friday = dt.AddDays(endDiff).Date;

            //Get all dates in between

            return Enumerable.Range(0, 1 + friday.Subtract(monday).Days)
          .Select(offset => monday.AddDays(offset))
          .ToArray();
        }
    }
}
