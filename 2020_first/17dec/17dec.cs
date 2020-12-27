using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode
{
    public class December17 : AoCSolver
    {
        public December17() : base(17)
        {
        }

        public class Cube
        {
            public Cube(int ix, int iy, int iz)
            {
                x = ix;
                y = iy;
                z = iz;
                IsActive = false;
            }
            public int x { get; set; }
            public int y { get; set; }
            public int z { get; set; }
            public bool IsActive { get; set; }
            public bool? SwapTo { get; set; }

            public bool SetNextState(int activeNeighbours)
            {
                if (IsActive)
                {
                    // If a cube is active and exactly 2 or 3 of its neighbors are also active, the cube remains active. 
                    // Otherwise, the cube becomes inactive.
                    if (activeNeighbours == 2 || activeNeighbours == 3)
                    {
                        SwapTo = true;
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
                        SwapTo = false;
                    }
                }

                return SwapTo.Value;
            }
            public void ExecuteSwap()
            {
                if (SwapTo.HasValue)
                {
                    IsActive = SwapTo.Value;
                    SwapTo = null;
                }
            }

            public virtual string Index => $"{x},{y},{z}";
        }

        public class Cube4D : Cube
        {
            public Cube4D(int ix, int iy, int iz, int iw) : base(ix, iy, iz)
            {
                w = iw;
            }

            public int w { get; set; }

            public override string Index => $"{x},{y},{z},{w}";
        }

        public class ConwayCubes4D : ConwayCubes
        {
            public ConwayCubes4D(List<string> input) : base()
            {
                Cubes = new Dictionary<string, Cube>();
                NewCubes = new Dictionary<string, Cube>();
                int z = 0; int w = 0;
                for (int x = 0; x < input.Count(); x++)
                {
                    var row = input[x];
                    for (int y = 0; y < row.Count(); y++)
                    {
                        var value = row[y];
                        bool isActive = value == '#'; // . == false

                        if (isActive)
                        {
                            var cube = new Cube4D(x, y, z, w);
                            cube.IsActive = isActive;
                            Cubes.Add(cube.Index, cube);
                        }
                    }
                }
            }

            public override int ActiveNeighbors(Cube cube, bool addIfMissing = true)
            {
                var neighbours = 0;

                int xIndex = cube.x;
                int yIndex = cube.y;
                int zIndex = cube.z;
                int wIndex = (cube as Cube4D).w;

                // -1, 0, 1
                List<int> xs = Enumerable.Range(-1, 3).Select(nr => nr + xIndex).ToList();
                List<int> ys = Enumerable.Range(-1, 3).Select(nr => nr + yIndex).ToList();
                List<int> zs = Enumerable.Range(-1, 3).Select(nr => nr + zIndex).ToList();
                List<int> ws = Enumerable.Range(-1, 3).Select(nr => nr + wIndex).ToList();

                foreach (var x in xs)
                {
                    foreach (var y in ys)
                    {
                        foreach (var z in zs)
                        {
                            foreach (var w in ws)
                            {
                                if (x == xIndex && y == yIndex && z == zIndex && w == wIndex)
                                {
                                    // this is the cube we have
                                    continue;
                                }

                                var index = $"{x},{y},{z},{w}";
                                if (Cubes.ContainsKey(index))
                                {
                                    var neighbour = Cubes[index];
                                    if (neighbour.IsActive)
                                    {
                                        neighbours++;
                                    }
                                }
                                else if (!NewCubes.ContainsKey(index) && addIfMissing)
                                {
                                    var newCube = new Cube4D(x, y, z, w);
                                    newCube.IsActive = false;
                                    NewCubes.Add(newCube.Index, newCube);
                                }
                            }
                        }
                    }
                }

                return neighbours;
            }
        }

        public class ConwayCubes
        {
            public Dictionary<string, Cube> Cubes { get; set; }

            public Dictionary<string, Cube> NewCubes { get; set; }

            public Dictionary<string, Cube> CubesToRemove { get; set; }

            public ConwayCubes()
            {
                NewCubes = new Dictionary<string, Cube>();
                CubesToRemove = new Dictionary<string, Cube>();
            }

            public ConwayCubes(List<string> input)
            {
                Cubes = new Dictionary<string, Cube>();
                NewCubes = new Dictionary<string, Cube>();
                int z = 0;
                for (int x = 0; x < input.Count(); x++)
                {
                    var row = input[x];
                    for (int y = 0; y < row.Count(); y++)
                    {
                        var value = row[y];
                        bool isActive = value == '#'; // . == false

                        if (isActive)
                        {
                            var cube = new Cube(x, y, z);
                            cube.IsActive = isActive;
                            Cubes.Add(cube.Index, cube);
                        }
                    }
                }

                NewCubes = new Dictionary<string, Cube>();
                CubesToRemove = new Dictionary<string, Cube>();
            }

            public virtual int ActiveNeighbors(Cube cube, bool addIfMissing = true)
            {
                var neighbours = 0;

                int xIndex = cube.x;
                int yIndex = cube.y;
                int zIndex = cube.z;

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

                            var index = $"{x},{y},{z}";
                            if (Cubes.ContainsKey(index))
                            {
                                var neighbour = Cubes[index];
                                if (neighbour.IsActive)
                                {
                                    neighbours++;
                                }
                            }
                            else if (!NewCubes.ContainsKey(index) && addIfMissing)
                            {
                                var newCube = new Cube(x, y, z);
                                newCube.IsActive = false;
                                NewCubes.Add(newCube.Index, newCube);
                            }
                        }
                    }
                }

                return neighbours;
            }

            public void BootProcess(int cycles = 6)
            {
                for (int cycle = 0; cycle < cycles; cycle++)
                {
                    foreach (var (ind, cube) in Cubes)
                    {
                        int activeNeighbours = ActiveNeighbors(cube);
                        bool nextState = cube.SetNextState(activeNeighbours);
                        // if the new state of the cube is inactive, then remove
                        if (!nextState)
                        {
                            CubesToRemove.Add(ind, cube);
                        }
                    }

                    // only add if the newCubeState is active
                    foreach (var (ind, cube) in NewCubes)
                    {
                        int activeNeighbours = ActiveNeighbors(cube, false);
                        var nextState = cube.SetNextState(activeNeighbours);
                        if (nextState)
                        {
                            Cubes.Add(ind, cube);
                        }
                    }

                    foreach (var (ind, cube) in CubesToRemove)
                    {
                        Cubes.Remove(ind);
                    }

                    foreach (var (ind, cube) in Cubes)
                    {
                        cube.ExecuteSwap();
                    }

                    NewCubes = new Dictionary<string, Cube>();
                    CubesToRemove = new Dictionary<string, Cube>();
                }
            }

            public int NrActiveCubes()
            {
                int sum = Cubes.Where((item) => item.Value.IsActive).Count();
                return sum;
            }
        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var conwayCubes = new ConwayCubes(input);
            conwayCubes.BootProcess(6);
            int activeCubes = conwayCubes.NrActiveCubes();
            bool testSucceeded = activeCubes == 112;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var conwayCubes = new ConwayCubes(input);
            conwayCubes.BootProcess(6);
            int activeCubes = conwayCubes.NrActiveCubes();
            return activeCubes.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var conwayCubes = new ConwayCubes4D(input);
            conwayCubes.BootProcess(6);
            int activeCubes = conwayCubes.NrActiveCubes();
            bool testSucceeded = activeCubes == 848;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var conwayCubes = new ConwayCubes4D(input);
            conwayCubes.BootProcess(6);
            int activeCubes = conwayCubes.NrActiveCubes();
            return activeCubes.ToString();
        }
    }
}
