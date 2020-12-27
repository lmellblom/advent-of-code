using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode
{
    public class December2 : AoCSolver
    {
        public class Passwords
        {
            public int FirstRuleNr { get; set; }
            public int SecondRuleNr { get; set; }
            public char Letter { get; set; }
            public string Password { get; set; }
            public Passwords(int first, int second, char letter, string password)
            {
                FirstRuleNr = first;
                SecondRuleNr = second;
                Letter = letter;
                Password = password;
            }

            public bool IsValid()
            {
                // find number of count of the letter in password and check if in range
                var foundMatches = Password.Count(c => c == Letter);
                return FirstRuleNr <= foundMatches && foundMatches <= SecondRuleNr;
            }

            public bool IsValidOnPosition()
            {
                // 1 = first letter. so -1 on everything
                var firstChar = Password.ElementAtOrDefault(FirstRuleNr - 1);
                var secondChar = Password.ElementAtOrDefault(SecondRuleNr - 1);

                // only one is accaptable
                return firstChar != secondChar && (firstChar == Letter || secondChar == Letter);
            }
        }

        public December2() : base(2)
        {
        }

        public override bool Test()
        {
            string fileName = GetTestFilename();
            List<Passwords> passwords = GetPasswordsFromInput(fileName);
            List<bool> nrOfValidPasswords = passwords.Select(pass => pass.IsValid()).ToList();
            int nrOfVaild = nrOfValidPasswords.Count(valid => valid == true);
            bool testSucceeded = nrOfVaild == 2;
            return testSucceeded;
        }

        public override string First()
        {
            string fileName = GetFilename();
            List<Passwords> passwords = GetPasswordsFromInput(fileName);
            List<bool> nrOfValidPasswords = passwords.Select(pass => pass.IsValid()).ToList();
            int nrOfVaild = nrOfValidPasswords.Count(valid => valid == true);
            return nrOfVaild.ToString();
        }

        public override bool Test2()
        {
            // read file
            string fileName = GetTestFilename();
            List<Passwords> passwords = GetPasswordsFromInput(fileName);
            List<bool> nrOfValidPasswords = passwords.Select(pass => pass.IsValidOnPosition()).ToList();
            int nrOfVaild = nrOfValidPasswords.Count(valid => valid == true);
            bool testSucceeded = nrOfVaild == 1;
            return testSucceeded;
        }
        public override string Second()
        {
            string fileName = GetFilename();
            List<Passwords> passwords = GetPasswordsFromInput(fileName);
            List<bool> nrOfValidPasswords = passwords.Select(pass => pass.IsValidOnPosition()).ToList();
            int nrOfVaild = nrOfValidPasswords.Count(valid => valid == true);
            return nrOfVaild.ToString();
        }

        private List<Passwords> GetPasswordsFromInput(string fileName)
        {
            List<string> inputs = System.IO.File.ReadAllLines(fileName).ToList();
            List<Passwords> passwords = inputs.Select(input => ConvertLineToPassword(input)).ToList();
            return passwords;
        }

        private Passwords ConvertLineToPassword(string input)
        {
            // split the password
            input = input.Replace(":", "");
            string[] splitInput = input.Split(" ");
            string bound = splitInput[0];
            List<int> bounds = bound.Split("-").Select(Int32.Parse).ToList();
            string letter = splitInput[1];
            string password = splitInput[2];
            return new Passwords(bounds.First(), bounds.Last(), char.Parse(letter), password);
        }
    }
}
