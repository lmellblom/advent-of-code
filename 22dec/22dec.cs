using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode2020
{
    public class December22 : AdventOfCode
    {
        public static bool PRINT = false;
        
        public December22() : base(22)
        {
        }

        public class Player
        {
            // Will store the cards on the players hand
            public Queue<int> Cards { get; private set; }
            public int PlayedCard { get; private set; }
            public string Name { get; set; }
            public int NrOfCards => Cards.Count();
            public Player(string name)
            {
                Name = name;
                Cards = new Queue<int>();
            }

            public void RoundEnded()
            {
                PlayedCard = -1;
            }

            public void AddCard(int card)
            {
                Cards.Enqueue(card);
            }

            public int PlayCard()
            {
                PlayedCard = Cards.Dequeue();
                if (PRINT) Console.WriteLine($"{Name} plays: {PlayedCard}");
                return PlayedCard;
            }

            public void WonCards(List<int> cards)
            {
                cards.Sort();
                cards.Reverse();

                foreach (var card in cards)
                {
                    AddCard(card);
                }

                if (PRINT) Console.WriteLine($"{Name} wins the round!");
            }

            public void PrintCards()
            {
                var cards = String.Join(",", Cards.ToArray());
                Console.WriteLine($"{Name}'s cards: {cards}");
            }

            public int GetScore()
            {
                // The bottom card in their deck is worth the value of the card multiplied by 1, 
                // the second-from-the-bottom card is worth the value of the card multiplied by 2, and so on
                int score = 0;
                var multiplier = Cards.Count();
                foreach (var card in Cards)
                {
                    score += (card * multiplier);
                    multiplier--;
                }

                return score;
            }
        }

        public class RecursiveCombat : CrabCombat
        {
            public RecursiveCombat(List<string> input) : base(input)
            {
            }
        }

        public class CrabCombat
        {
            public List<Player> Players { get; set; }

            public int Round { get; set; }

            private int TotalNumberOfCards = 0;

            public CrabCombat(List<string> input)
            {
                Players = new List<Player>();
                // split the input on the players
                string playerName = "";
                foreach (var line in input)
                {
                    if (line.StartsWith("Player"))
                    {
                        playerName = line.Replace(":", "");
                        Players.Add(new Player(playerName));
                    }
                    else if (String.IsNullOrEmpty(line))
                    {
                        continue;
                    }
                    else
                    {
                        var card = Helper.GetNumberFromString(line);
                        Players.Last().AddCard(card);
                        TotalNumberOfCards++;
                    }
                }
            }

            public void PlayGame()
            {
                Round = 0;

                bool playNextRound = true;
                while (playNextRound)
                {
                    Round++;
                    PlayRound();
                    if (PRINT) Console.WriteLine("");

                    // check if the round has ended?
                    if (Players.Any(p => p.NrOfCards == TotalNumberOfCards))
                    {
                        playNextRound = false;
                    }
                }

                Console.WriteLine("== Post-game results ==");
                foreach (var player in Players)
                {
                    player.PrintCards();
                }
            }

            public int Score1()
            {
                // get the player that won!
                var playerWon = Players.FirstOrDefault(p => p.NrOfCards == TotalNumberOfCards);
                var score = playerWon.GetScore();
                return score;
            }

            private void PlayRound()
            {
                List<int> cardsOnTable = new List<int>();

                if (PRINT) Console.WriteLine($"-- Round {Round} --");
                if (PRINT) Players.ForEach(p => p.PrintCards());
                Players.ForEach(p => cardsOnTable.Add(p.PlayCard()));

                // find player with the highest card!
                var playerWon = Players.OrderBy(p => p.PlayedCard).Reverse().FirstOrDefault();
                playerWon.WonCards(cardsOnTable);

                // reset round!
                Players.ForEach(p => p.RoundEnded());
            }

        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();

            PRINT = true;
            CrabCombat game = new CrabCombat(input);
            game.PlayGame();
            var result = game.Score1();

            bool testSucceeded = result == 306;
            return testSucceeded;
        }

        public override string First()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            PRINT = false;
            CrabCombat game = new CrabCombat(input);
            game.PlayGame();
            var result = game.Score1();
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
