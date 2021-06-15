using System.Collections.Generic;
using GuessingGame.Models.Enums;

namespace GuessingGame.Models.Players
{
    public class MemoryPlayer : GuessingPlayer
    {
        public MemoryPlayer(string name) : base(name)
        {
            Type = PlayerType.Memory;
            AlreadyGuessed = new HashSet<int>();
        }

        private HashSet<int> AlreadyGuessed { get; }

        public override int GuessBasketWeight()
        {
            int guess;

            do
            {
                guess = GuessingGameUtils.GenerateRandomBasketWeight();
            } 
            while (AlreadyGuessed.Contains(guess));

            AlreadyGuessed.Add(guess);

            return guess;
        }
    }
}