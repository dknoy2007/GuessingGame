using System;
using GuessingGame.Models.Enums;

namespace GuessingGame.Models.Players
{
    public class ThoroughCheaterPlayer : ThoroughPlayer
    {
        public ThoroughCheaterPlayer(string name) : base(name)
        {
            Type = PlayerType.ThoroughCheater;
        }

        public override int GuessBasketWeight()
        {
            int guess;

            do
            {
                guess = base.GuessBasketWeight();
            }
            while (Guesses.ContainsKey(Math.Abs(WeightOfBasket - guess)));

            return guess;
        }
    }
}