using GuessingGame.Models.Enums;
using GuessingGame.Models.Players;
using System.Collections.Generic;

namespace GuessingGame.Models.Game
{
    public class GetPlayersResult
    {
        public GetPlayersResult()
        {
            Players = new List<GuessingPlayer>();
        }

        public GameStage GameStage { get; set; }
        public List<GuessingPlayer> Players { get; set; }
    }
}
