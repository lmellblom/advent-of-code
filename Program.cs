using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    class Program
    {
        static void Main(string[] args)
        {
            var puzzles = new List<AdventOfCode>() 
            {
                new December1(),
                new December2(),
                new December3(),
                new December4(),
                new December5(),
                new December6(),
                new December7(),
                new December8(),
                new December9(),
                new December10(),
                new December11(),
                new December12(),
                new December13(),
                new December14(),
                new December15(),
                new December16(),
                new December17(),
                new December18()
            };

            DateTime thisDay = DateTime.Today;
            int day = thisDay.Day;

            // previous days, only print results
            for (int i = 0; i < day - 1; i++)
            {
                puzzles[i].RunOnlyResult();
            }

            // run this day
            puzzles[day - 1].Run();
        }
    }
}
