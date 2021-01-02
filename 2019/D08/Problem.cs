using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2019.D08
{
    [CodeName("Space Image Format")]
    public class AoC : IAdventOfCodeWithTest
    {

        public Result First(List<string> input)
        {
            var converter = new SpaceImageFormatConverter(25, 6, input.First());
            var layer = converter.GetLayer();
            var value = layer.NrOf(1) * layer.NrOf(2);
            return new Result(value);
        }

        public Result Second(List<string> input)
        {
            var converter = new SpaceImageFormatConverter(25, 6, input.First());
            var value = converter.GetNewImage();
            converter.PrintImage(value);
            return new Result("See output when debugging..");
        }

        public TestResult Test(List<string> input)
        {
            var converter = new SpaceImageFormatConverter(3, 2, input.First());
            var layer = converter.GetLayer();
            var value = layer.NrOf(1) * layer.NrOf(2);
            var expected = 1;
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        public TestResult Test2(List<string> input)
        {
            var input2 = "0222112222120000";
            var converter = new SpaceImageFormatConverter(2, 2, input2);
            var value = converter.GetNewImage();
            var expected = "0110";
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        public class SpaceImageFormatConverter
        {
            public int Wide { get; set; }
            public int Tall { get; set; }
            public string Image { get; set; }
            public SpaceImageFormatConverter(int w, int t, string input)
            {
                Wide = w;
                Tall = t;
                Image = input;
            }

            public Layer GetLayer()
            {
                var layers = ConvertToLayers();
                var order = layers.OrderBy(l => l.NrOf(0));
                return order.First();
            }

            public void PrintImage(string image)
            {
                for (int pixel = 0; pixel < Wide * Tall; pixel++)
                {
                    if (pixel % Wide == 0)
                    {
                        Console.WriteLine();
                    }
                    var element = image.ElementAt(pixel);
                    Console.Write(element == '1' ? "#" : " ");
                }
            }

            public string GetNewImage()
            {
                var layers = ConvertToLayers();
                var newString = "";
                for (int pixel = 0; pixel < Wide * Tall; pixel++)
                {
                    // go trough every layers until we find a 0 or 1 at that position
                    var layer = layers.FirstOrDefault(l => l.PixelIsBlackOrWhite(pixel));
                    newString += layer.PixelAt(pixel);
                }
                return newString;
            }

            private List<Layer> ConvertToLayers()
            {
                var layers = new List<Layer>();

                // 3 pixels wide (take 3 chars, then 2 tall! = 6 chars)
                var pixels = Image.Select(x => x.ToString()).Select(Int32.Parse).ToList();

                int layerId = 1;
                int pixelId = 0;
                while (pixelId < pixels.Count())
                {
                    var newImg = pixels.Skip(pixelId).Take(Wide * Tall);
                    layers.Add(new Layer(layerId, newImg.ToList()));
                    pixelId += Wide * Tall;
                    layerId++;
                }

                return layers;
            }
        }

        public record Layer(int id, List<int> values)
        {
            public int NrOf(int i) => values.Where(v => v == i).Count();

            public bool PixelIsBlackOrWhite(int pos) => PixelAt(pos) == 0 || PixelAt(pos) == 1;
            public int PixelAt(int pos) => values[pos];
        }
    }
}