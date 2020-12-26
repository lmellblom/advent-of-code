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
            public IEnumerable<string> Edges => Enum
                .GetValues(typeof(Edge))
                .Cast<Edge>()
                .Select(n => GetEdgeString(n));

            // Is filled during processing
            public bool IsPlacedOnGrid = false;
            public bool IsCorner => NrAdjacentTiles == 2;
            public bool IsEdge => NrAdjacentTiles == 3;
            public int NrAdjacentTiles = 0;
            public int Size => Data.Count();
            public int SizeWithoutBorder => Data.Count() - 2;

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
            public string GetRow(int row)
            {
                return Data[row];
            }

            public void UpdateRow(int row, string data)
            {
                Data[row] = data;
            }
        }

        public class BigImage
        {
            private Tile[,] Tiles { get; set; }
            public Tile Tile { get; set; }
            public BigImage(Tile[,] input, int gridSize)
            {
                Tiles = input;

                var newImage = new List<string>();
                for (int row = 0; row < gridSize; row++)
                {
                    var tiles = new List<Tile>();
                    for (int col = 0; col < gridSize; col++)
                    {
                        tiles.Add(Tiles[row, col]);
                    }

                    var tileRows = tiles.FirstOrDefault().Size;
                    // construct the new image, remove the borders.
                    for (int i = 1; i < tileRows - 1; i++)
                    {
                        var newRow = tiles.Aggregate("", (preTile, tile) =>
                            preTile + tile.GetRow(i).Substring(1, tileRows - 2));

                        newImage.Add(newRow);
                    }
                }
                Tile = new Tile(1, newImage);
            }

            public void LookForSeaMonster()
            {
                // look for seamonsters in each rotation, then flip vertical and look in each rotation
                Queue<Action> tileActions = new Queue<Action>();
                tileActions.Enqueue(Tile.Rotate);
                tileActions.Enqueue(Tile.Rotate);
                tileActions.Enqueue(Tile.Rotate);
                tileActions.Enqueue(Tile.Rotate);
                tileActions.Enqueue(Tile.FlipVertical);
                tileActions.Enqueue(Tile.Rotate);
                tileActions.Enqueue(Tile.Rotate);
                tileActions.Enqueue(Tile.Rotate);

                List<(int rowIndex, List<int> patternIndex)> pattern = new List<(int, List<int>)>();
                pattern.Add((0, new List<int>() { 18 }));
                pattern.Add((1, new List<int>() { 0, 5, 6, 11, 12, 17, 18, 19 }));
                pattern.Add((2, new List<int>() { 1, 4, 7, 10, 13, 16 }));

                bool foundSeaMonster = false;
                while (!foundSeaMonster)
                {
                    var action = tileActions.Dequeue();
                    action();

                    for (int row = 0; row <= Tile.Size - 3; row++)
                    {
                        for (int chars = 0; chars <= Tile.Size - 20; chars++)
                        {
                            var found = new List<bool>();
                            foreach (var p in pattern)
                            {
                                var tileRow = Tile.GetRow(row + p.rowIndex);
                                var res = p.patternIndex.All(i => tileRow[chars + i] == '#');
                                found.Add(res);
                            }

                            if (found.All(v => v == true))
                            {
                                foundSeaMonster = true;

                                // replace with 0
                                foreach (var p in pattern)
                                {
                                    var tileRow = Tile.GetRow(row + p.rowIndex);

                                    char[] newRow = tileRow.ToCharArray();
                                    p.patternIndex.ForEach(i => newRow[chars + i] = '0');
                                    Tile.UpdateRow(row + p.rowIndex, new string(newRow));
                                }
                            }
                        }
                    }
                }
            }

            public int CountWaterRoughness()
            {
                var sum = 0;
                foreach (var row in Tile.Data)
                {
                    sum += row.Count(r => r == '#');
                }
                return sum;
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

                // square tiles
                var gridSize = (int)Math.Sqrt(AllTiles.Count());

                // find connections
                foreach (var tile in AllTiles)
                {
                    tile.SetAdjacents(AllTiles);
                }
            }
            public BigImage ConstructImage()
            {
                // // square tiles
                var gridSize = (int)Math.Sqrt(AllTiles.Count());

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
                            var corners = AllTiles.Where(tile => tile.IsCorner);
                            tileToAdd = corners.First();
                            while (!tileByEdges.ContainsKey(tileToAdd.GetEdgeString(Edge.Right)) || !tileByEdges.ContainsKey(tileToAdd.GetEdgeString(Edge.Bottom)))
                            {
                                tileToAdd.Rotate();
                            }
                        }
                        else if (row == 0)
                        {
                            var lastTile = newImage[row, col - 1];
                            var edgeToConnectTo = lastTile.GetEdgeString(Edge.Right);
                            // get the tile by the edge that is not place
                            tileToAdd = tileByEdges[edgeToConnectTo].FirstOrDefault(tile => !tile.IsPlacedOnGrid);
                            while (tileToAdd.GetEdgeString(Edge.Left) != edgeToConnectTo && tileToAdd.GetEdgeString(Edge.LeftReverse) != edgeToConnectTo)
                            {
                                tileToAdd.Rotate();
                            }
                            // do we need to reverse?
                            if (tileToAdd.GetEdgeString(Edge.Left) != edgeToConnectTo)
                            {
                                tileToAdd.FlipHorizontal();
                            }
                        }
                        else
                        {
                            var lastTile = newImage[row - 1, col];
                            var edgeToConnectTo = lastTile.GetEdgeString(Edge.Bottom);
                            // get the tile by the edge that is not place
                            tileToAdd = tileByEdges[edgeToConnectTo].FirstOrDefault(tile => !tile.IsPlacedOnGrid);
                            while (tileToAdd.GetEdgeString(Edge.Top) != edgeToConnectTo && tileToAdd.GetEdgeString(Edge.TopReverse) != edgeToConnectTo)
                            {
                                tileToAdd.Rotate();
                            }
                            // do we need to reverse?
                            if (tileToAdd.GetEdgeString(Edge.Top) != edgeToConnectTo)
                            {
                                tileToAdd.FlipVertical();
                            }
                        }

                        newImage[row, col] = tileToAdd;
                        tileToAdd.IsPlacedOnGrid = true;
                    }
                }

                var newBigImage = new BigImage(newImage, gridSize);
                return newBigImage;
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
            var newImage = image.ConstructImage();
            newImage.LookForSeaMonster();
            var res = newImage.CountWaterRoughness();
            bool testSucceeded = res == 273;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var image = new Image(input);
            var newImage = image.ConstructImage();
            newImage.LookForSeaMonster();
            var res = newImage.CountWaterRoughness();
            return res.ToString();
        }
    }
}
