using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using DiscordPugBotcore.Entities;

namespace DiscordPugBotcore.Extensions
{
    public static class PugPlayerListExtensions
    {
        /// <summary>
        /// Checks if list contains IUser
        /// </summary>
        /// <param name="playerList"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static bool ContainsPlayer(this List<PugPlayer> playerList, IUser user)
        {
            return playerList.Any(p => p.User.Id == user.Id);
        }

        /// <summary>
        /// Returns specified PugPlayer
        /// </summary>
        /// <param name="playerList"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static PugPlayer GetPlayer(this List<PugPlayer> playerList, PugPlayer player)
        {
            return playerList.GetPlayer(player.User);
        }
        
        /// <summary>
        /// Returns specified PugPlayer by IUser
        /// </summary>
        /// <param name="playerList"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static PugPlayer GetPlayer(this List<PugPlayer> playerList, IUser user)
        {
            return playerList.FirstOrDefault(p => p.User.Id == user.Id);
        }

        /// <summary>
        /// Returns new List without specified PugPlayer
        /// </summary>
        /// <param name="playerList"></param>
        /// <param name="pugPlayer"></param>
        /// <returns></returns>
        public static List<PugPlayer> RemovePlayer(this List<PugPlayer> playerList, PugPlayer pugPlayer)
        {
            return playerList.RemovePlayer(pugPlayer.User);
        }
        
        /// <summary>
        /// Returns new List without specified IUser
        /// </summary>
        /// <param name="playerList"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public static List<PugPlayer> RemovePlayer(this List<PugPlayer> playerList, IUser user)
        {
            return playerList.Where(p => p.User.Id != user.Id).ToList();
        }
        
        /// <summary>
        /// Returns list with 2 captains selected at random that had WantsCaptain flag
        /// </summary>
        /// <param name="playerList"></param>
        /// <returns></returns>
        public static List<PugPlayer> SelectCaptains(this List<PugPlayer> playerList)
        {
            return playerList
                .Where(p => p.WantsCaptain)
                .OrderBy(x => Guid.NewGuid())
                .Take(2)
                .ToList();
        }

        /// <summary>
        /// Formats List&lt;PugPlayer&gt;
        /// </summary>
        /// <param name="playerList"></param>
        /// <returns></returns>
        public static string ToJoinedList(this List<PugPlayer> playerList)
        {
            var nameList = playerList.Select(p => p.User.Username);
            return string.Join(",", nameList);
        }
        
    }
}