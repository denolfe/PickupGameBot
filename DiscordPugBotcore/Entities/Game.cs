using System.Collections.Generic;
using System.Linq;
using Discord;

namespace DiscordPugBotcore.Entities
{
    public class Game
    {
        public Dictionary<int, Team> Teams;

        public Game()
        {
            this.Teams = new Dictionary<int, Team>();
        }

        public void CreateTeams(List<PugPlayer> captains)
        {
            foreach (var captain in captains)
            {
                this.Teams.Add(captain.TeamId, new Team(captain.TeamId));
                this.Teams[captain.TeamId].AddPlayer(captain);
            }
        }
        
        public void AddToCaptainsTeam(PugPlayer captain, PugPlayer player)
        {
            this.Teams[captain.TeamId].AddPlayer(player);
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