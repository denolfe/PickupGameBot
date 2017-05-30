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

        public Team(int id)
        {
            this.Id = id;
        }

        public Team(int id, PugPlayer captain, int maxPlayers)
        {
            this.Id = id;
            this.Captain = captain;
            this.MaxPlayers = maxPlayers;
            this.Players.Add(captain);
        }

        public void AddPlayer(PugPlayer player) => this.Players.Add(player);
        public bool IsFull() => this.Players.Count == this.MaxPlayers;

        public List<PugPlayer> PopAll()
        {
            var players = this.Players.Where(p => !p.IsCaptain).ToList();
            this.Players = new List<PugPlayer> {this.Captain};
            return players;
        }
    }
}