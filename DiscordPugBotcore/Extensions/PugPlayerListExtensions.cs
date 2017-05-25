using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using DiscordPugBotcore.Entities;

namespace DiscordPugBotcore.Extensions
{
    public static class PugPlayerListExtensions
    {
        public static bool ContainsPlayer(this List<PugPlayer> playerList, IUser user)
        {
            return playerList.Any(p => p.User.Id == user.Id);
        }

        public static PugPlayer GetPlayer(this List<PugPlayer> playerList, PugPlayer player)
        {
            return playerList.GetPlayer(player.User);
        }
        
        public static PugPlayer GetPlayer(this List<PugPlayer> playerList, IUser user)
        {
            return playerList.FirstOrDefault(p => p.User.Id == user.Id);
        }

        public static List<PugPlayer> WithPlayerRemoved(this List<PugPlayer> playerList, IUser user)
        {
            return playerList.Where(p => p.User.Id != user.Id).ToList();
        }
        
    }
}