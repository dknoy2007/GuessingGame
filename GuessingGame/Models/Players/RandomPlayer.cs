using GuessingGame.Models.Enums;

namespace GuessingGame.Models.Players
{
    public class RandomPlayer : GuessingPlayer
    {
        public RandomPlayer(string name) : base(name)
        {
            Type = PlayerType.Random;
        }

        public override int GuessBasketWeight()
        {
            return GuessingGameUtils.GenerateRandomBasketWeight();
        }
    }
}