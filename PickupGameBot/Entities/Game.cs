using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;

namespace PickupGameBot.Entities
{
    public class Game
    {
        public Dictionary<int, Team> Teams;
        public int MinimumPlayers;
        public bool Picked = false;

        public Game(int minimumPlayers = 10)
        {
            MinimumPlayers = minimumPlayers;
            Teams = new Dictionary<int, Team>();
        }

        public void CreateTeams(List<PugPlayer> captains)
        {
            foreach (var captain in captains)
            {
                this.Teams.Add(captain.TeamId, new Team(captain.TeamId, captain, MinimumPlayers/2));
//                this.Teams[captain.TeamId].AddPlayer(captain);
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

        public List<PugPlayer> RemoveTeams()
        {
            var players = new List<PugPlayer>();
            Teams.Select(p => p.Value).ToList().ForEach(t => players.AddRange(t.PopAll(true)));
            Teams = new Dictionary<int, Team>();
            return players;
        }

        public List<PugPlayer> Repick()
        {
            var players = new List<PugPlayer>();
            Teams.Select(p => p.Value).ToList().ForEach(t => players.AddRange(t.PopAll()));
            return players;
        }
        
        public bool BothTeamsAreFull()
        {
            if (Teams.Count == 0 || Teams[1] == null || Teams[2] == null)
                return false;

            return Teams[1].IsFull() && Teams[2].IsFull();
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            this.Teams
                .Select(t => t.Value).ToList()
                .ForEach(team => builder.Append(team.ToString()));
            return builder.ToString();
        }
    }
}