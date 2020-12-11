using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December11 : AdventOfCode
    {
        public December11() : base(11)
        {
        }

        public static char SEAT_IS_EMPTY = 'L';
        public static char SEAT_IS_OCCUPIED = '#';
        public static char FLOOR = '.';

        public override bool Test()
        {
            string filename = GetTestFilename();
            var input = System.IO.File.ReadAllLines(filename).ToList();
            int seats = HowManySeatsOccupied(input);
            bool testSucceeded = seats == 37;
            return testSucceeded;
        }

        private int HowManySeatsOccupied(List<string> input)
        {
            var newLayout = GetSeatLayout(input);
            var newLayoutString = string.Join(' ', newLayout);
            var inputLayoutString = string.Join(' ', input);

            while (newLayoutString != inputLayoutString)
            {
                input = newLayout;
                newLayout = GetSeatLayout(input);
                newLayoutString = string.Join(' ', newLayout);
                inputLayoutString = string.Join(' ', input);
            }

            // count seats ocuppied in layout
            return newLayoutString.Where(seat => seat == SEAT_IS_OCCUPIED).Count();
        }

        private List<string> GetSeatLayout(List<string> input)
        {
            // shuffle the layout, go trough all and change input into a new array?
            // string[] newLayout = new string[input.Count()];
            List<string> newLayout = new List<string>();

            for (int rowIndex = 0; rowIndex < input.Count(); rowIndex++)
            {
                var row = input[rowIndex];
                string newRow = "";

                // go trough each letter and add to new layout since we do not want to change current
                for (int colIndex = 0; colIndex < row.Count(); colIndex++)
                {
                    char currentSeat = row[colIndex];
                    string adjacentSeats = GetAdjacentSeats(input, rowIndex, colIndex);
                    int nrOfOccupiedAdjancentSeats = CountSeats(adjacentSeats);

                    if (currentSeat == SEAT_IS_EMPTY && nrOfOccupiedAdjancentSeats == 0)
                    {
                        newRow += SEAT_IS_OCCUPIED;
                    }
                    else if (currentSeat == SEAT_IS_OCCUPIED && nrOfOccupiedAdjancentSeats >= 4)
                    {
                        newRow += SEAT_IS_EMPTY;
                    }
                    else if (currentSeat == FLOOR)
                    {
                        // do nothing
                        newRow += currentSeat;
                    }
                    else
                    {
                        // do nothing
                        newRow += currentSeat;
                    }
                }

                newLayout.Add(newRow);
            }

            return newLayout;

        }

        private int CountSeats(string input)
        {
            return input.Where(seat => seat == SEAT_IS_OCCUPIED).Count();
        }

        private string GetAdjacentSeats(List<string> input, int rowIndex, int colIndex)
        {
            string seats = "";

            // rowIndex = -1  
            // rowIndex = same
            // rowIndex = +1

            List<int> rowRange = Enumerable.Range(-1, 3).Select(nr => nr + rowIndex).ToList();
            List<int> colRange = Enumerable.Range(-1, 3).Select(nr => nr + colIndex).ToList();

            foreach (var rowIn in rowRange)
            {
                foreach (var colIn in colRange)
                {
                    if (rowIn == rowIndex && colIn == colIndex)
                    {
                        // this is the seat and not the adjacent
                        continue;
                    }

                    if (rowIn >= 0 && rowIn < input.Count())
                    {
                        var row = input[rowIn];
                        if (colIn >= 0 && colIn < row.Count())
                        {
                            var value = row[colIn];
                            seats += value;
                        }
                    }
                }
            }

            return seats;
        }

        private int ShuffleSeatLayout(string[] input)
        {
            {
                int right = 1; int down = 1;

                int rowIndex = 0; // | - number of list
                int colIndex = 0; // - check letter

                int foundTrees = 0;
                var isTree = '#';

                // until the end
                rowIndex += down;
                colIndex += right;
                while (rowIndex < input.Count())
                {
                    var row = input[rowIndex]; // all letters
                    if (colIndex >= row.Count())
                    {
                        colIndex -= row.Count();
                    }
                    var letter = row[colIndex];
                    foundTrees += (letter == isTree) ? 1 : 0;
                    rowIndex += down;
                    colIndex += right;
                }

                return foundTrees;
            }
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            int seats = HowManySeatsOccupied(input);
            return seats.ToString();
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
