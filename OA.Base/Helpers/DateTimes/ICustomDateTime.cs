﻿using System;

namespace OA.Base.Helpers.DateTimes
{
    public interface ICustomDateTime
    {
        /// <summary>
        /// This mathod get current date time for sudan
        /// </summary>
        /// <returns></returns>
        DateTime GetCurrentDateTime(int hour);
        /// <summary>
        /// This mathod get custom format date time
        /// </summary>
        /// <returns></returns>
        DateTime CustomDateTimeFormat(int hour);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="valueDayOfWeek"></param>
        /// <param name="keyDayOfWeek"></param>
        /// <returns></returns>
        DateTime GetLastDateOfDayOfWeek(DateTime currentDate);
    }
}