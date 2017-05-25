using System.Collections.Generic;
using System.Linq;
using Discord;

namespace DiscordPugBotcore.Entities
{
    public class Game
    {
        public List<PugPlayer> Team1;
        public List<PugPlayer> Team2;

        public void AddToTeam1(PugPlayer player) => Team1.Add(player);    
        public void AddToTeam2(PugPlayer player) => Team2.Add(player);

        public List<PugPlayer> PopAll()
        {
            List<PugPlayer> allPickedPlayers = new List<PugPlayer>();
            allPickedPlayers.AddRange(Team1);
            allPickedPlayers.AddRange(Team2);
            Team1.Clear();
            Team2.Clear();

            return allPickedPlayers;
        }
    }
}