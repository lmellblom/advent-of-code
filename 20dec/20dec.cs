using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventOfCode2020
{
    public static class Helper
    {
        public static int GetNumberFromString(string input)
        {
            string mynumber = Regex.Replace(input, @"\D", "");
            return Int32.Parse(mynumber);
        }

        public static string ReverseString(string s)
        {
            // Convert to char array, then call Array.Reverse.
            // ... Finally use string constructor on array.
            char[] array = s.ToCharArray();
            Array.Reverse(array);
            return new string(array);
        }
    }

    public class December20 : AdventOfCode
    {
        public December20() : base(20)
        {
        }

        public class Tile
        {
            public int Id;
            public List<string> Data;
            public List<string> PossibleEdges;

            // Is filled during image processing
            public HashSet<Tile> AdjacentTiles;
            public Dictionary<string, Tile> UsedEdgeAndAdjacentTile; // which edge each tile is neighbour to?
            public bool IsCorner => AdjacentTiles.Count == 2;   // 2 sides is empty
            public bool IsEdge => AdjacentTiles.Count == 3;     // 1 side is empty

            public Tile(int id, List<string> data)
            {
                Id = id;
                Data = data;
                PossibleEdges = GetAllEdges();
                AdjacentTiles = new HashSet<Tile>(); // to store when we find a match!!
                UsedEdgeAndAdjacentTile = new Dictionary<string, Tile>();
            }

            public bool AddAdjacentIfMatch(Tile maybeNeigbour)
            {
                foreach (var edge in PossibleEdges)
                {
                    var foundEdge = maybeNeigbour.PossibleEdges.FirstOrDefault(inputEdge => edge == inputEdge);
                    if (foundEdge != null)
                    {
                        UsedEdgeAndAdjacentTile[foundEdge] = maybeNeigbour;
                        maybeNeigbour.UsedEdgeAndAdjacentTile[foundEdge] = this;

                        AdjacentTiles.Add(maybeNeigbour);
                        maybeNeigbour.AdjacentTiles.Add(this);
                        return true;
                    }
                }

                return false;
            }

            private List<string> GetAllEdges()
            {
                var topEdge = Data[0];
                var bottomEdge = Data[Data.Count() - 1];

                var leftEdge = "";
                var rightEdge = "";
                foreach (var line in Data)
                {
                    leftEdge += line[0];
                    rightEdge += line[Data.Count() - 1];
                }

                var topEdgeRev = Helper.ReverseString(topEdge);
                var bottomEdgeRev = Helper.ReverseString(bottomEdge);
                var leftEdgeRev = Helper.ReverseString(leftEdge);
                var rightEdgeRev = Helper.ReverseString(rightEdge);

                return new List<string>() { topEdge, rightEdge, bottomEdge,
                    leftEdge, topEdgeRev, rightEdgeRev, bottomEdgeRev, leftEdgeRev };
            }

            public void Print()
            {
                Console.WriteLine($"--- TILE: {Id} ---");
                foreach (var line in Data)
                {
                    Console.WriteLine(line);
                }
                Console.WriteLine();
            }
        }

        public class Image
        {
            public List<Tile> AllTiles = new List<Tile>();
            public Image(List<string> input)
            {
                int id = 0;
                var tileInput = new List<string>();
                foreach (var line in input)
                {
                    if (line.StartsWith("Tile"))
                    {
                        id = Helper.GetNumberFromString(line);
                    }
                    else if (String.IsNullOrEmpty(line))
                    {
                        var newTile = new Tile(id, tileInput);
                        AllTiles.Add(newTile);
                        id = 0;
                        tileInput = new List<string>();
                    }
                    else
                    {
                        tileInput.Add(line);
                    }
                }

                if (id != 0)
                {
                    // add last tile!
                    var tileLeft = new Tile(id, tileInput);
                    AllTiles.Add(tileLeft);
                }
            }

            public long ConstructImage()
            {
                foreach (var tile in AllTiles)
                {
                    foreach (var tile2 in AllTiles)
                    {
                        if (tile != tile2)
                        {
                            tile.AddAdjacentIfMatch(tile2);
                        }
                    }
                }

                // Get all corners and the ids
                long prod = 1;
                var cornerTileIds = AllTiles.Where(tile => tile.IsCorner).Select(tile => tile.Id).ToList();
                foreach (var tileId in cornerTileIds)
                {
                    prod *= tileId;
                }

                return prod;
            }
        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var image = new Image(input);
            var prod = image.ConstructImage();
            bool testSucceeded = prod == 20899048083289;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var image = new Image(input);
            var prod = image.ConstructImage();
            return prod.ToString();
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
