using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Discord;
using PickupGameBot.Enums;
using PickupGameBot.Extensions;
using PickupGameBot.Utility;
using PickupGameBot.Validators;

namespace PickupGameBot.Entities
{
    public class PickupChannel
    {
        public IMessageChannel MessageChannel { get; set; }
        public List<PugPlayer> PlayerPool { get; set; } = new List<PugPlayer>();
        public List<PugPlayer> Captains { get; set; } = new List<PugPlayer>();
        public PugPlayer PickingCaptain { get; set; }
        public PickupState PickupState { get; private set; } = PickupState.Gathering;
        public Game CurrentGame { get; set; }
        public Game LastGame { get; set; }
        private int _pickNumber = 1;
        private int _preferredTeamSize = 0;

        public bool HasMinimumPlayers => PlayerPool.Count >= CurrentGame.MinimumPlayers;
        public bool HasEnoughEligibleCaptains => PlayerPool.Count(p => p.WantsCaptain) >= 2 || Captains.Count == 2;
        public int PlayersNeeded => CurrentGame.MinimumPlayers - PlayerPool.Count;
        private string FormattedPlayerNumbers() => $"[{PlayerPool.Count}/{CurrentGame.MinimumPlayers}]";
        private string FormattedPlayersNeeded() => HasMinimumPlayers
            ? string.Empty
            : $"Need {PlayersNeeded} more players.";
        
        public PickupChannel(IMessageChannel chan)
        {
            MessageChannel = chan;
            CurrentGame = _preferredTeamSize == 0 ? new Game() : new Game(_preferredTeamSize);
        }

        public PickupResponse GetStatus()
        {
            return PlayerPool.Count == 0 
                ? PickupResponse.NoPlayersInPool 
                : PickupResponse.Good($"**{FormattedPlayerNumbers()}**: {PlayerPool.ToFormattedList()}");
        }
       
        private PickupResponse StartPicking(string playerJoinString)
        {
            PickupState = PickupState.Picking;
            SelectCaptains(HasEnoughEligibleCaptains);
            AssignCaptains();

            // TODO: Announce captains in this string as well
            return PickupResponse.PickingToStart(playerJoinString);
        }
        
        public PickupResponse AddPlayerToPool(IUser user, bool wantsCaptain)
        {
            var pugPlayer = new PugPlayer(user, wantsCaptain);
            
            var validator = new AddPlayerValidator();
            var validationResponse = validator.Validate(this, pugPlayer);
            if (validationResponse != null)
                return validationResponse;

            PlayerPool.Add(pugPlayer);
            var captainMessage = pugPlayer.WantsCaptain ? " as eligible captain" : string.Empty;

            return HasMinimumPlayers 
                ? StartPicking($"{pugPlayer.User.Username} joined{captainMessage}.")
                : PickupResponse.Good($"{pugPlayer.User.Username} joined{captainMessage}.");
        }

        public PickupResponse RemovePlayerFromPool(IUser user)
        {
            var validator = new RemovePlayerValidator();
            var validationResponse = validator.Validate(this, user);
            if (validationResponse != null)
                return validationResponse;
            
            PlayerPool = PlayerPool.RemovePlayer(user);
            return PickupResponse.RemovedFromList(user.Username);
        }
        
        public PickupResponse PickPlayer(IUser captain, IUser user)
        {
            var validator = new PickPlayerValidator();
            var validationResponse = validator.Validate(this, captain, user);
            if (validationResponse != null)
                return validationResponse;
            
            var pickedPlayer = PlayerPool.GetPlayer(user);
            CurrentGame.AddToCaptainsTeam(PickingCaptain, pickedPlayer);
            
            // Remove from Player Pool
            PlayerPool = PlayerPool.RemovePlayer(pickedPlayer);

            // Check if full AFTER the pick
            if (CurrentGame.BothTeamsAreFull())
            {
                var gameString = CurrentGame.ToString();
                PickingFinished();
                return PickupResponse.PickingCompleted(gameString);
            }

            SetNextCaptain();

            return PickupResponse.Good($"{user.Username} has been picked by {captain.Username}"+
                                                         $" to Team {Captains.GetPlayer(captain).TeamId}\n" +
                                                         $"{PickingCaptain.User.Username}'s Pick");
        }

