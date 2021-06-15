using System;
using GuessingGame.Models.Enums;

namespace GuessingGame.Models.Players
{
    public class CheaterPlayer : RandomPlayer
    {
        public CheaterPlayer(string name) : base(name)
        {
            Type = PlayerType.Cheater;
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