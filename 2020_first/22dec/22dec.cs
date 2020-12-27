using System;
using System.Collections.Generic;
using System.Linq;

namespace adventOfCode
{
    public class December22 : AoCSolver
    {
        public static bool PRINT = false;

        public December22() : base(22)
        {
        }

        public class Player
        {
            public Queue<int> Cards { get; private set; }   // Will store the cards on the players hand
            public int PlayedCard { get; private set; }     // Current played card from that player
            public string Name { get; set; }
            public int NrOfCards => Cards.Count();
            public bool Won = false; // for part 2

            private string _cardsString => String.Join(",", Cards.ToArray());
            private List<string> _memory = new List<string>();

            public Player(string name)
            {
                Name = name;
                Cards = new Queue<int>();
                _memory = new List<string>();
            }

            public Player(string name, List<int> cards)
            {
                Name = name;
                Cards = new Queue<int>();
                cards.ForEach(card => Cards.Enqueue(card));
                _memory = new List<string>();
            }

            public bool HasPlayedCardBefore()
            {
                return _memory.Contains(_cardsString);
            }

            public void ResetRound()
            {
                PlayedCard = -1;
            }

            public void AddCard(int card)
            {
                Cards.Enqueue(card);
            }

            public int PlayCard()
            {
                _memory.Add(_cardsString);
                PlayedCard = Cards.Dequeue();
                if (PRINT) Console.WriteLine($"{Name} plays: {PlayedCard}");
                return PlayedCard;
            }

            public void WonCards(List<int> cards)
            {
                cards.ForEach(card => AddCard(card));
            }

            public void WonCards(int card1, int card2)
            {
                AddCard(card1);
                AddCard(card2);
            }

            public void PrintCards()
            {
                var cards = String.Join(",", Cards.ToArray());
                if (PRINT) Console.WriteLine($"{Name}'s cards: {cards}");
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
            public int SubGame = 1;
            public RecursiveCombat(List<string> input, int gameNr = 1) : base(input, gameNr)
            {
            }

            public RecursiveCombat(List<Player> players, int gameNr = 1) : base(players, gameNr)
            {
                Players = players;
                TotalNumberOfCards = Players.Sum(p => p.Cards.Count);
            }

            protected override void SetResultOfRound()
            {
                // CHECK IF A RECURSIVE GAME SHOULD HAPPEN

                // If both players have at least as many cards remaining 
                // in their deck as the value of the card they just drew, 
                // the winner of the round is determined by playing a new game of Recursive Combat.
                Player playerWon;

                bool playSubgame = Players.All(p => p.NrOfCards >= p.PlayedCard);
                if (playSubgame)
                {
                    if (PRINT) Console.WriteLine("Playing a sub-game to determine the winner...");

                    var playersWithNewCards = Players.Select(p => new Player(p.Name, p.Cards.ToList().Take(p.PlayedCard).ToList())).ToList();
                    SubGame++;
                    var newGame = new RecursiveCombat(playersWithNewCards, SubGame);
                    var playerWonRecursiceGame = newGame.PlayGame();
                    playerWon = Players.FirstOrDefault(p => p.Name == playerWonRecursiceGame.Name);
                    var otherPlayer = Players.FirstOrDefault(p => p.Name != playerWonRecursiceGame.Name);
                    // note that the winner's card might be the lower-valued of the two cards
                    playerWon.WonCards(playerWon.PlayedCard, otherPlayer.PlayedCard);

                    if (PRINT) Console.WriteLine($"...anyway, back to game {GameNr}.");
                }
                else
                {
                    // find player with the highest card!
                    playerWon = Players.OrderBy(p => p.PlayedCard).Reverse().FirstOrDefault();

                    CardsOnTable.Sort();
                    CardsOnTable.Reverse();
                    playerWon.WonCards(CardsOnTable);
                }

                if (PRINT) Console.WriteLine($"{playerWon.Name} wins round {Round} of game {GameNr}!");

                // reset round!
                Players.ForEach(p => p.ResetRound());
                CardsOnTable = new List<int>();
            }

            protected override bool RoundHasEnded()
            {
                // if there was a previous round in this game that had exactly the same cards in the same order in the same players' decks, 
                // the game instantly ends in a win for player 1.
                var shouldEnd = Players.Any(p => p.HasPlayedCardBefore());
                if (shouldEnd)
                {
                    if (PRINT) Console.WriteLine("Game ended! Same cards appeared..");
                    Players.FirstOrDefault().Won = true;
                    return true;
                }

                var player = Players.FirstOrDefault(p => p.NrOfCards == TotalNumberOfCards);
                if (player != null)
                {
                    player.Won = true;
                    return true;
                }
                return false;
            }
        }

