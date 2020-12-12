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

        public bool IsSeat(char input)
        {
            return input == SEAT_IS_EMPTY || input == SEAT_IS_OCCUPIED;
        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            var input = System.IO.File.ReadAllLines(filename).ToList();
            int seats = HowManySeatsOccupied(input, Rule1);
            bool testSucceeded = seats == 37;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            int seats = HowManySeatsOccupied(input, Rule1);
            return seats.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            int seats = HowManySeatsOccupied(input, Rule2);
            bool testSucceeded = seats == 26;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            int seats = HowManySeatsOccupied(input, Rule2);
            return seats.ToString();
        }

        private int HowManySeatsOccupied(List<string> input, Func<List<string>, int, int, string> rule)
        {
            var newLayout = GetSeatLayout(input, rule);
            var newLayoutString = string.Join(' ', newLayout);
            var inputLayoutString = string.Join(' ', input);

            while (newLayoutString != inputLayoutString)
            {
                input = newLayout;
                newLayout = GetSeatLayout(input, rule);
                newLayoutString = string.Join(' ', newLayout);
                inputLayoutString = string.Join(' ', input);
            }

            // count seats ocuppied in layout
            return newLayoutString.Where(seat => seat == SEAT_IS_OCCUPIED).Count();
        }

        private string Rule1(List<string> input, int rowIndex, int colIndex)
        {
            return Rule(input, rowIndex, colIndex, 4, GetAdjacentSeats);
        }

        private string Rule2(List<string> input, int rowIndex, int colIndex)
        {
            return Rule(input, rowIndex, colIndex, 5, GetFirstAdjacentSeats);
        }

        private string Rule(List<string> input, int rowIndex, int colIndex, int occupiedSeatRule, Func<List<string>, int, int, string> rule)
        {
            char currentSeat = input[rowIndex][colIndex];
            if (currentSeat == FLOOR)
            {
                return currentSeat.ToString();
            }

            string adjacentSeats = rule(input, rowIndex, colIndex);
            int nrOfOccupiedAdjancentSeats = CountSeats(adjacentSeats);

            if (currentSeat == SEAT_IS_EMPTY && nrOfOccupiedAdjancentSeats == 0)
            {
                return SEAT_IS_OCCUPIED.ToString();
            }
            else if (currentSeat == SEAT_IS_OCCUPIED && nrOfOccupiedAdjancentSeats >= occupiedSeatRule)
            {
                return SEAT_IS_EMPTY.ToString();
            }
            else
            {
                return currentSeat.ToString();
            }
        }

        private List<string> GetSeatLayout(List<string> input, Func<List<string>, int, int, string> rule)
        {
            List<string> newLayout = new List<string>();
            for (int rowIndex = 0; rowIndex < input.Count(); rowIndex++)
            {
                var row = input[rowIndex];
                string newRow = "";

                // go trough each letter and add to new layout since we do not want to change current
                for (int colIndex = 0; colIndex < row.Count(); colIndex++)
                {
                    newRow += rule(input, rowIndex, colIndex);
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
        
        private string GetFirstAdjacentSeats(List<string> input, int rowIndex, int colIndex)
        {
            string seats = "";

            seats += GetFirstSeatOccurence(input, -1, 0, rowIndex, colIndex);
            seats += GetFirstSeatOccurence(input, 1, 0, rowIndex, colIndex);

            seats += GetFirstSeatOccurence(input, 0, -1, rowIndex, colIndex);
            seats += GetFirstSeatOccurence(input, 0, 1, rowIndex, colIndex);

            seats += GetFirstSeatOccurence(input, -1, -1, rowIndex, colIndex);
            seats += GetFirstSeatOccurence(input, 1, 1, rowIndex, colIndex);

            seats += GetFirstSeatOccurence(input, 1, -1, rowIndex, colIndex);
            seats += GetFirstSeatOccurence(input, -1, 1, rowIndex, colIndex);
            return seats;
        }

        // Traverse the input with the desired steps, check for forst occurence
        private string GetFirstSeatOccurence(List<string> input, int right, int down, int startRow, int startCol)
        {
            int rowIndex = startRow; // | - number of list
            int colIndex = startCol; // - check letter

            rowIndex += down;
            colIndex += right;

            while (rowIndex >= 0 && rowIndex < input.Count())
            {
                var row = input[rowIndex]; // all letters
                if (colIndex < 0 || colIndex >= row.Count())
                {
                    // out of range
                    return "";
                }

                var letter = row[colIndex];
                if (IsSeat(letter))
                {
                    var output = letter.ToString();
                    return output;
                }

                rowIndex += down;
                colIndex += right;
            }

            return ""; // no found
        }
    }
}