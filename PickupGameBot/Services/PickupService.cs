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
        private int _minimumPlayers;
        private int _pickNumber = 1;
        private readonly IServiceProvider _provider;
        
        public List<PugPlayer> Captains { get; set; }
        public List<PugPlayer> PlayerPool { get; set; } = new List<PugPlayer>();
        public List<PugPlayer> Subs { get; set; }
        public PickupState PickupState { get; private set; } = PickupState.Gathering;
        
        public bool HasMinimumPlayers => this.PlayerPool.Count >= this._minimumPlayers;
        public bool HasEnoughEligibleCaptains => this.PlayerPool.Count(p => p.WantsCaptain) >= 2;
        public bool HasCorrectCaptains => this.Captains.Count == 2;
        public int PlayersNeeded => this._minimumPlayers - this.PlayerPool.Count;

        public bool BothTeamsAreFull()
        {
            if (this.Team1 == null || this.Team2 == null)
                return false;
            
            return this.Team1.IsFull() && this.Team2.IsFull();
        }

        public PickupService(IServiceProvider provider, int minimumPlayers = 10)
        {
            this._provider = provider;
            this._minimumPlayers = minimumPlayers;
        }

        public PickupStatus StartPicking()
        {
            if (!this.HasMinimumPlayers)
                return BuildPickupStatus(PickupResponse.Bad(
                    $"Not enough players in pool {this.FormattedPlayerNumbers()}. {this.FormattedPlayersNeeded()}"));

            this.PickupState = PickupState.Picking;
            
            this.SelectCaptains(this.HasEnoughEligibleCaptains);
            this.AssignCaptains();

            return BuildPickupStatus(PickupResponse.PickingToStart);
        }

        public PickupStatus AddPlayer(PugPlayer pugPlayer, bool admin = false)
        {
            if (this.PickupState != PickupState.Gathering)
                return BuildPickupStatus(PickupResponse.Bad($"State: {this.PickupState}. New players cannot join at this time"));

            if (this.PlayerPool.ContainsPlayer(pugPlayer))
                return BuildPickupStatus(PickupResponse.AlreadyJoined(pugPlayer.User.Username));
            
            this.PlayerPool.Add(pugPlayer);
            var captainMessage = pugPlayer.WantsCaptain ? " as eligible captain" : string.Empty;
            
            //TODO: Add conditional if pug is full and start picking
            
            if (this.PlayerPool.Count == this._minimumPlayers)
                return BuildPickupStatus(PickupResponse.Good($"{pugPlayer.User.Username} joined{captainMessage}.\n" + 
                                    "Pickup has enough players to start!"));

            return BuildPickupStatus(
                admin 
                    ? PickupResponse.Good($"{pugPlayer.User.Username} was force added.") 
                    : PickupResponse.Good($"{pugPlayer.User.Username} joined{captainMessage}."));
        }

        public PickupStatus RemovePlayer(IUser user)
        {
            if (!this.PlayerPool.ContainsPlayer(user))
                return BuildPickupStatus(PickupResponse.NotInPlayerList(user.Username));
            
            this.PlayerPool = this.PlayerPool.RemovePlayer(user);
            return BuildPickupStatus(PickupResponse.SuccessfullyRemoved(user.Username));
        }

        public PickupStatus PickPlayer(IUser captain, IUser user)
        {
            if (BothTeamsAreFull())
                return BuildPickupStatus(PickupResponse.TeamsFull);
            
            if (user == null)
                return BuildPickupStatus(PickupResponse.NoPlayersLeftInPool);
            
            if (this.PickupState != PickupState.Picking)
                return BuildPickupStatus(PickupResponse.NotInPickingState);
            
            if (!this.Captains.ContainsPlayer(captain))
                return BuildPickupStatus(PickupResponse.NotCaptain(captain.Username));   

            if (this.Captains.GetPlayer(captain).TeamId != PickingCaptain.TeamId)
                return BuildPickupStatus(PickupResponse.NotPick(user.Username));

            if (!this.PlayerPool.ContainsPlayer(user))
                return BuildPickupStatus(PickupResponse.NotInPlayerList(user.Username));

            var playerFromUser = this.PlayerPool.GetPlayer(user);
            if (this.PickingCaptain.TeamId == 1)
                this.Team1.AddPlayer(playerFromUser);
            else
                this.Team2.AddPlayer(playerFromUser);

            // Remove from Player Pool
            this.PlayerPool = this.PlayerPool.RemovePlayer(playerFromUser);
            
            // Check if full after the pick
            if (BothTeamsAreFull())
            {
                // TODO: take a look at how states are used. This somehow needs to set back to gather!
                this.PickupState = PickupState.Starting;
                return BuildPickupStatus(PickupResponse.PickingCompleted);
            }
                
            SetNextCaptain();
            
            return BuildPickupStatus(PickupResponse.Good($"{user.Username} has been picked by {captain.Username}" +
                                       $" to Team {this.Captains.GetPlayer(captain).TeamId}\n" +
                                       $"{this.PickingCaptain.User.Username}'s Pick"));
        }

        public PickupStatus Status()
        {
            return BuildPickupStatus(
                this.PlayerPool.Count == 0 
                    ? PickupResponse.NoPlayersInPool 
                    : PickupResponse.Good(this.FormattedPlayersNeeded()));
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
            {
                if (this.PlayerPool.ContainsPlayer(pugPlayer))
                    this.PlayerPool = this.PlayerPool.RemovePlayer(pugPlayer);
            }
                
        }
        
        private void AssignCaptains()
        {
            var captain1 = this.Captains.ElementAt(0);
            var captain2 = this.Captains.ElementAt(1);
            this.Captains.ElementAt(0).TeamId = 1;
            this.Captains.ElementAt(1).TeamId = 2;
            this.Team1 = new Team(1, captain1, _minimumPlayers/2);
            this.Team2 = new Team(2, captain2, _minimumPlayers/2);
            this.PickingCaptain = this.Team1.Captain;
            PrettyConsole.Log(LogSeverity.Debug, "Bot", $"Assigned Captains: {this.Captains.OrderBy(c => c.TeamId).ToList().ToFormattedList()}");
        }
        
        private void SetNextCaptain()
        {
            if (_pickNumber >= _minimumPlayers)
                return;
            
            _pickNumber++;
            var pickMap = new Dictionary<int, int>
            {
                {1, 1}, // Should never happen, set initially to 1
                {2, 2},
                {3, 2},
                {4, 1},
                {5, 2},
                {6, 1},
                {7, 2},
                {8, 1},
                {9, 2},
                {10, 1},
                {11, 1},
                {12, 1}
            };

            this.PickingCaptain =
                pickMap[_pickNumber] == 1
                    ? this.Team1.Captain
                    : this.Team2.Captain;
        }

        private PickupStatus BuildPickupStatus(PickupResponse puResponse)
        {
            return new PickupStatus(
                this.PickupState,
                this._minimumPlayers,
                this.Captains,
                this.PlayerPool,
                this.Team1,
                this.Team2,
                this.BothTeamsAreFull(),
                puResponse
                );
        }

        public PickupStatus Repick()
        {
            if (this.PickupState != PickupState.Picking)
                return BuildPickupStatus(PickupResponse.NotInPickingStateRepick);
            
            
            if (this.Team1?.Players.Count > 0)
                this.PlayerPool.AddRange(this.Team1.PopAll());
            if (this.Team2?.Players.Count > 0)
                this.PlayerPool.AddRange(this.Team2.PopAll());

            // Remove captains from normal player pool
            foreach (var pugPlayer in this.Captains)
            {
                if (this.PlayerPool.ContainsPlayer(pugPlayer))
                    this.PlayerPool = this.PlayerPool.RemovePlayer(pugPlayer);
            }
            
            this.PickingCaptain = this.Team1?.Captain;

            return BuildPickupStatus(PickupResponse.PickingRestarted);
        }

        // TODO: Use PickupStatus as response in tuple
        public Tuple<PickupResponse,List<PugPlayer>> Reset()
        {
            var currentPool = this.PlayerPool;
            if (this.Team1?.Captain != null)
                currentPool.Add(this.Team1.Captain);
            if (this.Team2?.Captain != null)
                currentPool.Add(this.Team2.Captain);
            this.PlayerPool = new List<PugPlayer>();
            this.Team1 = null;
            this.Team2 = null;
            this.PickupState = PickupState.Gathering;
            return Tuple.Create<PickupResponse, List<PugPlayer>>(
                    PickupResponse.PickupReset,
                    currentPool
                ); 
        }

        public string FormattedPlayerNumbers() => $"[{this.PlayerPool.Count}/{this._minimumPlayers}]";

        public string FormattedPlayersNeeded() => this.HasMinimumPlayers ? string.Empty : $"Need {this.PlayersNeeded} more players.";
    }
}