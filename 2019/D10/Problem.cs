using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2019.D10
{
    [CodeName("Monitoring Station")]
    public class AoC : IAdventOfCodeWithTest
    {
        public Result First(List<string> input)
        {
            var station = new MonitoringStation(input);
            var position = station.FindBestPlace();
            return new Result(position.nr);
        }

        public Result Second(List<string> input)
        {
            var station = new MonitoringStation(input);
            var value = station.RunLaser();
            return new Result(value);
        }

        public TestResult Test(List<string> input)
        {
            var station = new MonitoringStation(input);
            var position = station.FindBestPlace();
            var expected = 210;
            bool succeded = position.nr == expected;
            return new TestResult(succeded, expected, position.nr);
        }

        public TestResult Test2(List<string> input)
        {
            var station = new MonitoringStation(input);
            var value = station.RunLaser();
            var expected = 802;
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        public class MonitoringStation
        {
            public List<string> MapData { get; set; }
            public MonitoringStation(List<string> input)
            {
                MapData = input;
                ConvertedAstroidsPoint = new Dictionary<Point, List<PolarPoint>>();

                var coordinatesWithAstroids = GetAstroids();

                // calculate the polarcoordinates for every shiftedpoint
                foreach (var point in coordinatesWithAstroids)
                {
                    var shifted = ShiftCoordinates(point.x, point.y, coordinatesWithAstroids.ToList());
                    var polarPoints = GetPolarPoints(shifted);
                    ConvertedAstroidsPoint.Add(point, polarPoints);
                }
            }

            public record PolarPoint(double radius, double angle, Point orgOffset)
            {
                public double Degree() => angle * (180 / Math.PI);

                // convert to up...
                public double GetRotatedDegrees()
                {
                    var degree = Degree() - 270;
                    while (degree < 0)
                    {
                        degree += 360;
                    }
                    return degree;
                }
            }
            public record Point(int x, int y) { }

            public Dictionary<Point, List<PolarPoint>> ConvertedAstroidsPoint { get; set; }

            public (Point p, int nr) FindBestPlace()
            {
                var asteroids = new List<(Point p, int nr)>();
                foreach (var (point, polarPoints) in ConvertedAstroidsPoint)
                {
                    // get unique by radius, then it is that number than we are after!
                    var asteroidsInSight = polarPoints.GroupBy(p => p.angle).Count();
                    asteroids.Add((point, asteroidsInSight));
                }

                // get the max number!
                return asteroids.OrderBy(a => a.nr).Reverse().First();
            }

            public int RunLaser()
            {
                var fromPlace = FindBestPlace();
                var place = ConvertedAstroidsPoint[fromPlace.p];

                var vaporized = new Dictionary<int, Point>();
                int i = 1;

                // run from angle 0 and then run every angle and remove first in every angle until 200th
                while (place.Count() > 0)
                {
                    var groups = place.OrderBy(i => i.GetRotatedDegrees()).GroupBy(p => p.GetRotatedDegrees()).ToList();

                    var reference = groups.Where(g => g.ToList().Count() == 12);
                    // for each group, remove the first asteroid and put it in the list?
                    foreach (var angle in groups)
                    {
                        var items = angle.OrderBy(a => a.radius).ToList();
                        var vaporize = items.First();

                        // vaporize item!!!
                        var point = new Point(vaporize.orgOffset.x + fromPlace.p.x, vaporize.orgOffset.y + fromPlace.p.y);
                        vaporized.Add(i, point);
                        place.Remove(vaporize);
                        i++;
                    }
                }

                if (vaporized.ContainsKey(200))
                {
                    var nr200 = vaporized[200];
                    return nr200.x * 100 + nr200.y;
                }

                return -1;
            }

            private List<Point> GetAstroids()
            {
                var coordinatesWithAstroids = new List<Point>();
                for (int y = 0; y < MapData.Count(); y++)
                {
                    for (int x = 0; x < MapData.Count(); x++)
                    {
                        if (MapData[y][x] == '#')
                        {
                            coordinatesWithAstroids.Add(new Point(x, y));
                        }
                    }
                }
                return coordinatesWithAstroids;
            }

            private List<PolarPoint> GetPolarPoints(List<Point> points)
            {
                var output = new List<PolarPoint>();
                foreach (var item in points)
                {
                    int x = item.x;
                    int y = item.y;
                    double radius = Math.Sqrt((x * x) + (y * y));
                    double angle = Math.Atan2(y, x);
                    output.Add(new PolarPoint(radius, angle, item));
                }
                return output;
            }

            private List<Point> ShiftCoordinates(int h, int k, List<Point> points)
            {
                points.Remove(new Point(h, k)); // remove self
                return points
                    .Select(p => new Point(p.x - h, p.y - k))
                    .ToList();
            }
        }
    }
}