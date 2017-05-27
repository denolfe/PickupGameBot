using System.Collections.Generic;

namespace DiscordPugBotcore.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public List<PugPlayer> Players { get; set; } = new List<PugPlayer>();
        public PugPlayer Captain { get; set; }

        public Team(int id)
        {
            this.Id = id;
        }

        public Team(int id, PugPlayer captain)
        {
            this.Id = id;
            this.Captain = captain;
        }

        public void AddPlayer(PugPlayer player) => this.Players.Add(player);
    }
}