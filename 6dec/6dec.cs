using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December6 : AdventOfCode
    {
        public December6() : base(6)
        {
        }

        public override bool Test()
        {
            string fileName = "6dec/input_test.txt";
            int sum = CountYesAnyone(fileName);
            bool testSucceeded = sum == 11;
            return testSucceeded;
        }

        public override string First()
        {
            string fileName = "6dec/input.txt";
            int sum = CountYesAnyone(fileName);
            return sum.ToString();
        }

        public override bool Test2()
        {
            string fileName = "6dec/input_test.txt";
            int sum = CountYesEveryone(fileName);
            bool testSucceeded = sum == 6;
            return testSucceeded;
        }

        public override string Second()
        {
            string fileName = "6dec/input.txt";
            int sum = CountYesEveryone(fileName);
            return sum.ToString();
        }

        public class Person
        {
            public List<char> Answers { get; private set; }
            public Person(string line)
            {
                Answers = line.Select(x => x).ToList();
            }
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

        private List<Group> GetGroupsFromInput(string filename)
        {
            List<Person> persons = System.IO.File.ReadAllLines(filename)
                .Select(line => String.IsNullOrWhiteSpace(line) ? null : new Person(line)).ToList();

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

        private int CountYesAnyone(string filename)
        {
            List<Group> groups = GetGroupsFromInput(filename);
            int sum = groups.Sum(g => g.AnswersAnyone());
            return sum;
        }

        private int CountYesEveryone(string filename)
        {
            List<Group> groups = GetGroupsFromInput(filename);
            int sum = groups.Sum(g => g.AnswersEveryone());
            return sum;
        }
    }
}
