using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using PickupGameBot.Entities;
using PickupGameBot.Enums;
using PickupGameBot.Extensions;
using PickupGameBot.Utility;

namespace PickupGameBot.Services
{
    public class PickupService
    {
        public Team Team1;
        public Team Team2;
        public PugPlayer PickingCaptain = null;
        private int _minimumPlayers = 10;
        private readonly IServiceProvider _provider;
        
        public List<PugPlayer> Captains { get; set; }
        public List<PugPlayer> PlayerPool { get; set; } = new List<PugPlayer>();
        public List<PugPlayer> Subs { get; set; }
        public PickupState PickupState { get; private set; } = PickupState.Gathering;
        
        public bool HasMinimumPlayers => this.PlayerPool.Count >= this._minimumPlayers;
        public bool HasEnoughEligibleCaptains => this.PlayerPool.Count(p => p.WantsCaptain) >= 2;
        public bool HasCorrectCaptains => this.Captains.Count == 2;
        public int PlayersNeeded => this._minimumPlayers - this.PlayerPool.Count;

        public PickupService(IServiceProvider provider, int minimumPlayers = 10)
        {
            this._provider = provider;
            this._minimumPlayers = minimumPlayers;
        }

        public PickupResponse StartPicking()
        {
            if (!this.HasMinimumPlayers)
                return PickupResponse.Bad(
                    $"Not enough players in pool {this.FormattedPlayerNumbers()}. {this.FormattedPlayersNeeded()}");

            this.PickupState = PickupState.Picking;
            
            this.SelectCaptains(this.HasEnoughEligibleCaptains);
            this.AssignCaptains();
            PrettyConsole.Log(LogSeverity.Debug, "Bot", "Assigned Captains");

            var message = "Picking is about to start!\n" +
                          $"Captains: {this.Captains.ToJoinedList()}\n" +
                          $"Player Pool: {string.Join(",", this.PlayerPool.Select(p => p.User.Username))}\n" +
                          $"{this.PickingCaptain.User.Username} picks first";
            
            return PickupResponse.Good(message);
        }

        public PickupResponse AddPlayer(PugPlayer pugPlayer)
        {
            if (this.PickupState != PickupState.Gathering)
                return PickupResponse.Bad($"State: {this.PickupState}. New players cannot join at this time");

            if (this.PlayerPool.ContainsPlayer(pugPlayer))
                return PickupResponse.Bad($"{pugPlayer.User.Username} has already joined.");
            
            this.PlayerPool.Add(pugPlayer);
            var captainMessage = pugPlayer.WantsCaptain ? " as eligible captain" : string.Empty;
            return PickupResponse.Good($"{pugPlayer.User.Username} joined{captainMessage}.");
        }

        public PickupResponse RemovePlayer(IUser user)
        {
            if (!this.PlayerPool.ContainsPlayer(user))
                return PickupResponse.Bad($"{user.Username} is not in the player list.");
            
            this.PlayerPool = this.PlayerPool.RemovePlayer(user);
            return PickupResponse.Good($"{user.Username} successfully removed from list.");
        }

        public PickupResponse PickPlayer(IUser captain, IUser user)
        {
            //TODO: This can be removed once attributes to validate user are added
            if (!this.Captains.ContainsPlayer(captain))
                return PickupResponse.Bad($"{captain.Username} is not a captain!");   

            if (this.Captains.GetPlayer(captain).TeamId != PickingCaptain.TeamId)
                return PickupResponse.Bad($"It is not {captain.Username}'s pick!");

            if (!this.PlayerPool.ContainsPlayer(user))
                PickupResponse.Bad($"{user.Username} is not in the player pool");

            var playerFromUser = this.PlayerPool.GetPlayer(user);
            if (this.PickingCaptain.TeamId == 1)
            {
                this.Team1.AddPlayer(playerFromUser);
                this.PickingCaptain = this.Team2.Captain;
            }
            else
            {
                this.Team2.AddPlayer(playerFromUser);
                this.PickingCaptain = this.Team1.Captain;
            }
                
            
            return PickupResponse.Good($"{user.Username} has been picked by {captain.Username} to Team {this.Captains.GetPlayer(captain).TeamId}");
        }

        public PickupStatus Status()
        {
            if (this.PlayerPool.Count == 0)
                return BuildPickupStatus(PickupResponse.Good(
                    "No players in player pool. Type **!join** to be added to the player pool."
                ));
            
            return BuildPickupStatus(PickupResponse.Good(this.FormattedPlayersNeeded()));
        }
        
        

        private void SelectCaptains(bool enoughEligibleCaptains)
        {
            if (enoughEligibleCaptains)
            {
                // Select 2 players that have WantsCaptain flag at random
                this.Captains = this.PlayerPool
                    .Where(p => p.WantsCaptain)
                    .OrderBy(x => Guid.NewGuid())
                    .Take(2)
                    .ToList();
            }
            else
            {
                // Select 2 players at random
                this.Captains = this.PlayerPool
                    .OrderBy(x => Guid.NewGuid())
                    .Take(2)
                    .ToList();
            }
            
            // Set IsCaptain flag
            this.Captains.ForEach(p => p.SetCaptain());
            
            // Remove captains from normal player pool
            foreach (var pugPlayer in this.Captains)
                this.PlayerPool = this.PlayerPool.RemovePlayer(pugPlayer);
        }
        
        private void AssignCaptains()
        {
            var captain1 = this.Captains.ElementAt(0);
            var captain2 = this.Captains.ElementAt(1);
            this.Captains.ElementAt(0).TeamId = 1;
            this.Captains.ElementAt(1).TeamId = 2;
            this.Team1 = new Team(1, captain1);
            this.Team2 = new Team(2, captain2);
            this.PickingCaptain = this.Team1.Captain;
        }

        private PickupStatus BuildPickupStatus(PickupResponse puResponse)
        {
            return new PickupStatus(
                this.PickupState,
                this._minimumPlayers,
                this.Captains,
                this.PlayerPool,
                puResponse
                );
        }

        public PickupResponse Repick()
        {
            this.PlayerPool.AddRange(this.Team1.PopAll());
            this.PlayerPool.AddRange(this.Team2.PopAll());
            this.PickingCaptain = this.Team1.Captain;
            return PickupResponse.Good("Picking has restarted."); 
        }

        public Tuple<PickupResponse,List<PugPlayer>> Reset()
        {
            var currentPool = this.PlayerPool;
            if (this.Team1?.Captain != null)
                currentPool.Add(this.Team1.Captain);
            if (this.Team2?.Captain != null)
                currentPool.Add(this.Team2.Captain);
            this.PlayerPool.Clear();
            this.Team1 = null;
            this.Team2 = null;
            this.PickupState = PickupState.Gathering;
            return Tuple.Create<PickupResponse, List<PugPlayer>>(
                    PickupResponse.Good("Pickup game has been reset."),
                    currentPool
                ); 
        }

//        public string Status() => $"Status: {this.PickupState} - {this.FormattedPlayerNumbers()}";

        public string FormattedPlayerNumbers() => $"[{this.PlayerPool.Count}/{this._minimumPlayers}]";

        public string FormattedPlayersNeeded() => this.HasMinimumPlayers ? string.Empty : $"Need {this.PlayersNeeded} more players.";
    }
}