using System;
using System.Collections.Generic;
using System.Text;

namespace YoutubeCenter.Library
{
    internal static class Extension
    {
        public static T Clamp<T>(this T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }

        public static void NullCheck(this object obj, string caller)
        {
            if (obj == null || (obj is string s && string.IsNullOrEmpty(s)))
                throw new NullReferenceException(caller);
        }
    }
}
