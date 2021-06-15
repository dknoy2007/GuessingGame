using GuessingGame.Models.Enums;
using GuessingGame.Models.Game;
using GuessingGame.Models.Players;
using System;
using System.Collections.Generic;
using System.Text;

namespace GuessingGame
{
    public static class GuessingGameUtils
    {
        public const int LowerPossibleWeightOfBasket = 41;
        public const int UpperPossibleWeightOfBasket = 141;
        public const int MaxOverallGuessesAllowed = 100;
        public const int GameTimeInMilliseconds = 1500;
        public static List<string> _playerTypes = new List<string> { "r", "m", "c", "t", "tc" };

        public static string GenerateId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static int GenerateRandomBasketWeight()
        {
            return new Random().Next(LowerPossibleWeightOfBasket, UpperPossibleWeightOfBasket);
        }

        public static void DisplayGameInstructions()
        {
            var sb = new StringBuilder();

            sb.AppendLine("Welcome to Fruit Basket Weight guessing game!");
            sb.AppendLine();
            sb.AppendLine("Game Instructions:");
            sb.AppendLine();
            sb.AppendLine("The goal of the game is to guess the weight of a fruit basket.");
            sb.AppendLine("The weight of the basket will be randomly selected between 40 (exclusive) and 140 (inclusive) kilos.");
            sb.AppendLine();
            sb.AppendLine("Game Rules:");
            sb.AppendLine();
            sb.AppendLine("The game ends when one of the players identifies the weight correctly or when 100 attempts were completed.");
            sb.AppendLine();
            sb.AppendLine("Game Player Types:");
            sb.AppendLine();
            sb.AppendLine("Random player: guesses a random number between 40 (exclusive) and 140 (inclusive).");
            sb.AppendLine("Memory player: guesses a random number between 40 and 140 but does not try the same number more than once.");
            sb.AppendLine("Thorough player: tries all numbers by order – 41,42,43 …");
            sb.AppendLine("Cheater player: guesses a random number between 40 (exclusive) and 140 (inclusive), but does not try any of the numbers that other players had already tried.");
            sb.AppendLine("Thorough Cheater player: tries all numbers by order – 41,42,43 … but skips numbers that were already been tried before by any of the players.");
            sb.AppendLine();
            sb.AppendLine("Game Flow:");
            sb.AppendLine();
            sb.AppendLine("If a player guessed a number incorrectly – he will have to wait the absolute delta (between the real weight and his guess) in milliseconds.");
            sb.AppendLine();
            sb.AppendLine("For example:");
            sb.AppendLine("If the actual weight of the basket is 100, and a player guessed 70, the player will wait (sleep) for 30 milliseconds.");
            sb.AppendLine("If his guess was 130, he will also sleep for 30 milliseconds.");
            sb.AppendLine();
            sb.AppendLine("Inputs:");
            sb.AppendLine();
            sb.AppendLine("1. The number of participating players – 2 through 8");
            sb.AppendLine("2. For each player – his name and his type,  by ','.");
            sb.AppendLine("Outputs:");
            sb.AppendLine();
            sb.AppendLine("1. The real weight of the basket.");
            sb.AppendLine("2. At the end of the game:");
            sb.AppendLine("\ta. If there was a winner – his name and total amount of attempts in the game.");
            sb.AppendLine("\tb. In case there was no winner – the name of the player who was the closest (in absolute value) and his guess.");
            sb.AppendLine("\t   If there were more than one – the one that was the first. Also, his guess should be printed as well.");
            sb.AppendLine();

            Console.WriteLine(sb.ToString());
        }

        internal static void PrintGameResult(GuessingGameResult gameResult)
        {
            Console.WriteLine("Game result:\n");
            Console.WriteLine($"Weight of basket: {gameResult.WeightOfBasket}");
            Console.WriteLine($"Winner: {gameResult.Winner.Name}");

            if (gameResult.WeightOfBasket == gameResult.WinnerGuess)
            {
                Console.WriteLine($"Number of guesses: {gameResult.Winner.NumberOfGuesses}");
            }
            else
            {
                Console.WriteLine($"Winner guess: {gameResult.WinnerGuess}");
            }
        }

