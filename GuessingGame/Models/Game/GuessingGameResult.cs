using GuessingGame.Models.Players;

namespace GuessingGame.Models.Game
{
    public class GuessingGameResult
    {
        public int WeightOfBasket { get; set; }
        public GuessingPlayer Winner { get; set; }
        public int WinnerGuess { get; set; }
    }
}