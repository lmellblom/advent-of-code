using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace adventOfCode.Y2020.D04
{
    [CodeName("Passport Processing")]
    public class AoC : IAdventOfCodeWithTest
    {

        public Result First(List<string> input)
        {
            var scanner = new PassportScanner(input);
            int validPassports = scanner.NrOfPassportsWithRequiredFields();
            return new Result(validPassports);
        }

        public Result Second(List<string> input)
        {
            var scanner = new PassportScanner(input);
            int validPassports = scanner.NrOfValidPassports();
            return new Result(validPassports);
        }

        public TestResult Test(List<string> input)
        {
            var scanner = new PassportScanner(input);
            int validPassports = scanner.NrOfPassportsWithRequiredFields();
            var expected = 2;
            bool succeded = validPassports == expected;
            return new TestResult(succeded, expected, validPassports);
        }

        public TestResult Test2(List<string> input)
        {
            var scanner = new PassportScanner(input);
            int validPassports = scanner.NrOfValidPassports();
            var expected = 2;
            bool succeded = validPassports == expected;
            return new TestResult(succeded, expected, validPassports);
        }

        protected class PassportScanner
        {
            // optional
            // cid (Country ID)
            private string[] _optionalFieldKeys = new string[]
            {
                "cid"
            };

            // requiered fields
            // byr (Birth Year)
            // iyr (Issue Year)
            // eyr (Expiration Year)
            // hgt (Height)
            // hcl (Hair Color)
            // ecl (Eye Color)
            // pid (Passport ID)
            private string[] _requiredFieldKeys = new string[]
            {
                "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid"
            };

            private List<Passport> ReadPassports { get; set; }

            public PassportScanner(List<string> input)
            {
                ReadPassports = new List<Passport>();

                // process input
                // read every row until a blank line, then a new passport is recognized
                ReadPassports.Add(new Passport());
                foreach (var row in input)
                {
                    if (String.IsNullOrWhiteSpace(row))
                    {
                        // new passport found
                        ReadPassports.Add(new Passport());
                    }
                    else
                    {
                        // add all fields to the latest known passport
                        foreach (var field in row.Split(' '))
                        {
                            var keyValuePair = field.Split(':');
                            ReadPassports.Last().AddField(keyValuePair[0], keyValuePair[1]);
                        }
                    }
                }
            }

            internal int NrOfPassportsWithRequiredFields()
            {
                int valid = 0;
                foreach (var passport in ReadPassports)
                {
                    if (PassportHasAllRequiredFields(passport))
                    {
                        valid++;
                    }
                }
                return valid;
            }

            internal int NrOfValidPassports()
            {
                int valid = 0;
                foreach (var passport in ReadPassports)
                {
                    if (PassportHasAllRequiredFields(passport) && PassportValidates(passport))
                    {
                        valid += 1;
                    }
                }
                return valid;
            }

            private bool PassportValidates(Passport passport)
            {
                var fields = passport.GetFields();
                foreach (var field in fields)
                {
                    bool fieldIsValid = FieldIsValid(field.Key, field.Value);
                    if (!fieldIsValid)
                    {
                        return false;
                    }
                }

                return true;
            }

            public bool FieldIsValid(string key, string value)
            {
                Regex colorRegex = new Regex(@"#[0-9a-f]{6}");
                List<string> eyeColorOptions = new List<string>() {
                    "amb", "blu", "brn", "gry", "grn", "hzl", "oth"
                };
                Regex passIdRegex = new Regex(@"[0-9]{9}$");

                switch (key)
                {
                    case "byr":
                        // four digits; at least 1920 and at most 2002
                        if (!FieldIsBetweenDigits(value, 1920, 2002))
                        {
                            return false;
                        }
                        break;
                    case "iyr":
                        // four digits; at least 2010 and at most 2020
                        if (!FieldIsBetweenDigits(value, 2010, 2020))
                        {
                            return false;
                        }
                        break;
                    case "eyr":
                        // four digits; at least 2020 and at most 2030
                        if (!FieldIsBetweenDigits(value, 2020, 2030))
                        {
                            return false;
                        }
                        break;
                    case "hgt":
                        // a number followed by either cm or in:
                        if (value.EndsWith("cm"))
                        {
                            string fieldvalue = value.Replace("cm", "");
                            // If cm, the number must be at least 150 and at most 193.
                            if (!FieldIsBetweenDigits(fieldvalue, 150, 193))
                            {
                                return false;
                            }
                        }
                        else if (value.EndsWith("in"))
                        {
                            string fieldvalue = value.Replace("in", "");
                            // If in, the number must be at least 59 and at most 76.
                            if (!FieldIsBetweenDigits(fieldvalue, 59, 76))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                        break;
                    case "hcl":
                        // a # followed by exactly six characters 0-9 or a-f.
                        Match m = colorRegex.Match(value);
                        if (!m.Success || value.Length != 7)
                        {
                            return false;
                        }
                        break;
                    case "ecl":
                        // exactly one of: amb blu brn gry grn hzl oth
                        if (!eyeColorOptions.Contains(value))
                        {
                            return false;
                        }
                        break;
                    case "pid":
                        // a nine-digit number, including leading zeroes.
                        // passIdRegex
                        Match match = passIdRegex.Match(value);
                        if (match.Success && value.Length == 9)
                        {
                            break;
                        }
                        else
                        {
                            return false;
                        }
                    case "cid":
                        break;
                    default:
                        break;
                }

                return true;
            }

            private bool FieldIsBetweenDigits(string field, int low, int high)
            {
                if (int.TryParse(field, out int fieldDigit))
                {
                    bool fieldIsValid = fieldDigit >= low && fieldDigit <= high;
                    if (fieldIsValid)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            private bool PassportHasAllRequiredFields(Passport passport)
            {
                var fields = passport.GetFields();

                // check if every required fields exists in the fields
                foreach (var fieldKey in _requiredFieldKeys)
                {
                    if (!fields.ContainsKey(fieldKey))
                    {
                        return false; ;
                    }
                }
                return true;
            }
        }

        protected record Passport
        {
            Dictionary<string, string> Fields { get; set; } = new Dictionary<string, string>();

            public void AddField(string key, string value)
            {
                Fields.Add(key, value);
            }
            public Dictionary<string, string> GetFields()
            {
                return Fields;
            }
        }
    }
}