using GuessingGame.Models.Enums;

namespace GuessingGame.Models.Players
{
    public class ThoroughPlayer : GuessingPlayer
    {
        public ThoroughPlayer(string name) : base(name)
        {
            Type = PlayerType.Thorough;
        }

        private int LastGuess { get; set; } = GuessingGameUtils.LowerPossibleWeightOfBasket;

        public override int GuessBasketWeight()
        {
            return LastGuess == GuessingGameUtils.UpperPossibleWeightOfBasket - 1 
                ? GuessingGameUtils.LowerPossibleWeightOfBasket 
                : LastGuess++;
        }
    }
}