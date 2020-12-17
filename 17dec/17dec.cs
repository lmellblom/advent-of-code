using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December17 : AdventOfCode
    {
        public December17() : base(17)
        {
        }

        public class PocketDimension
        {
            public List<Cube> Cubes { get; set; }
            private List<Cube> CubesToAdd { get; set; }
            public PocketDimension(List<string> input)
            {
                Cubes = new List<Cube>();
                CubesToAdd = new List<Cube>();
                int z = 0;
                for (int x = 0; x < input.Count(); x++)
                {
                    var row = input[x];
                    for (int y = 0; y < row.Count(); y++)
                    {
                        var value = row[y];
                        bool isActive = value == '#'; // . == false
                        Cubes.Add(new Cube(x, y, z, isActive));
                    }
                }

                foreach (var cube in Cubes)
                {
                    var cubeNeighbours = Neighbors(cube);
                }

                // add missing neigbours to Cubes
                Cubes.AddRange(CubesToAdd);

                // clear neigbours
                CubesToAdd = new List<Cube>();
            }

            public int NrOfActiveCubes()
            {
                return Cubes.Where(c => c.IsActive).Count();
            }

            public void BootProcess(int cycles = 6, bool print = true)
            {
                Print();

                for (int cycle = 0; cycle < cycles; cycle++)
                {
                    Console.WriteLine($"Cycle = {cycle + 1}");
                    foreach (var cube in Cubes)
                    {
                        var cubeNeighbours = Neighbors(cube);
                        int activeNeighbours = cubeNeighbours.Where(c => c.IsActive).Count();

                        // get the next state
                        cube.SetNextState(activeNeighbours);
                    }

                    // execute flip
                    foreach (var cube in Cubes)
                    {
                        cube.ExecuteSwap();
                    }

                    // end of round
                    Print();

                    // add missing neigbours to Cubes
                    Cubes.AddRange(CubesToAdd);

                    // clear neigbours
                    CubesToAdd = new List<Cube>();



                }
            }

            private bool CubeExists(int xIndex, int yIndex, int zIndex)
            {
                return CubeExists(xIndex, yIndex, zIndex, Cubes);
            }

            private bool CubeExists(int xIndex, int yIndex, int zIndex, List<Cube> cubes)
            {
                if (cubes.Any())
                {
                    var (x, y, z) = GetRange(cubes);
                    return x.Contains(xIndex) && y.Contains(yIndex) && z.Contains(zIndex);
                }
                return false;
            }

            // public List<Cube> Neighbors(int xIndex, int yIndex, int zIndex)
            private List<Cube> Neighbors(Cube cube)
            {
                List<Cube> output = new List<Cube>();

                int xIndex = cube.X;
                int yIndex = cube.Y;
                int zIndex = cube.Z;

                // -1, 0, 1
                List<int> xs = Enumerable.Range(-1, 3).Select(nr => nr + xIndex).ToList();
                List<int> ys = Enumerable.Range(-1, 3).Select(nr => nr + yIndex).ToList();
                List<int> zs = Enumerable.Range(-1, 3).Select(nr => nr + zIndex).ToList();

                foreach (var x in xs)
                {
                    foreach (var y in ys)
                    {
                        foreach (var z in zs)
                        {
                            if (x == xIndex && y == yIndex && z == zIndex)
                            {
                                // this is the cube we have
                                continue;
                            }

                            // check if out of range?
                            var neigbour = GetCube(x, y, z);
                            if (neigbour == null)
                            {
                                // out of range, add a new cube with inactive state
                                var cubeState = false;
                                var newCube = new Cube(x, y, z, cubeState);

                                var cubenotfound = GetCube(x, y, z, CubesToAdd) == null;
                                if (cubenotfound)
                                {
                                    CubesToAdd.Add(newCube);
                                }
                                output.Add(newCube);
                            }
                            else
                            {
                                output.Add(neigbour);
                            }
                        }
                    }
                }

                if (output.Count() != 26)
                {
                    Console.WriteLine("ERROR! neigbour count is wrong");
                }

                return output;
            }

            private (IEnumerable<int> xRange, IEnumerable<int> yRange, IEnumerable<int> zRange) GetRange(List<Cube> cubes)
            {
                var x = cubes.Select(cube => cube.X).Distinct();
                var xValues = GetRange(x.Min(), x.Max());
                var y = cubes.Select(cube => cube.Y).Distinct();
                var yValues = GetRange(y.Min(), y.Max());
                var z = cubes.Select(cube => cube.Z).Distinct();
                var zValues = GetRange(z.Min(), z.Max());
                return (xValues, yValues, zValues);
            }

            private IEnumerable<int> GetRange(int min, int max)
            {
                return Enumerable.Range(min, max - min + 1);
            }

            private Cube GetCube(int x, int y, int z)
            {
                return GetCube(x, y, z, Cubes);
            }

            private Cube GetCube(int x, int y, int z, List<Cube> cubes)
            {
                return cubes.FirstOrDefault(cube => cube.X == x && cube.Y == y && cube.Z == z);
            }

            public void Print()
            {
                // get all dimensions
                var (x, y, z) = GetRange(Cubes);

                foreach (var zIndex in z)
                {
                    Console.WriteLine($"z={zIndex}");
                    foreach (var xIndex in x)
                    {
                        foreach (var yIndex in y)
                        {
                            var cube = GetCube(xIndex, yIndex, zIndex);
                            if (cube != null)
                            {
                                var value = cube.Print();
                                Console.Write(value);
                            }
                            else
                            {
                                Console.Write(".");
                            }
                        }

                        Console.WriteLine("");
                    }
                }
                Console.WriteLine("");
            }
        }

        public class Cube
        {
            public bool IsActive { get; set; } // # == active, . == inactive
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public bool? SwapTo { get; set; }

            public string Print()
            {
                if (IsActive)
                {
                    return "#";
                }
                else
                {
                    return ".";
                }
            }

            public void SetNextState(int activeNeighbours)
            {
                if (IsActive)
                {
                    // If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active. 
                    // Otherwise, the cube becomes inactive.
                    if (activeNeighbours == 2 || activeNeighbours == 3)
                    {
                        // SwapTo = true;
                    }
                    else
                    {
                        SwapTo = false;
                    }
                }
                else
                {
                    // If a cube is inactive but exactly 3 of its neighbors are active, the cube becomes active. 
                    // Otherwise, the cube remains inactive.
                    if (activeNeighbours == 3)
                    {
                        SwapTo = true;
                    }
                    else
                    {
                        // SwapTo = false;
                    }
                }
            }

            public void ExecuteSwap()
            {
                if (SwapTo.HasValue)
                {
                    IsActive = SwapTo.Value;
                    SwapTo = null;
                }
            }

            public Cube(int x, int y, int z, bool isActive = false)
            {
                X = x;
                Y = y;
                Z = z;
                IsActive = isActive;
                SwapTo = null; // not!
            }

            // get all neighbours är typ -1 -> 1 i varje dimension, tre foor-lopar
        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();

            var pocketDimension = new PocketDimension(input);
            pocketDimension.BootProcess(6, true);
            int activeCubes = pocketDimension.NrOfActiveCubes();
            bool testSucceeded = activeCubes == 112;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var pocketDimension = new PocketDimension(input);
            pocketDimension.BootProcess(6, true);
            int activeCubes = pocketDimension.NrOfActiveCubes();
            return activeCubes.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            bool testSucceeded = false;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            return "not implemented";
        }
    }
}
