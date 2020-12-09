using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December5 : AdventOfCode
    {
        public static int HIGHEST_ROW_ID = 127;
        public static int HIGHEST_COL_ID = 7;
        public December5() : base(5)
        {
        }

        public class PlaneSeat
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public int SeatId { get; set; }
            public PlaneSeat(int row, int col, int seatId)
            {
                Row = row;
                Col = col;
                SeatId = seatId;
            }

            public override bool Equals(object obj)
            {
                return obj is PlaneSeat seat &&
                       Row == seat.Row &&
                       Col == seat.Col &&
                       SeatId == seat.SeatId;
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(Row, Col, SeatId);
            }
        }

        public override bool Test()
        {
            var testResult = new List<bool>();

            string input0 = "FBFBBFFRLR";
            var seat0 = GetSeat(input0);
            var seatOResult = new PlaneSeat(44, 5, 357);
            testResult.Add(seat0.Equals(seatOResult));

            string input1 = "BFFFBBFRRR";
            var seat = GetSeat(input1);
            var seatResult = new PlaneSeat(70, 7, 567);
            testResult.Add(seat.Equals(seatResult));

            string input2 = "FFFBBBFRRR";
            var seat2 = GetSeat(input2);
            var seat2Result = new PlaneSeat(14, 7, 119);
            testResult.Add(seat2.Equals(seat2Result));

            string input3 = "BBFFBBFRLL";
            var seat3 = GetSeat(input3);
            var seat3Result = new PlaneSeat(102, 4, 820);
            testResult.Add(seat3.Equals(seat3Result));

            bool testSucceeded = testResult.All(v => v == true);
            return testSucceeded;
        }

        public override string First()
        {
            string fileName = "5dec/input.txt";
            List<string> input = System.IO.File.ReadAllLines(fileName).ToList();
            List<PlaneSeat> seatsTaken = input.Select(boardingpass => GetSeat(boardingpass)).ToList();

            // get the passport with the highest seatId.
            var seatWithHighestId = seatsTaken.Max(s => s.SeatId);
            return seatWithHighestId.ToString();
        }

        public override bool Test2()
        {
            // NOT IMPLEMENTED
            bool testSucceeded = true;
            return testSucceeded;
        }

        public override string Second()
        {
            string fileName = "5dec/input.txt";
            List<string> input = System.IO.File.ReadAllLines(fileName).ToList();
            List<PlaneSeat> seatsTaken = input.Select(boardingpass => GetSeat(boardingpass)).ToList();

            int lowestSeat = CalculateSeatId(0,0);
            int highestSeat = CalculateSeatId(HIGHEST_ROW_ID, HIGHEST_COL_ID);

            List<int> allSeats = Enumerable.Range(lowestSeat, highestSeat).ToList();
            List<int> seatsTakenIds = seatsTaken.Select(s => s.SeatId).ToList();

            List<int> seatsEmpty = allSeats.Except(seatsTakenIds).ToList();

            // It's a completely full flight, so your seat should be the only missing boarding pass in your list. 
            // However, there's a catch: some of the seats at the very front and back of the plane don't 
            // exist on this aircraft, so they'll be missing from your list as well.

            // Your seat wasn't at the very front or back, though; the seats with IDs +1 and -1 
            // from yours will be in your list.
            var foundSeat = seatsEmpty.FirstOrDefault(sId => seatsTakenIds.Contains(sId - 1)
                && seatsTakenIds.Contains(sId + 1));
            return foundSeat.ToString();
        }

        private int BinarySpacePartioning((int low, int high) range, (char low, char high) letter, string input)
        {
            int result = -1;

            foreach (var character in input)
            {
                int addToRange = (range.high - range.low + 1) / 2;
                if (character == letter.low)
                {
                    // lower half, subtract from the high
                    range.high -= addToRange;
                }
                else if (character == letter.high)
                {
                    // higher half, add to the low
                    range.low += addToRange;
                }
            }

            if (range.low == range.high)
            {
                result = range.low;
            }

            return result;
        }

        private PlaneSeat GetSeat(string boardingpass)
        {
            (int low, int high) rowRange = (0, HIGHEST_ROW_ID);
            (char low, char high) rowLetter = ('F', 'B');

            // read each characters - first 7 is either F or B. (128 rows)
            // first seven characters, 
            int row = BinarySpacePartioning(rowRange, rowLetter, boardingpass.Substring(0, 7));

            (int low, int high) colRange = (0, HIGHEST_COL_ID);
            (char low, char high) colLetter = ('L', 'R');
            int col = BinarySpacePartioning(colRange, colLetter, boardingpass.Substring(7));

            int seatId = CalculateSeatId(row, col);

            return new PlaneSeat(row, col, seatId);
        }

        private int CalculateSeatId(int row, int col)
        {
            int seatId = row * 8 + col;
            return seatId;
        }
    }
}
