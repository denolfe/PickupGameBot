using System;
using System.Linq;
using PickupGameBot.Entities;
using PickupGameBot.Enums;
using PickupGameBot.Services;
using PickupGameBot.Tests.Stubs;
using Xunit;

namespace PickupGameBot.Tests
{
    public class PickupServiceTests
    {
#pragma warning disable 649
        private IServiceProvider _provider;
#pragma warning restore 649
        
        private PickupService _service;
        private Random _rand;
        
        public PickupServiceTests()
        {
            _service = new PickupService(_provider);
            _rand = new Random();
        }
        
        [Fact]
        public void ShouldInitializeState()
        {
            Assert.Equal(PickupState.Gathering, _service.PickupState);
        }

        [Fact]
        public void ShouldAddPlayers()
        {
            var user = UserStub.Generate(new Random());
            var response = _service.AddPlayer(new PugPlayer(user, true));
            
            Assert.True(response.Success);
            Assert.Equal(1, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldRemovePlayers()
        {
            var user = UserStub.Generate(_rand);
            _service.AddPlayer(new PugPlayer(user, true));
            var response = _service.RemovePlayer(user);

            Assert.True(response.Success);
            Assert.Equal(0, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldNotRemoveIfPlayerNotInPool()
        {
            var user = UserStub.Generate(_rand);
            var response = _service.RemovePlayer(user);
            
            Assert.False(response.Success);
            Assert.Equal("John is not in the player list.", response.Message);
            Assert.Equal(0, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldNotAddPlayerIfNotGathering()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5, _rand);
            playerList.ForEach(p => _service.AddPlayer(p));
            var pickResponse = _service.StartPicking();
            Assert.True(pickResponse.Success);

            var latePlayer = PugPlayerStub.NormalPlayer(_rand);
            var joinResponse = _service.AddPlayer(latePlayer);
            
            Assert.False(joinResponse.Success);
            Assert.Equal("State: Picking. New players cannot join at this time",
                joinResponse.Message);
            
            // 10 - 2 captains = 8
            Assert.Equal(8, _service.PlayerPool.Count);
        }
        
        [Fact]
        public void ShouldNotAddPlayerIfAlreadyJoined()
        {
            var user = PugPlayerStub.NormalPlayer(_rand);
            var response1 = _service.AddPlayer(user);
            Assert.True(response1.Success);
            var response2 = _service.AddPlayer(user);
            
            Assert.False(response2.Success);
            Assert.Equal("John has already joined.",
                response2.Message);
            Assert.Equal(1, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldAllowCaptainEligibility()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5, _rand);
            playerList.ForEach(p => _service.AddPlayer(p));

            Assert.Equal(10, _service.PlayerPool.Count);
            Assert.Equal(5, _service.PlayerPool.Count(p => p.WantsCaptain));
            Assert.True(_service.HasEnoughEligibleCaptains);
            Assert.True(_service.PlayersNeeded <= 0);
        }

        [Fact]
        public void ShouldNotAllowStartIfNotEnoughPlayers()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 4, _rand);
            playerList.ForEach(p => _service.AddPlayer(p));

            var response = _service.StartPicking();
            
            Assert.False(response.Success);
            Assert.Equal($"Not enough players in pool {_service.FormattedPlayerNumbers()}." +
                         $" {_service.FormattedPlayersNeeded()}", response.Message);
        }

        [Fact]
        public void ShouldSelectTwoCaptains()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5, _rand);
            playerList.ForEach(p => _service.AddPlayer(p));
            Assert.True(_service.HasEnoughEligibleCaptains);
            
            var response = _service.StartPicking();
            Assert.True(response.Success);
            Assert.True(_service.HasCorrectCaptains);
            Assert.NotEqual(null, _service.PickingCaptain);
            Assert.Equal(8, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldRandomlySelectCaptainsIfNotEnough()
        {
            var playerList = PugPlayerStub.GeneratePlayers(0, 10, _rand);
            playerList.ForEach(p => _service.AddPlayer(p));
            Assert.False(_service.HasEnoughEligibleCaptains);
            
            var response = _service.StartPicking();
            Assert.True(response.Success);
            Assert.True(_service.HasCorrectCaptains);
            Assert.NotEqual(null, _service.PickingCaptain);
            Assert.Equal(8, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldAssignCaptainsToTeams()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5, _rand);
            playerList.ForEach(p => _service.AddPlayer(p));

            var response = _service.StartPicking();
            Assert.True(response.Success);
            
            // Only 1 captain per team
            Assert.Equal(1, _service.Captains.Count(c => c.TeamId == 1));
            Assert.Equal(1, _service.Captains.Count(c => c.TeamId == 2));

            // Each captain has a Team Id
            Assert.Equal(1, _service.Team1.Captain.TeamId);
            Assert.Equal(2, _service.Team2.Captain.TeamId);
            
            // Each team has Id
            Assert.Equal(1, _service.Team1.Id);
            Assert.Equal(2, _service.Team2.Id);
            
            // Team 1 picks first
            Assert.Equal(1, _service.PickingCaptain.TeamId);
        }
        
        [Fact]
        public void ShouldBeAbleToPickPlayersToTeam()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5, _rand);
            playerList.ForEach(p => _service.AddPlayer(p));

            var response = _service.StartPicking();
            Assert.True(response.Success);
            
            var randomPlayerInPool = _service.PlayerPool.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();

            var pickResponse = _service.PickPlayer(_service.PickingCaptain.User, randomPlayerInPool.User);
            Assert.True(pickResponse.Success);
            
            // Confirm player was added to team
            Assert.Equal(1, _service.Team1.Players.Count);
            
            // Picking captain should change to 2
            Assert.Equal(2, _service.PickingCaptain.TeamId);
        }

        [Fact (Skip = "Pending")]
        public void ShouldReturnDetailedResponseToStatus()
        {
        }
    }
}