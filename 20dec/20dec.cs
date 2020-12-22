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

        public static List<string> Rotate2DString(List<string> input)
        {
            List<string> ret = new List<string>();
            int size = input.Count();
            for (int row = 0; row < size; ++row)
            {
                var newCol = "";
                for (int col = 0; col < size; ++col)
                {
                    newCol += input[size - col - 1][row];
                }
                ret.Add(newCol);
            }
            return ret;
        }

        public static List<string> Rotate2DString(List<string> input, int degrees)
        {
            // degrees can only be 90, 180, 270
            var newResult = Helper.Rotate2DString(input);
            degrees -= 90;
            while (degrees != 0)
            {
                newResult = Helper.Rotate2DString(newResult);
                degrees -= 90;
            }
            return newResult;
        }

        public static List<string> Reverse2DString(List<string> input)
        {
            List<string> ret = new List<string>();
            foreach (var row in input)
            {
                var newRow = Helper.ReverseString(row);
                ret.Add(newRow);
            }
            return ret;
        }
    }

    public class December20 : AdventOfCode
    {
        public December20() : base(20)
        {
        }

        public enum Edge { Top, TopReverse, Left, LeftReverse, Bottom, BottomReverse, Right, RightReverse }

        public class Tile
        {
            public int Id { get; private set; }
            public List<string> Data { get; private set; }

            public bool IsPlacedOnGrid = false;

            public IEnumerable<string> Edges => Enum
                .GetValues(typeof(Edge))
                .Cast<Edge>()
                .Select(n => GetEdgeString(n));

            public IEnumerable<(string, string)> EdgesWithSide => Enum
                .GetValues(typeof(Edge))
                .Cast<Edge>()
                .Select(n => (n.ToString(), GetEdgeString(n)));

            // Is filled during processing
            public bool IsCorner => NrAdjacentTiles == 2;
            public bool IsEdge => NrAdjacentTiles == 3;
            public int NrAdjacentTiles = 0;

            public Tile(int id, List<string> data)
            {
                Id = id;
                Data = data;
            }

            public void Rotate()
            {
                Data = Helper.Rotate2DString(Data);
            }

            public void FlipHorizontal()
            {
                Data = Helper.Rotate2DString(Data);
                Data = Helper.Rotate2DString(Data);
                Data = Helper.Reverse2DString(Data);
            }

            public void FlipVertical()
            {
                Data = Helper.Reverse2DString(Data);
            }

            public void SetAdjacents(IEnumerable<Tile> tiles)
            {
                // REMEMBER to not to check against yourself
                NrAdjacentTiles = 0;
                foreach (var tile in tiles)
                {
                    if (tile.Id != Id)
                    {
                        foreach (var edge in Edges)
                        {
                            var foundEdge = tile.Edges.FirstOrDefault(e => edge == e);
                            if (foundEdge != null)
                            {
                                NrAdjacentTiles++;
                                break;
                            }
                        }
                    }
                }
            }

            // Get edge of a specific direction!
            public string GetEdgeString(Edge edge)
            {
                int SIZE = Data.Count();
                switch (edge)
                {
                    case Edge.Top:
                        return new string(Data[0].ToArray());
                    case Edge.TopReverse:
                        return new string(Data[0].Reverse().ToArray());
                    case Edge.Right:
                        return new string(Data.Select(s => s[SIZE - 1]).ToArray());
                    case Edge.RightReverse:
                        return new string(Data.Select(s => s[SIZE - 1]).Reverse().ToArray());
                    case Edge.Bottom:
                        return new string(Data[SIZE - 1].ToArray());
                    case Edge.BottomReverse:
                        return new string(Data[SIZE - 1].Reverse().ToArray());
                    case Edge.Left:
                        return new string(Data.Select(s => s[0]).ToArray());
                    case Edge.LeftReverse:
                        return new string(Data.Select(s => s[0]).Reverse().ToArray());
                    default:
                        return null;
                }
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

            public void ConstructImage()
            {
                // square tiles
                var gridSize = (int)Math.Sqrt(AllTiles.Count());

                foreach (var tile in AllTiles)
                {
                    tile.SetAdjacents(AllTiles);
                }

                var corners = AllTiles.Where(tile => tile.IsCorner);

                // Get all edges and store each tile that belongs to that edge!
                var tileByEdges = new Dictionary<string, List<Tile>>(
                    AllTiles.SelectMany(tile => tile.Edges.Select(edge => new { edge, tile }))
                    .GroupBy(val => val.edge)
                    .Where(val => val.Count() > 1) // only interested in edges that has more than one connection!
                    .Select(val => new KeyValuePair<string, List<Tile>>(val.Key, val.Select(i => i.tile).ToList()))
                );

                // a new image constructed in an array!
                var newImage = new Tile[gridSize, gridSize];

                for (int row = 0; row < gridSize; row++)
                {
                    for (int col = 0; col < gridSize; col++)
                    {
                        Tile tileToAdd;
                        if (col == 0 && row == 0)
                        {
                            tileToAdd = corners.First();
                            while (!tileByEdges.ContainsKey(tileToAdd.GetEdgeString(Edge.Right)) || !tileByEdges.ContainsKey(tileToAdd.GetEdgeString(Edge.Bottom)))
                            {
                                tileToAdd.Rotate();
                            }
                        }
                        else if (row == 0)
                        {
                            tileToAdd = FindTileByDirection(tileByEdges, newImage, Edge.Right, Edge.Left, Edge.LeftReverse, row, col - 1);
                        }
                        else
                        {
                            tileToAdd = FindTileByDirection(tileByEdges, newImage, Edge.Bottom, Edge.Top, Edge.TopReverse, row - 1, col);
                        }

                        newImage[row, col] = tileToAdd;
                        tileToAdd.IsPlacedOnGrid = true;
                        tileToAdd.Print();
                    }
                }

                for (int row = 0; row < gridSize; row++)
                {
                    for (int col = 0; col < gridSize; col++)
                    {
                        Console.Write(newImage[row, col].Id + " ");
                    }
                    Console.WriteLine();
                }
            }

            private static Tile FindTileByDirection(Dictionary<string, List<Tile>> tileByEdges, Tile[,] newImage, Edge edgeConnectTo, Edge edgeNew, Edge edgeNewReverse, int row, int col)
            {
                Tile tileToAdd;
                var lastTile = newImage[row, col];
                var edgeToConnectTo = lastTile.GetEdgeString(edgeConnectTo);
                // get the tile by the edge that is not place
                tileToAdd = tileByEdges[edgeToConnectTo].FirstOrDefault(tile => !tile.IsPlacedOnGrid);
                while (tileToAdd.GetEdgeString(edgeNew) != edgeToConnectTo && tileToAdd.GetEdgeString(edgeNewReverse) != edgeToConnectTo)
                {
                    tileToAdd.Rotate();
                }
                // do we need to reverse?
                if (tileToAdd.GetEdgeString(edgeNew) != edgeToConnectTo)
                {
                    tileToAdd.FlipHorizontal();
                }

                return tileToAdd;
            }

            public long FindCornerProduct()
            {
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
            var prod = image.FindCornerProduct();
            bool testSucceeded = prod == 20899048083289;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var image = new Image(input);
            var prod = image.FindCornerProduct();
            return prod.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var image = new Image(input);
            image.ConstructImage();

            // foreach (var item in image.AllTiles)
            // {
            //     item.Print();
            // }

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
