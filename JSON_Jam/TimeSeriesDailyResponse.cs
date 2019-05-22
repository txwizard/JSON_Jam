﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSON_Jam
{
    //  ------------------------------------------------------------------------
    //  This class was generated by a tool.
    //
    //  It was created by using the Paste Special tool on the Edit menu of the
    //  Visual Studio 2017 code editor to generate a class from a JSON string.
    //  Apart from adding this flower box, the only edit was the substitution of
    //  Rootobject with TimeSeriesDailyResponse, since we cannot afford to have
    //  all such objects in a namespace called Rootobject. (There is already one
    //  other object that was generated in this way in StockTickerSparklines.
    //  ------------------------------------------------------------------------

    public class TimeSeriesDailyResponse
    {
        public Meta_Data Meta_Data
        {
            get; set;
        }
        public Time_Series_Daily [ ] Time_Series_Daily
        {
            get; set;
        }
    }

    public class Meta_Data
    {
        public string Information
        {
            get; set;
        }
        public string Symbol
        {
            get; set;
        }
        public string LastRefreshed
        {
            get; set;
        }
        public string OutputSize
        {
            get; set;
        }
        public string TimeZone
        {
            get; set;
        }
    }

    public class Time_Series_Daily
    {
        public string Activity_Date
        {
            get; set;
        }
        public string Open
        {
            get; set;
        }
        public string High
        {
            get; set;
        }
        public string Low
        {
            get; set;
        }
        public string Close
        {
            get; set;
        }
        public string AdjustedClose
        {
            get; set;
        }
        public string Volume
        {
            get; set;
        }
        public string DividendAmount
        {
            get; set;
        }
        public string SplitCoefficient
        {
            get; set;
        }
    }   // public class TimeSeriesDailyResponse
}   // partial namespace JSON_Jam