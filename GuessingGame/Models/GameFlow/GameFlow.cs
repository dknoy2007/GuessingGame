using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuessingGame.Models.Enums;
using GuessingGame.Models.Players;

namespace GuessingGame.Models.GameFlow
{
    public class GameFlow
    {
        private readonly List<GuessingPlayer> _players;
        
        public GameFlow()
        {
            _players = new List<GuessingPlayer>();
            GameStage = GameStage.BeforeStart;
        }

        public GameStage GameStage { get; set; }

        public async Task Run()
        {
            GuessingGameUtils.DisplayGameInstructions();

            DisplayGameMessage();

            if (GameStage == GameStage.Exit)
            {
                await ExitGame();
                return;
            }

            do
            {
                var getPlayersResult = GuessingGameUtils.GetPlayers();

                GameStage = getPlayersResult.GameStage;

                if (GameStage == GameStage.Exit)
                {
                    await ExitGame();
                    return;
                }

                Console.WriteLine("\nAll players are on board, lets play!\n");

                await Task.Delay(1000);

                var game = new Game.GuessingGame(getPlayersResult.Players);

                var gameResult = await game.Run();

                GuessingGameUtils.PrintGameResult(gameResult);

                GameStage = GameStage.End;

                Console.WriteLine("\nGreat game!\n");

                DisplayGameMessage();
            }
            while (GameStage == GameStage.Start);
        }

        private void DisplayGameMessage()
        {
            var isBeforeStart = GameStage == GameStage.BeforeStart;

            do
            {
                Console.WriteLine($"Do you want to start {(isBeforeStart ? "playing" : "another game")}? [Y]es  [N]o");

                var playerChoice = Console.ReadLine();

                if (playerChoice != null)
                {
                    switch (playerChoice.ToLower().Trim())
                    {
                        case "y":
                            GameStage = GameStage.Start;
                            break;
                        case "n":
                            GameStage = GameStage.Exit;
                            break;
                        default:
                            Console.WriteLine("\nInvalid input, please try again.\n");
                            break;
                    }
                }
            }
            while (GameStage == (isBeforeStart ? GameStage.BeforeStart : GameStage.End));
        }

        private async Task ExitGame()
        {
            Console.WriteLine("\nGoodbye and have a nice Day!");
            await Task.Delay(1000);
        }
    }
}