        public static GetPlayersResult GetPlayers()
        {
            Console.WriteLine("\nLets start the game!\n");

            int numberOfPlayers = 0;
            var playersResult = new GetPlayersResult();
            string userInput;

            do
            {
                Console.WriteLine("Please enter the numer of participating players [2-8] or [E]xit: ");

                userInput = Console.ReadLine();

                if (string.IsNullOrEmpty(userInput))
                {
                    Console.WriteLine("\nInvalid player details: please try again\n");
                    continue;
                }

                if (userInput.ToLower().Trim() == "e")
                {
                    return new GetPlayersResult
                    {
                        GameStage = GameStage.Exit
                    };
                }

                if (!int.TryParse(userInput, out numberOfPlayers) || numberOfPlayers < 2 || numberOfPlayers > 8)
                {
                    Console.WriteLine("\nNumber of participating players must be between [2-8].\n");
                    numberOfPlayers = 0;
                }
            }
            while (numberOfPlayers == 0);

            var playerIndex = 1;

            do
            {
                Console.WriteLine($"\nPlease enter {GetIntAsString(playerIndex)} player name and type, delimited by ',' or [E]xit");
                Console.WriteLine("Player types: [R] Random, [M] Memory, [T] Thorough, [C] Cheater, [TC] Thorough Cheater.");
                Console.WriteLine("For example: Daniel Katz, M");

                userInput = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(userInput))
                {
                    Console.WriteLine("\nNo player details: please try again\n");
                    continue;
                }

                if (userInput.ToLower().Trim() == "e")
                {
                    return new GetPlayersResult
                    {
                        GameStage = GameStage.Exit
                    };
                }

                var splittedUserInput = userInput.Split(',');

                if (splittedUserInput.Length != 2 || string.IsNullOrWhiteSpace(splittedUserInput[0]) || 
                    string.IsNullOrWhiteSpace(splittedUserInput[1]) || !_playerTypes.Contains(splittedUserInput[1].ToLower().Trim())) 
                {
                    Console.WriteLine("\nInvalid player details: please try again\n");
                    continue;
                }

                var playerType = GetPlayerTypeFromUserInput(splittedUserInput[1]);

                if (playerType == PlayerType.None)
                {
                    Console.WriteLine("\nInvalid player type: please try again.\n");
                    continue;
                }

                var player = playersResult.Players
                    .Find(x => x.Name == splittedUserInput[0] && x.Type == playerType);

                if (player != null)
                {
                    Console.WriteLine($"\nPlayer {splittedUserInput[0]} of type {playerType} already exist: please try again.\n");
                    continue;
                }

                playersResult.Players.Add(CreatePlayer(splittedUserInput[0], playerType));

                playerIndex++;
            }
            while (numberOfPlayers != playerIndex - 1);

            playersResult.GameStage = GameStage.Continue;

            return playersResult;
        }

        private static GuessingPlayer CreatePlayer(string name, PlayerType type)
        {
            switch (type)
            {
                case PlayerType.Random:
                    return new RandomPlayer(name);
                case PlayerType.Memory:
                    return new MemoryPlayer(name);
                case PlayerType.Cheater:
                    return new CheaterPlayer(name);
                case PlayerType.Thorough:
                    return new ThoroughPlayer(name);
                case PlayerType.ThoroughCheater:
                    return new ThoroughCheaterPlayer(name);
                default:
                    throw new Exception($"Invalid user input type {type} for player {name}");
            }
        }

        private static PlayerType GetPlayerTypeFromUserInput(string playerInput)
        {
            switch (playerInput.ToLower().Trim())
            {
                case "r":
                    return PlayerType.Random;
                case "m":
                    return PlayerType.Memory;
                case "c":
                    return PlayerType.Cheater;
                case "t":
                    return PlayerType.Thorough;
                case "tc":
                    return PlayerType.ThoroughCheater;
                default:
                    return PlayerType.None;
            }
        }

        private static string GetIntAsString(int number)
        {
            if (number <= 0)
            {
                throw new Exception($"Invalid number {number} argument: number must be grater than 0");
            }

            switch (number)
            {
                case 1:
                    return $"{1}st";
                case 2:
                    return $"{2}nd";
                case 3:
                    return $"{3}rd";
                default:
                    return $"{number}th";
            }
        }
    }
}