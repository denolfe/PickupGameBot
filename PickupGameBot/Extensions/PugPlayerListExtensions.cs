using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using PickupGameBot.Entities;

namespace PickupGameBot.Extensions
{
    public static class PugPlayerListExtensions
    {
        /// <summary>
        /// Checks if list contains PugPlayer
        /// </summary>
        /// <param name="playerList"></param>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool ContainsPlayer(this List<PugPlayer> playerList, PugPlayer player)
        {
            return playerList.Any(p => p.User.Id == player.User.Id);
        }
        
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
        /// 
        /// </summary>
        /// <param name="playerList"></param>
        /// <param name="mention"></param>
        /// <returns></returns>
        public static string ToFormattedList(this List<PugPlayer> playerList, bool mention = false)
        {
            var nameList = mention 
                ? playerList.Select(p => p.User.Mention).ToList() 
                : playerList.Select(p => p.ToString()).ToList();

            return string.Join(", ", nameList);
        }
    }
}