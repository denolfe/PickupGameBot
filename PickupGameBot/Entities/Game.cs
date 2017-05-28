using System.Collections.Generic;
using System.Linq;
using Discord;

namespace PickupGameBot.Entities
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
    }
}