using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December24 : AdventOfCode
    {
        public December24() : base(24)
        {
        }

        public record Tile(int x, int y)
        {
            public bool White { get; set; } = true;
            public void Flip()
            {
                White = !White;
            }
            public string Index => $"{x},{y}";
        }

        public class LobbyLayout
        {
            public List<string> TilesToFlip { get; set; }

            public Dictionary<string, Tile> TilesVisited { get; set; }

            public LobbyLayout(List<string> input)
            {
                TilesToFlip = input;
                TilesVisited = new Dictionary<string, Tile>();
            }

            public List<Tile> AdjacentTiles(Tile tile)
            {
                // directions of neighbour
                // e = y: same, x: +1
                // se = y: -1, x: +1
                // sw = y: -1, x: same
                // w = y: same, x: -1
                // nw = y: 1, x: -1
                // ne = y: 1, x: same

                return new List<Tile>();
            }

            public void Run()
            {
                // convert the steps to a x and y position
                // https://gamedev.stackexchange.com/a/44814
                foreach (var tilepath in TilesToFlip)
                {
                    int x = 0;
                    int y = 0;
                    var pathToTile = tilepath;

                    while (pathToTile.Length != 0)
                    {
                        var indexToRemove = 1;
                        if (pathToTile.StartsWith("e"))
                        {
                            // e = y: same, x: +1
                            x++;
                        }
                        else if (pathToTile.StartsWith("se"))
                        {
                            // se = y: -1, x: +1
                            y--;
                            x++;
                            indexToRemove = 2;
                        }
                        else if (pathToTile.StartsWith("sw"))
                        {
                            // sw = y: -1, x: same
                            y--;
                            indexToRemove = 2;
                        }
                        else if (pathToTile.StartsWith("w"))
                        {
                            // w = y: same, x: -1
                            x--;
                        }
                        else if (pathToTile.StartsWith("nw"))
                        {
                            // nw = y: 1, x: -1
                            y++;
                            x--;
                            indexToRemove = 2;
                        }
                        else if (pathToTile.StartsWith("ne"))
                        {
                            // ne = y: 1, x: same
                            y++;
                            indexToRemove = 2;
                        }

                        pathToTile = pathToTile.Substring(indexToRemove);
                    }

                    // tile to flip!
                    var tile = new Tile(x, y);
                    if (!TilesVisited.ContainsKey(tile.Index))
                    {
                        TilesVisited[tile.Index] = tile;
                    }

                    var visitedTile = TilesVisited[tile.Index];
                    visitedTile.Flip();
                }
            }

            public int CountBlackTiles()
            {
                int sum = TilesVisited.Where((item) => !item.Value.White).Count();
                return sum;
            }
        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var lobby = new LobbyLayout(input);
            lobby.Run();
            var res = lobby.CountBlackTiles();
            bool testSucceeded = res == 10;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var lobby = new LobbyLayout(input);
            lobby.Run();
            var res = lobby.CountBlackTiles();
            return res.ToString();
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
