using System.Collections.Generic;
using System.Linq;

namespace PickupGameBot.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public List<PugPlayer> Players { get; set; } = new List<PugPlayer>();
        public PugPlayer Captain { get; set; }
        private int MaxPlayers { get; set; }

        public Team(int id, PugPlayer captain, int maxPlayers)
        {
            Id = id;
            Captain = captain;
            MaxPlayers = maxPlayers;
            Players.Add(captain);
        }

        public void AddPlayer(PugPlayer player) => Players.Add(player);
        public bool IsFull() => Players.Count == MaxPlayers;

        public List<PugPlayer> PopAll(bool includeCaptain = false)
        {
            var playerList = new List<PugPlayer>();
            if (includeCaptain)
            {
                playerList = Players;
            }
            else
            {
                playerList = Players.Where(p => !p.IsCaptain).ToList();
                Players = new List<PugPlayer> {Captain};
            }
                
            
            Players = new List<PugPlayer> {Captain};
            return playerList;
        }
    }
}