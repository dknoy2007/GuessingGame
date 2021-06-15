using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GuessingGame.Models.Players;

namespace GuessingGame.Models.Game
{
    public class GuessingGame
    {
        private readonly ConcurrentDictionary<int, KeyValuePair<int, string>> _guesses;

        private readonly int _weightOfBasket;

        private readonly List<GuessingPlayer> _players;

        public GuessingGame(List<GuessingPlayer> players)
        {
            _players = players;
            _guesses = new ConcurrentDictionary<int, KeyValuePair<int, string>>();
            _weightOfBasket = GuessingGameUtils.GenerateRandomBasketWeight();
        }

        public async Task<GuessingGameResult> Run()
        {
            InitPlayers();

            var tasks = _players.Select(player => player.Guess()).ToList();

            await Task.WhenAny(tasks);

            return GetGameResult();
        }

        private GuessingGameResult GetGameResult()
        {
            var (guess, value) = _guesses[_guesses.Keys.Min()];
            
            var winner = _players.FirstOrDefault(x => x.Id == value);
            
            return new GuessingGameResult
            {
                WeightOfBasket = _weightOfBasket,
                Winner = winner,
                WinnerGuess = guess
            };
        }

        private void InitPlayers()
        {
            Parallel.ForEach(_players, player => {
                player.WeightOfBasket = _weightOfBasket;
                player.Guesses = _guesses;
            });
        }
    }
}
