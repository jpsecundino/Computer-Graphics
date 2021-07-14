using System.Collections.Generic;
using System.Numerics;

namespace AllianceEngine
{
    public static class StringExtensions
    {
        public static string ToCapital(this string s) => s[0].ToString().ToUpper() + s[1..];
        public static bool StartsWithOptimized(this string s, in string comparator)
        {
            if (comparator.Length > s.Length)
                return false;

            for (int i = 0; i < comparator.Length; i++)
            {
                if (s[i] != comparator[i])
                    return false;
            }

            return true;
        }
    }
}
