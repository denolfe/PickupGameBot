using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Discord;
using DiscordPugBotcore.Entities;
using DiscordPugBotcore.Enums;
using DiscordPugBotcore.Tests.Stubs;
using DiscordPugBotcore.Tests.Utility;
using Xunit;
using Game = Discord.Game;

namespace DiscordPugBotcore.Tests
{
    public class PickupServiceTests
    {
        private IServiceProvider _provider;
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
            
            Assert.Equal(true, response.Success);
            Assert.Equal(1, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldRemovePlayers()
        {
            var user = UserStub.Generate(new Random());
            _service.AddPlayer(new PugPlayer(user, true));
            var response = _service.RemovePlayer(user);

            Assert.True(response.Success);
            Assert.Equal(0, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldNotRemoveIfPlayerNotInPool()
        {
            var user = UserStub.Generate(new Random());
            var response = _service.RemovePlayer(user);
            
            Assert.False(response.Success);
            Assert.Equal("John is not in the player list.", response.Message);
            Assert.Equal(0, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldNotAddPlayerIfNotGathering()
        {
            _service.PickupState = PickupState.Starting;
            var random = new Random();
            var player = PugPlayerStub.NormalPlayer(random);
            var response = _service.AddPlayer(player);
            
            Assert.False(response.Success);
            Assert.Equal("State: Starting. New players cannot join at this time",
                response.Message);
            Assert.Equal(0, _service.PlayerPool.Count);
        }
        
        [Fact]
        public void ShouldNotAddPlayerIfAlreadyJoined()
        {
            var random = new Random();
            var user = PugPlayerStub.NormalPlayer(random);
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
            foreach (var player in playerList)
                _service.AddPlayer(player);

            Assert.Equal(_service.PlayerPool.Count, 10);
            Assert.Equal(_service.PlayerPool.Count(p => p.WantsCaptain), 5);
            Assert.True(_service.HasEnoughEligibleCaptains);
            Assert.True(_service.PlayersNeeded <= 0);
        }
    }
}
