using System;
using System.Drawing;
using System.Numerics;

namespace AllianceEngine
{
    static class UtilExtensions
    {
        public static Vector4 ToVector4(this Color c)
        {
            return new Vector4(c.R, c.G, c.B, c.A);
        }
    }
}
