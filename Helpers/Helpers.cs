using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace adventOfCode
{
    public static class Helpers
    {
        public static int ManhattanDistance(int x1, int x2, int y1, int y2)
        {
            var sum1 = x1 > x2 ? x1 - x2 : x2 - x1;
            var sum2 = y1 > y2 ? y1 - y2 : y2 - y1;
            return Math.Abs(sum1) + Math.Abs(sum2);
        }

        public static int ManhattanDistance(int x1, int x2)
        {
            return Math.Abs(x1) + Math.Abs(x2);
        }
    }
}