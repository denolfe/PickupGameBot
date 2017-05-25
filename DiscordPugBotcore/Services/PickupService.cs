using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using DiscordPugBotcore.Enums;
using DiscordPugBotcore.Extensions;

namespace DiscordPugBotcore.Entities
{
    public class PickupService
    {
        public Game _currentGame;
        private int _minimumPlayers = 10;
        private readonly IServiceProvider _provider;
        
        public List<PugPlayer> Captains { get; set; }
        public List<PugPlayer> PlayerPool { get; set; }
        public List<PugPlayer> Subs { get; set; }
        public PickupState PickupState;
        
//        public List<PugPlayer> Captains => this.PlayerPool.Where(p => p.IsCaptain).ToList();
        
        public bool HasMinimumPlayers => this.PlayerPool.Count >= this._minimumPlayers;
        public bool HasEnoughCaptains => this.PlayerPool.Select(p => p.WantsCaptain).ToList().Count >= 2;
        public int PlayersNeeded => this._minimumPlayers - this.PlayerPool.Count;

        public PickupService(IServiceProvider provider, int minimumPlayers = 10)
        {
            this._provider = provider;
            this._minimumPlayers = minimumPlayers;
            this._currentGame = new Game();
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
                return $"Not enough players in pool {this.FormattedPlayerNumbers()}. {this.FormattedPlayersNeeded()}";
            
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
            if (this.PickupState != PickupState.Gathering)
            {
                Console.WriteLine($"State: {this.PickupState}. New players cannot join at this time");
                return;
            }

            if (this.PlayerPool.Contains(pugPlayer))
            {
                Console.WriteLine($"{pugPlayer.User.Username} has already joined.");
                return;
            }
            
            this.PlayerPool.Add(pugPlayer);
            Console.WriteLine($"{pugPlayer.User.Username} joined.");
        }

        public void RemovePlayer(IUser user)
        {
            var match = this.PlayerPool.FirstOrDefault(p => p.User.Id == user.Id);
            if (match != null)
            {
                this.PlayerPool = this.PlayerPool.WithPlayerRemoved(user);
                Console.WriteLine($"{user.Username} successfully removed from list.");
            }
            else
            {
                Console.WriteLine($"{user.Username} is not in the player list.");
            }
        }

        public void PickPlayer(IUser captain, IUser user)
        {
            if (this.Captains.ContainsPlayer(captain))
            {
                Console.WriteLine($"{captain.Username} is not a captain!");   
            }
            if (this.PlayerPool.ContainsPlayer(user))
            {
                Console.WriteLine($"{user.Username} is not in the player pool");
            }
            
            //TODO: Get current captain's team id
            //TODO: Assign player to captain's team
        }

        public void Repick()
        {
            this.PlayerPool.AddRange(this._currentGame.PopAll());
        }

        public string Status() => $"Status: {this.PickupState} - {this.FormattedPlayerNumbers()}";

        public string FormattedPlayerNumbers() => $"[{this.PlayerPool.Count}/{this._minimumPlayers}]";

        public string FormattedPlayersNeeded() => this.HasMinimumPlayers ? string.Empty : $"{this.PlayersNeeded} more players needed.";
    }
}