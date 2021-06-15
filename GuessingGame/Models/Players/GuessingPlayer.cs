using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using GuessingGame.Models.Enums;

namespace GuessingGame.Models.Players
{
    public abstract class GuessingPlayer
    {
        protected GuessingPlayer(string name)
        {
            Id = GuessingGameUtils.GenerateId();
            Name = name;
        }

        public string Id { get; }

        public string Name { get; set; }

        public PlayerType Type { get; set; }

        public int WeightOfBasket { get; set; }

        public int NumberOfGuesses { get; set; }

        public ConcurrentDictionary<int, KeyValuePair<int, string>> Guesses { get; set; }

        public async Task Guess()
        {
            var guess = 0;

            var endOfGameTime = DateTime.UtcNow.AddMilliseconds(GuessingGameUtils.GameTimeInMilliseconds);

            while (guess != WeightOfBasket &&
                   Guesses.Count <= GuessingGameUtils.MaxOverallGuessesAllowed &&
                   DateTime.UtcNow < endOfGameTime)
            {
                NumberOfGuesses++;

                guess = GuessBasketWeight();

                var delta = Math.Abs(WeightOfBasket - guess);

                if (!Guesses.ContainsKey(delta))
                {
                    Guesses.TryAdd(delta, new KeyValuePair<int, string>(guess, Id));
                }

                if (delta > 0)
                {
                    await Task.Delay(delta);
                }
            }
        }

        public abstract int GuessBasketWeight();
    }
}