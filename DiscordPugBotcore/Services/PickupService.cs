using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using DiscordPugBotcore.Enums;

namespace DiscordPugBotcore.Entities
{
    public class PickupService
    {
        public Game CurrentGame;
        private int _minimumPlayers = 10;
        private readonly IServiceProvider _provider;
        
        public List<PugPlayer> PlayerPool { get; set; }
        public List<PugPlayer> Subs { get; set; }
        public PickupState PickupState;
        
        public List<PugPlayer> Captains => this.PlayerPool.Where(p => p.IsCaptain).ToList();
        public bool HasMinimumPlayers => this.PlayerPool.Count >= this._minimumPlayers;
        public bool HasEnoughCaptains => this.PlayerPool.Select(p => p.WantsCaptain).ToList().Count >= 2;
        public int PlayersNeeded => this._minimumPlayers - this.PlayerPool.Count;

        public PickupService(IServiceProvider provider, int minimumPlayers = 10)
        {
            this._provider = provider;
            this._minimumPlayers = minimumPlayers;
            this.PickupState = PickupState.Gathering;
            this.PlayerPool = new List<PugPlayer>();
        }

        public void Gather()
        {
            PickupState = PickupState.Gathering;
        }
        
        public string StartPicking()
        {
            if (!this.HasMinimumPlayers)
                return $"Not enough players in pool {this.FormattedPlayerNumbers()}";
            
            if (this.HasEnoughCaptains)
            {
                this.PickupState = PickupState.Captains;
                //TODO: Randomly select captains
                return "Picking is about to start!";
            }
            return $"Not enough players in pool {this.FormattedPlayerNumbers()}";
        }

        public void AddPlayer(PugPlayer pugPlayer)
        {
            if (this.HasMinimumPlayers)
            {
                Console.WriteLine("Pug is full.");
                return;
            }

            if (this.PlayerPool.Contains(pugPlayer))
            {
                Console.WriteLine($"{pugPlayer.User.Username} is already added.");
                return;
            }
            
            this.PlayerPool.Add(pugPlayer);
            Console.WriteLine($"{pugPlayer.User.Username} added.");
        }

        public void RemovePlayer(IUser user)
        {
            var match = this.PlayerPool.FirstOrDefault(p => p.User.Id == user.Id);
            if (match != null)
            {
                this.PlayerPool = this.PlayerPool.Where(p => p.User.Id != user.Id).ToList();
                Console.WriteLine($"{user.Username} successfully removed from list.");
            }
            else
            {
                Console.WriteLine($"{user.Username} is not in the player list.");
            }
        }

        public string Status() => $"Status: {this.PickupState} - {this.FormattedPlayerNumbers()}";

        public string FormattedPlayerNumbers() => $"[{this.PlayerPool.Count}/{this._minimumPlayers}]";

        public string FormattedPlayersNeeded() => this.HasMinimumPlayers ? string.Empty : $"{this.PlayersNeeded} more players needed.";
    }
}