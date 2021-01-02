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
            return new Result("not implemented");
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
            var value = "-";
            var expected = "";
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
        }
    }
}