using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December21 : AdventOfCode
    {
        public December21() : base(21)
        {
        }

        public class AllergenAssessment
        {
            public List<Food> Foods;
            public Dictionary<string, List<string>> IngredientsByAllergen = new Dictionary<string, List<string>>();

            public AllergenAssessment(List<string> input)
            {
                Foods = input.Select(line => new Food(line)).ToList();
                ContructAllergens();
            }

            private void ContructAllergens()
            {
                var distinctAllergens = Foods.SelectMany(x => x.Allergens).Distinct();
                foreach (var allergen in distinctAllergens)
                {
                    var ingredientsThatMaybeContainsTheAllergen = Foods
                        .Where(x => x.Allergens.Contains(allergen))
                        .Select(x => x.Ingredients)
                        .ToList();

                    var possibleIngredients = ingredientsThatMaybeContainsTheAllergen
                        .Aggregate((x, y) => x.Intersect(y).ToList());

                    // find the intersections between all the ingredients
                    IngredientsByAllergen.Add(
                        allergen,
                        possibleIngredients);
                }
            }

            public int Run()
            {
                // Find the ingredients that is not in the allergy list!
                return Foods
                    .SelectMany(x => x.Ingredients)
                    .Where(x => !IngredientsByAllergen.SelectMany(x => x.Value).Distinct().Contains(x))
                    .Count();
            }

            public string Run2()
            {
                // same as 16dec
                while (IngredientsByAllergen.Any(item => item.Value.Count() != 1))
                {
                    // get the first value that is one and remove in other lists
                    var remove = IngredientsByAllergen
                        .Where(item => item.Value.Count() == 1)
                        .SelectMany(item => item.Value)
                        .ToList();
                    var valuesMoreThen1 = IngredientsByAllergen.Where(item => item.Value.Count() != 1);
                    foreach (var item in valuesMoreThen1)
                    {
                        item.Value.RemoveAll(x => remove.Contains(x));
                    }
                }

                var orderedIngredients = IngredientsByAllergen
                    .OrderBy(item => item.Key)
                    .SelectMany((item => item.Value));
                return string.Join(",", orderedIngredients);
            }
        }

        public class Food
        {
            public List<string> Ingredients;
            public List<string> Allergens;
            public Food(string input)
            {
                Ingredients = input
                    .Split('(')
                    .First()
                    .Trim()
                    .Split(" ")
                    .Select(value => value.Trim())
                    .ToList();

                Allergens = input
                    .Split('(')
                    .Last()
                    .Replace(")", "")
                    .Replace("contains", "")
                    .Split(",")
                    .Select(value => value.Trim())
                    .ToList();
            }

            // Each allergen is found in exactly one ingredient. Each ingredient contains zero or one allergen
        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();

            var assessment = new AllergenAssessment(input);
            var result = assessment.Run();

            bool testSucceeded = result == 5;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var assessment = new AllergenAssessment(input);
            var result = assessment.Run();
            return result.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var assessment = new AllergenAssessment(input);
            var result = assessment.Run2();
            bool testSucceeded = result == "mxmxvkd,sqjhc,fvjkl";
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            var assessment = new AllergenAssessment(input);
            var result = assessment.Run2();
            return result.ToString();
        }
    }
}
