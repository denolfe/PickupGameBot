using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PickupGameBot.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public List<PugPlayer> Players { get; set; } = new List<PugPlayer>();
        public PugPlayer Captain { get; set; }
        public int MaxPlayers { get; set; }

        public Team(int id, PugPlayer captain, int maxPlayers)
        {
            Id = id;
            Captain = captain;
            MaxPlayers = maxPlayers;
            Players.Add(captain);
        }

        public Team(int id, int maxPlayers)
        {
            Id = id;
            MaxPlayers = maxPlayers;
        }

        public void AddPlayer(PugPlayer player) => Players.Add(player);
        public bool IsFull() => Players.Count == MaxPlayers;

        /// <summary>
        /// Pop all players, optionally including captain
        /// </summary>
        /// <param name="includeCaptain"></param>
        /// <returns></returns>
        public List<PugPlayer> PopAll(bool includeCaptain = false)
        {
            List<PugPlayer> playerList;
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

        public string Formatted()
        {
            var builder = new StringBuilder();
            builder.Append($"Team {Id}: ");
            builder.Append(string.Join(",", Players.Select(p => p.ToMentionString())));
            return builder.ToString();
        }
    }
}