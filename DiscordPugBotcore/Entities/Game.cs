using System.Collections.Generic;
using System.Linq;
using Discord;

namespace DiscordPugBotcore.Entities
{
    public class Game
    {
        public Dictionary<int, Team> Teams;

        public void AddToCaptainsTeam(PugPlayer captain, PugPlayer player)
        {
            this.Teams[captain.TeamId].AddPlayer(player);
        }

        public Game()
        {
            Teams = new Dictionary<int, Team>
            {
                { 1, new Team(1) },
                { 2, new Team(2) }
            };
        }
        
        public List<PugPlayer> PopAll()
        {
            var allPickedPlayers = new List<PugPlayer>();
            allPickedPlayers.AddRange(Teams[0].Players);
            allPickedPlayers.AddRange(Teams[1].Players);
            Teams[0].Players.Clear();
            Teams[1].Players.Clear();

            return allPickedPlayers;
        }
    }
}