﻿using System;

namespace WcfServiceApp.Extentions
{
    public static class SQLExtentions
    {
        public static bool IsValidSqlDateTime(DateTime? dateTime)
        {
            if (dateTime == null) return true;

            DateTime minValue = (DateTime)System.Data.SqlTypes.SqlDateTime.MinValue;
            DateTime maxValue = (DateTime)System.Data.SqlTypes.SqlDateTime.MaxValue;

            if (minValue > dateTime.Value || maxValue < dateTime.Value)
                return false;

            return true;
        }

        public static string ToStringInstance(this object obj)
        {
            if (obj == null)
            { return string.Empty; }

            return obj.ToString();
        }

        public static bool HasValue(this string str)
        {
            return string.IsNullOrWhiteSpace(str) == false;
        }
    }
}