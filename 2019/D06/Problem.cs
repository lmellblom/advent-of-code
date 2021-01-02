using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2019.D06
{
    [CodeName("Universal Orbit Map")]
    public class AoC : IAdventOfCodeWithTest
    {

        public Result First(List<string> input)
        {
            var orbit = new Orbit(input);
            var value = orbit.CountOrbits();
            return new Result(value);
        }

        public Result Second(List<string> input)
        {
            var orbit = new Orbit(input);
            var value = orbit.CountOrbitTransfers("YOU", "SAN");
            return new Result(value);
        }

        public TestResult Test(List<string> input)
        {
            var orbit = new Orbit(input);
            var value = orbit.CountOrbits();
            var expected = 54;
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        public TestResult Test2(List<string> input)
        {
            var orbit = new Orbit(input);
            var value = orbit.CountOrbitTransfers("YOU", "SAN");
            var expected = 4;
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        public class Orbit
        {
            Dictionary<string, string> planets = new Dictionary<string, string>();

            public Orbit(List<string> input)
            {
                foreach (var item in input)
                {
                    var splitted = item.Split(")");
                    var orbitsAround = splitted[0];
                    var planet = splitted[1];
                    planets.Add(planet, orbitsAround);
                }
            }

            public int CountOrbits()
            {
                int nrOrbits = 0;
                foreach (var (planet, orbits) in planets)
                {
                    // i is hos many planets planet orbits around
                    int i = 1;
                    var planetToSearch = orbits;
                    while (planets.ContainsKey(planetToSearch))
                    {
                        i++;
                        planetToSearch = planets[planetToSearch];
                    }
                    nrOrbits += i;
                }

                return nrOrbits;
            }

            private List<string> TraverseFromPlanet(string from)
            {
                var fromPath = new List<string>();

                var planetFrom = planets[from];
                fromPath.Add(planetFrom);
                while (planets.ContainsKey(planetFrom))
                {
                    planetFrom = planets[planetFrom];
                    fromPath.Add(planetFrom);
                }

                return fromPath;
            }

            public int CountOrbitTransfers(string from, string to)
            {
                // get the path from the planets
                var fromPath = TraverseFromPlanet(from);
                var toPath = TraverseFromPlanet(to);

                // merge the lists and take unique
                var planet1Count = fromPath.Except(toPath).ToList();
                var planet2Count = toPath.Except(fromPath).ToList();

                return planet1Count.Count() + planet2Count.Count();
            }
        }
    }
}