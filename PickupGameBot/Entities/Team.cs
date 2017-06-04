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
            Players.ForEach(p => builder.Append(p.ToMentionString()));
            return builder.ToString();
        }
    }
}