        public class CrabCombat
        {
            public int GameNr { get; set; }
            public List<Player> Players { get; set; }
            public int Round { get; set; }
            protected int TotalNumberOfCards = 0;
            protected List<int> CardsOnTable { get; set; }

            public CrabCombat(List<string> input, int gameNr = 1)
            {
                Players = new List<Player>();
                CardsOnTable = new List<int>();
                GameNr = gameNr;
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

            public CrabCombat(List<Player> players, int gameNr = 1)
            {
                Players = players;
                GameNr = gameNr;
                TotalNumberOfCards = Players.Sum(p => p.Cards.Count);
                CardsOnTable = new List<int>();
            }

            public Player PlayGame()
            {
                if (PRINT) Console.WriteLine($"=== Game {GameNr} ===");
                Round = 0;

                bool playNextRound = true;
                while (playNextRound)
                {
                    if (RoundHasEnded())
                    {
                        playNextRound = false;
                        break;
                    }
                    else
                    {
                        Round++;
                        PlayRound();
                        if (PRINT) Console.WriteLine("");
                    }
                }
                var playerWon = Players.FirstOrDefault(p => p.Won);
                if (PRINT) Console.WriteLine($"The winner of game {GameNr} is {playerWon.Name}!");

                if (GameNr == 1)
                {
                    if (PRINT) Console.WriteLine("== Post-game results ==");
                    foreach (var player in Players)
                    {
                        player.PrintCards();
                    }
                }
                return playerWon;
            }

            protected virtual bool RoundHasEnded()
            {
                // check if the round has ended?
                var player = Players.FirstOrDefault(p => p.NrOfCards == TotalNumberOfCards);
                if (player != null)
                {
                    player.Won = true;
                    return true;
                }
                return false;
            }

            public int Score()
            {
                // get the player that won!
                var playerWon = Players.FirstOrDefault(p => p.Won == true);
                var score = playerWon.GetScore();
                return score;
            }

            protected virtual void PlayRound()
            {
                PlayCards();
                SetResultOfRound();
            }

            protected virtual void SetResultOfRound()
            {
                // find player with the highest card!
                var playerWon = Players.OrderBy(p => p.PlayedCard).Reverse().FirstOrDefault();
                CardsOnTable.Sort();
                CardsOnTable.Reverse();
                playerWon.WonCards(CardsOnTable);
                if (PRINT) Console.WriteLine($"{playerWon.Name} wins round {Round} of game {GameNr}!");

                // reset round!
                Players.ForEach(p => p.ResetRound());
                CardsOnTable = new List<int>();
            }

            protected void PlayCards()
            {
                if (PRINT) Console.WriteLine($"-- Round {Round} (Game: {GameNr}) --");
                if (PRINT) Players.ForEach(p => p.PrintCards());
                Players.ForEach(p => CardsOnTable.Add(p.PlayCard()));
            }
        }

        public override bool Test()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();

            PRINT = false;
            CrabCombat game = new CrabCombat(input);
            game.PlayGame();
            var result = game.Score();

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

            var result = game.Score();
            return result.ToString();
        }

        public override bool Test2()
        {
            string filename = GetTestFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            PRINT = false;
            RecursiveCombat game = new RecursiveCombat(input);
            game.PlayGame();
            var result = game.Score();

            bool testSucceeded = result == 291;
            return testSucceeded;
        }

        public override string Second()
        {
            string filename = GetFilename();
            List<string> input = System.IO.File.ReadAllLines(filename).ToList();
            PRINT = false;
            RecursiveCombat game = new RecursiveCombat(input);
            game.PlayGame();
            var result = game.Score();
            return result.ToString();
        }
    }
}
