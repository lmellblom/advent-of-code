using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2020.D05
{
    [CodeName("Binary Boarding")]
    public class AoC : IAdventOfCodeWithTest
    {
        public static int HIGHEST_ROW_ID = 127;
        public static int HIGHEST_COL_ID = 7;

        public Result First(List<string> input)
        {
            List<PlaneSeat> seatsTaken = input.Select(boardingpass => GetSeatFromBoardingPass(boardingpass)).ToList();
            // get the passport with the highest seatId.
            var seatWithHighestId = seatsTaken.Max(s => s.SeatId);
            return new Result(seatWithHighestId);
        }

        public Result Second(List<string> input)
        {
            List<PlaneSeat> seatsTaken = input.Select(boardingpass => GetSeatFromBoardingPass(boardingpass)).ToList();

            int lowestSeat = CalculateSeatId(0, 0);
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
            return new Result(foundSeat);
        }

        public TestResult Test(List<string> input)
        {
            var testResult = new List<(PlaneSeat actual, PlaneSeat expected)>();

            string input0 = "FBFBBFFRLR";
            var seat0 = GetSeatFromBoardingPass(input0);
            var seat0Result = new PlaneSeat(44, 5, 357);
            testResult.Add((seat0, seat0Result));

            string input1 = "BFFFBBFRRR";
            var seat = GetSeatFromBoardingPass(input1);
            var seatResult = new PlaneSeat(70, 7, 567);
            testResult.Add((seat, seatResult));

            bool succeded = testResult.All(v => v.expected.Equals(v.actual));
            var expected = String.Join(", ", testResult.Select(t => t.expected));
            var actual = String.Join(", ", testResult.Select(t => t.actual));

            return new TestResult(succeded, expected, actual);
        }

        public TestResult Test2(List<string> input)
        {
            // NOT IMPLEMENTED
            var value = "";
            var expected = "";
            bool succeded = value == expected;
            return new TestResult(succeded, expected, value);
        }

        private PlaneSeat GetSeatFromBoardingPass(string boardingpass)
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


        public record PlaneSeat(int Row, int Col, int SeatId)
        {
        }
    }
}