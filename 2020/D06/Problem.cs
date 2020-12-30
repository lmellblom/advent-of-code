using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace adventOfCode.Y2020.D06
{
    [CodeName("Custom Customs")]
    public class AoC : IAdventOfCodeWithTest
    {

        public Result First(List<string> input)
        {
            int sum = GetGroups(input).Sum(g => g.AnswersAnyone());
            return new Result(sum);
        }

        public Result Second(List<string> input)
        {
            int sum = GetGroups(input).Sum(g => g.AnswersEveryone());
            return new Result(sum);
        }

        public TestResult Test(List<string> input)
        {
            int sum = GetGroups(input).Sum(g => g.AnswersAnyone());
            var expected = 11;
            bool succeded = sum == expected;
            return new TestResult(succeded, expected, sum);
        }

        public TestResult Test2(List<string> input)
        {
            int sum = GetGroups(input).Sum(g => g.AnswersEveryone());
            var expected = 6;
            bool succeded = sum == expected;
            return new TestResult(succeded, expected, sum);
        }

        public class Group
        {
            public List<Person> Members { get; private set; }

            public Group()
            {
                Members = new List<Person>();
            }

            public void AddPerson(Person p)
            {
                Members.Add(p);
            }

            public int AnswersAnyone()
            {
                return Members.SelectMany(x => x.Answers).Distinct().ToList().Count();
            }

            public int AnswersEveryone()
            {
                return Members.Select(x => x.Answers).Aggregate<IEnumerable<char>>(
                    (previousList, nextList) => previousList.Intersect(nextList)
                    ).ToList().Count();
            }
        }

        public record Person(char[] Answers) { }

        private List<Group> GetGroups(List<string> input)
        {
            List<Person> persons = input
                .Select(line => String.IsNullOrWhiteSpace(line) ? null : new Person(line.ToCharArray()))
                .ToList();

            // add the person to the right groups
            List<Group> groups = new List<Group>()
            {
                new Group()
            };

            foreach (var person in persons)
            {
                if (person == null)
                {
                    // new group found
                    groups.Add(new Group());
                }
                else
                {
                    groups.Last().AddPerson(person);
                }
            }

            return groups;
        }
    }
}