        public PickupResponse Reset()
        {
            var removedPlayers = PickingFinished(true);
            var response = PickupResponse.PickupReset;
            response.Messages.Add(removedPlayers.ToFormattedList(true));
            return response;
        }

        private List<PugPlayer> PickingFinished(bool resetFlag = false)
        {
            var removedPlayers = CurrentGame.RemoveTeams();
            removedPlayers.AddRange(PlayerPool);
            PlayerPool = new List<PugPlayer>();
            Captains = new List<PugPlayer>();
            
            if (!resetFlag)
            {
                CurrentGame.Picked = true;
                LastGame = CurrentGame;
            }
            
            CurrentGame = new Game();
            _pickNumber = 1;
            PickupState = PickupState.Gathering;
            return removedPlayers;
        }

        public PickupResponse Repick()
        {
            if (PickupState != PickupState.Picking)
                return PickupResponse.NotInPickingState;
            
            PlayerPool.AddRange(CurrentGame.Repick());
            _pickNumber = 1;
            PickingCaptain = Captains.FirstOrDefault(c => c.TeamId == 1);
            return PickupResponse.Good("Players have been moved back to the player pool.");
        }
        
        public PickupResponse SetTeamSize(string value)
        {
            if (PickupState != PickupState.Gathering)
                return PickupResponse.Bad("Cannot set team size when not in Gathering state.");

            int teamSize;
            var isNumber = int.TryParse(value, out teamSize);

            if (teamSize % 2 != 0)
                return PickupResponse.Bad("Team size must be an even number");

            if (isNumber)
            {
                CurrentGame.MinimumPlayers = teamSize;
                _preferredTeamSize = teamSize;
                return PickupResponse.Good($"Team size successfully set to **{CurrentGame.MinimumPlayers}**");
            }
            else
                return PickupResponse.Bad("Invalid value for team size.");
        }
        
        private void SelectCaptains(bool enoughEligibleCaptains)
        {
            if (enoughEligibleCaptains)
            {
                // Select 2 players that have WantsCaptain flag at random
                Captains = PlayerPool
                    .Where(p => p.WantsCaptain)
                    .OrderBy(x => Guid.NewGuid())
                    .Take(2)
                    .ToList();
            }
            else
            {
                // Select 2 players at random
                Captains = PlayerPool
                    .OrderBy(x => Guid.NewGuid())
                    .Take(2)
                    .ToList();
            }

            // Set IsCaptain flag
            Captains.ForEach(p => p.SetCaptain());

            // Remove captains from normal player pool
            foreach (var pugPlayer in Captains)
            {
                if (PlayerPool.ContainsPlayer(pugPlayer))
                    PlayerPool = PlayerPool.RemovePlayer(pugPlayer);
            }
        }
        
        private void AssignCaptains()
        {
            Captains.ElementAt(0).TeamId = 1;
            Captains.ElementAt(1).TeamId = 2;
            CurrentGame.CreateTeams(Captains);
            PickingCaptain = Captains.FirstOrDefault(c => c.TeamId == 1);
            PrettyConsole.Log(LogSeverity.Debug, "Bot",
                $"Assigned Captains: {Captains.OrderBy(c => c.TeamId).ToList().ToFormattedList()}");
        }
        
        private void SetNextCaptain()
        {
            if (_pickNumber >= CurrentGame.MinimumPlayers)
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

            PickingCaptain = CurrentGame.GetCaptainFromId(pickMap[_pickNumber]);
        }

        private PickupStatus BuildPickupStatus(PickupResponse puResponse) 
            => new PickupStatus(
                PickupState,
                PlayerPool,
                CurrentGame,
                puResponse
            );
    }
}