using System.Collections.Generic;

namespace PickupGameBot.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public List<PugPlayer> Players { get; set; } = new List<PugPlayer>();
        public PugPlayer Captain { get; set; }
        private int _maxPlayers { get; set; }

        public Team(int id)
        {
            this.Id = id;
        }

        public Team(int id, PugPlayer captain, int maxPlayers)
        {
            this.Id = id;
            this.Captain = captain;
            this._maxPlayers = maxPlayers;
        }

        public void AddPlayer(PugPlayer player) => this.Players.Add(player);
        public bool IsFull() => this.Players.Count == this._maxPlayers;

        public List<PugPlayer> PopAll()
        {
            var players = this.Players;
            this.Players.Clear();
            return players;
        }
    }
}