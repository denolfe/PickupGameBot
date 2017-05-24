using System;
using System.Collections.Generic;
using DiscordPugBotcore.Enums;

namespace DiscordPugBotcore.Entities
{
    public class Pickup
    {
        public Game CurrentGame;
        private int _minimumPlayers;
        public List<PugPlayer> PlayerPool { get; set; }
        public List<PugPlayer> Subs { get; set; }
        public bool HasMinimumPlayers => this.PlayerPool.Count >= this._minimumPlayers;
        public PickupState PickupState;

        public Pickup(int minimumPlayers = 10)
        {
            this._minimumPlayers = minimumPlayers;
            this.PickupState = PickupState.Gathering;
            this.PlayerPool = new List<PugPlayer>();
        }

        public void Start()
        {
            if (!this.HasMinimumPlayers)
            {
                Console.WriteLine($"Not enough players in pool [{this.PlayerPool.Count}/{this._minimumPlayers}]");
            }
                
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

        public void RemovePlayer(PugPlayer pugPlayer)
        {
            if (this.PlayerPool.Contains(pugPlayer))
            {
                this.PlayerPool.Remove(pugPlayer);
                Console.WriteLine($"{pugPlayer.User.Username} successfully removed from list.");
            }
            else
            {
                Console.WriteLine($"{pugPlayer.User.Username} is not in the player list.");
            }
        }

        public string Status()
        {
            return $"Status: {this.PickupState} - {this.PlayerPool.Count}/{this._minimumPlayers}";
        }
    }
}