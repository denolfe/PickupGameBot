using System;
using PickupGameBot.Entities;
using PickupGameBot.Tests.Stubs;
using Xunit;

namespace PickupGameBot.Tests.Tests
{
    public class TeamTests
    {
        private Team _team;
        private Random _rand;
        
        public TeamTests()
        {
            _rand = new Random();
        }

        [Fact]
        public void ShouldInitializeTeamWithCaptainAsFirstPlayer()
        {
            var captain = PugPlayerStub.EligibleCaptain(_rand);
            _team = new Team(1, captain, 5);
            
            Assert.Equal(1, _team.Players.Count);
            Assert.Equal(1, _team.Id);
            Assert.NotNull(_team.Captain);
            Assert.Equal(5, _team.MaxPlayers);
            Assert.False(_team.IsFull());
        }
        
        [Fact]
        public void ShouldAddPlayer()
        {
            var captain = PugPlayerStub.EligibleCaptain(_rand);
            _team = new Team(1, captain, 5);
            
            _team.AddPlayer(PugPlayerStub.NormalPlayer(_rand));
            
            Assert.Equal(2, _team.Players.Count);
        }
        
        [Fact]
        public void ShouldShowTeamAsFull()
        {
            var captain = PugPlayerStub.EligibleCaptain(_rand);
            _team = new Team(1, captain, 5);

            PugPlayerStub.GeneratePlayers(0, 4)
                .ForEach(p => _team.AddPlayer(p));
            
            Assert.Equal(5, _team.Players.Count);
            Assert.True(_team.IsFull());
        }
    }
}