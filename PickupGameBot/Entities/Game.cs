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

        public Game(Dictionary<int, Team> teams)
        {
            Teams = teams;
            Picked = true;
        }

        /// <summary>
        /// Creates teams from list of captains
        /// </summary>
        /// <param name="captains"></param>
        public void CreateTeams(List<PugPlayer> captains)
        {
            foreach (var captain in captains)
            {
                Teams.Add(captain.TeamId, new Team(captain.TeamId, captain, MinimumPlayers/2));
            }
        }

        public void CreateTeams()
        {
            Teams.Add(1, new Team(1, MinimumPlayers/2));
            Teams.Add(2, new Team(2, MinimumPlayers/2));
        }
        
        /// <summary>
        /// Adds player to captain's team
        /// </summary>
        /// <param name="captain"></param>
        /// <param name="player"></param>
        public void AddToCaptainsTeam(PugPlayer captain, PugPlayer player)
        {
            Teams[captain.TeamId].AddPlayer(player);
        }

        public void AddToTeamById(int teamId, PugPlayer player)
        {
            Teams[teamId].AddPlayer(player);
        }

        public PugPlayer GetCaptainFromId(int id)
        {
            return Teams[id].Captain;
        }

        /// <summary>
        /// Removes teams from game, returns all players
        /// </summary>
        /// <returns></returns>
        public List<PugPlayer> RemoveTeams()
        {
            var players = new List<PugPlayer>();
            Teams.Select(p => p.Value).ToList().ForEach(t => players.AddRange(t.PopAll(true)));
            Teams = new Dictionary<int, Team>();
            return players;
        }

        /// <summary>
        /// Removes players from team
        /// </summary>
        /// <returns></returns>
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

        public Game Clone()
            => new Game(Teams);

        public override string ToString()
        {
            var builder = new StringBuilder();
            Teams
                .Select(t => t.Value).ToList()
                .ForEach(team => builder.Append(team.ToString()));
            return builder.ToString();
        }
    }
}