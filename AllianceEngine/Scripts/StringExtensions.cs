using System.Numerics;

namespace AllianceEngine
{
    public static class StringExtensions
    {
        public static string ToCapital(this string s) => s[0].ToString().ToUpper() + s[1..];
    }
}
