using System;
using System.Linq;
using System.Collections.Generic;
using PickupGameBot.Entities;
using PickupGameBot.Tests.Stubs;
using Xunit;

namespace PickupGameBot.Tests.Meta
{
    public class PugPlayerStubTests
    {
        private Random _rand;
        
        public PugPlayerStubTests()
        {
            _rand = new Random();
        }
        
        [Fact]
        public void UniquePugPlayer()
        {
            var player1 = new PugPlayer(UserStub.Generate(_rand), true);
            var player2 = new PugPlayer(UserStub.Generate(_rand), true);
            Assert.NotEqual(player1.User.Id, player2.User.Id);
        }
        
        [Fact]
        public void UniquePugPlayerStatic()
        {
            var player1 = PugPlayerStub.EligibleCaptain(_rand);
            var player2 = PugPlayerStub.EligibleCaptain(_rand);
            Assert.NotEqual(player1.User.Id, player2.User.Id);
        }
        
        [Fact]
        public void PugPlayerStubShouldDeclareCaptainEligibility()
        {
            var player1 = PugPlayerStub.EligibleCaptain(_rand);
            Assert.True(player1.WantsCaptain);
        }
        
        [Fact]
        public void PugPlayerStubShouldDeclareNormalPlayersNotEligibleForCaptain()
        {
            var player1 = PugPlayerStub.NormalPlayer(_rand);
            Assert.False(player1.WantsCaptain);
        }

        [Fact]
        public void GeneratePlayersShouldGenerateCorrectly()
        {
            List<PugPlayer> playerList = PugPlayerStub.GeneratePlayers(4, 6, _rand);
            Assert.Equal(10, playerList.Count);
            Assert.Equal(4, playerList.Count(p => p.WantsCaptain));
        }
    }
}