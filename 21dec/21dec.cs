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
            public Dictionary<string, List<string>> MaybeAllergenListByIngredient = new Dictionary<string, List<string>>();
            public Dictionary<string, List<Food>> FoodsByAllergies = new Dictionary<string, List<Food>>();
            public AllergenAssessment(List<string> input)
            {
                Foods = new List<Food>();
                foreach (var item in input)
                {
                    var food = new Food(item);
                    Foods.Add(food);

                    foreach (var ingredient in food.Ingredients)
                    {
                        if (!MaybeAllergenListByIngredient.ContainsKey(ingredient))
                        {
                            MaybeAllergenListByIngredient[ingredient] = new List<string>();
                        }

                        MaybeAllergenListByIngredient[ingredient] = MaybeAllergenListByIngredient[ingredient].Union(food.Allergens).ToList();
                    }

                    foreach (var allergi in food.Allergens)
                    {
                        if (!FoodsByAllergies.ContainsKey(allergi))
                        {
                            FoodsByAllergies[allergi] = new List<Food>();
                        }

                        FoodsByAllergies[allergi].Add(food);
                    }
                }
            }

            public int Run()
            {
                var safeIngredients = new List<string>();
                foreach (var (ingredient, possibleAllergies) in MaybeAllergenListByIngredient)
                {
                    var impossible = new List<string>();
                    foreach (var allergi in possibleAllergies)
                    {
                        foreach (var food in FoodsByAllergies[allergi])
                        {
                            var contains = food.Ingredients.Contains(ingredient);
                            if (!contains)
                            {
                                impossible.Add(allergi);
                                break;
                            }
                        }
                    }

                    var possibleLeft = possibleAllergies.Except(impossible).ToList();
                    if (!possibleLeft.Any())
                    {   
                        safeIngredients.Add(ingredient);
                    }
                }

                var total = 0;
                foreach (var ingr in safeIngredients)
                {
                    foreach (var food in Foods)
                    {
                        total += food.Ingredients.Count(i => i == ingr);
                    }
                }

                return total;
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
