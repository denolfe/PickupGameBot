using System.Collections.Generic;
using System.Linq;
using Discord;

namespace PickupGameBot.Entities
{
    public class Game
    {
        public Dictionary<int, Team> Teams;
        public int MinimumPlayers;

        public Game(int minimumPlayers = 10)
        {
            MinimumPlayers = minimumPlayers;
            Teams = new Dictionary<int, Team>();
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

        public PugPlayer GetCaptainFromId(int id)
        {
            return Teams[id].Captain;
        }
        
        public bool BothTeamsAreFull()
        {
            if (Teams.Count == 0 || Teams[1] == null || Teams[2] == null)
                return false;

            return Teams[1].IsFull() && Teams[2].IsFull();
        }
    }
}