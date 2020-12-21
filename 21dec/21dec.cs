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
                Foods = new List<Food>();
                foreach (var item in input)
                {
                    var food = new Food(item);
                    Foods.Add(food);
                }

                ContructAllergens();
            }

            private void ContructAllergens()
            {
                var allAllergens = Foods.SelectMany(x => x.Allergens).Distinct();
                foreach (var allergen in allAllergens)
                {
                    var ingredientsThatMaybeContainsTheAllergen = Foods
                        .Where(x => x.Allergens.Contains(allergen))
                        .Select(x => x.Ingredients)
                        .ToList();

                    // find the intersections between all the ingredients
                    IngredientsByAllergen.Add(
                        allergen,
                        ingredientsThatMaybeContainsTheAllergen
                            .Aggregate((x, y) => x.Intersect(y)
                            .ToList()
                        ));
                }
            }

            public int Run()
            {
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
                    var remove = IngredientsByAllergen.Where(item => item.Value.Count() == 1).Select(item => item.Value).SelectMany(v => v).ToList();
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
                var splittedInput = input.Split('(');
                var ingredientsInput = splittedInput[0].Trim();
                Ingredients = ingredientsInput.Split(" ").ToList();

                var allergensInput = splittedInput[1].Replace(")", "").Replace("contains", "");
                Allergens = allergensInput.Split(",").Select(a => a.Trim()).ToList();
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
