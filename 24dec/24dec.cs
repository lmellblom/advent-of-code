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

            public bool? FlipTo { get; set; }

            public void Flip()
            {
                White = !White;
            }

            public void ExecuteFlip()
            {
                if (FlipTo.HasValue)
                {
                    White = FlipTo.Value;
                    FlipTo = null;
                }
            }

            public string Index => $"{x},{y}";
        }

        public class LobbyLayout
        {
            public List<string> TilesToFlip { get; set; }

            public Dictionary<string, Tile> TilesVisited { get; set; }

            public Dictionary<string, Tile> NewTiles { get; set; }

            public LobbyLayout(List<string> input)
            {
                TilesToFlip = input;
                TilesVisited = new Dictionary<string, Tile>();
                NewTiles = new Dictionary<string, Tile>();
            }

            public List<Tile> AdjacentTiles(Tile tile, bool addIfMissing = true)
            {
                // directions of neighbour
                List<(int x, int y)> neighboursIndex = new List<(int x, int y)>()
                {
                    (1, 0), (1, -1), (0, -1), (-1, 0), (-1, 1), (0, 1)
                };

                // e = y: same, x: +1
                // se = y: -1, x: +1
                // sw = y: -1, x: same
                // w = y: same, x: -1
                // nw = y: 1, x: -1
                // ne = y: 1, x: same

                var neighbours = new List<Tile>();
                foreach (var item in neighboursIndex)
                {
                    int x = item.x + tile.x;
                    int y = item.y + tile.y;

                    if (TilesVisited.ContainsKey($"{x},{y}"))
                    {
                        neighbours.Add(TilesVisited[$"{x},{y}"]);
                    }
                    else if (!NewTiles.ContainsKey($"{x},{y}") && addIfMissing)
                    {
                        NewTiles.Add($"{x},{y}", new Tile(x, y));
                    }
                }

                return neighbours;
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

            public void Run2(int days = 10)
            {
                Run();

                // start to execute days of flipping 
                for (int day = 1; day <= days; day++)
                {
                    // Any black tile with zero or more than 2 black tiles immediately adjacent to it is flipped to white.
                    // Any white tile with exactly 2 black tiles immediately adjacent to it is flipped to black.
                    foreach (var (ind, tile) in TilesVisited)
                    {
                        var adjacent = AdjacentTiles(tile);
                        var blackAdjacent = adjacent.Where((item) => !item.White).Count();

                        if (tile.White && blackAdjacent == 2)
                        {
                            // exactly 2 black tiles, flip to black!
                            tile.FlipTo = false;
                        }
                        else if (!tile.White && (blackAdjacent == 0 || blackAdjacent > 2))
                        {
                            tile.FlipTo = true;
                        }
                    }

                    foreach (var (ind, tile) in NewTiles)
                    {
                        TilesVisited.Add(tile.Index, tile);

                        var adjacent = AdjacentTiles(tile, false);
                        var blackAdjacent = adjacent.Where((item) => !item.White).Count();

                        if (tile.White && blackAdjacent == 2)
                        {
                            // exactly 2 black tiles, flip to black!
                            tile.FlipTo = false;
                        }
                        else if (!tile.White && (blackAdjacent == 0 || blackAdjacent > 2))
                        {
                            tile.FlipTo = true;
                        }
                    }

                    foreach (var (ind, tile) in TilesVisited)
                    {
                        tile.ExecuteFlip();
                    }

                    NewTiles = new Dictionary<string, Tile>();
                    var nr = CountBlackTiles();

                    // Console.WriteLine($"Day {day}: {nr}");
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
            var lobby = new LobbyLayout(input);
            lobby.Run2(100);
            var res = lobby.CountBlackTiles();
            bool testSucceeded = res == 2208;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var lobby = new LobbyLayout(input);
            lobby.Run2(100);
            var res = lobby.CountBlackTiles();
            return res.ToString();
        }
    }
}
