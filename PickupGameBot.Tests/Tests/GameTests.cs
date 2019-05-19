using System;
using System.Collections.Generic;
using PickupGameBot.Entities;
using PickupGameBot.Tests.Stubs;
using Xunit;

namespace PickupGameBot.Tests.Tests
{
    public class GameTests
    {
        private Game _game;
        private Random _rand;
        
        public GameTests()
        {
            _game = new Game();
            _rand = new Random();
        }
        
        [Fact]
        public void ShouldInitializeMinimumPlayers()
        {
            Assert.Equal(10, _game.MinimumPlayers);
        }

        [Fact]
        public void ShouldCreateTeamsFromCaptains()
        {
            var captain1 = new PugPlayer(UserStub.Generate(_rand), true);
            var captain2 = new PugPlayer(UserStub.Generate(_rand), true);
            captain1.TeamId = 1;
            captain2.TeamId = 2;
            var captains = new List<PugPlayer> {captain1, captain2};
            
            _game.CreateTeams(captains);
            
            Assert.Equal(2, _game.Teams.Count);
            Assert.Equal(1, _game.Teams[1].Captain.TeamId);
            Assert.Equal(2, _game.Teams[2].Captain.TeamId);
        }

        [Fact]
        public void ShouldAllowAddingPlayersByCaptain()
        {
            var captain1 = new PugPlayer(UserStub.Generate(_rand), true);
            var captain2 = new PugPlayer(UserStub.Generate(_rand), true);
            captain1.TeamId = 1;
            captain2.TeamId = 2;
            var captains = new List<PugPlayer> {captain1, captain2};
            _game.CreateTeams(captains);
            
            _game.AddToCaptainsTeam(captain1, new PugPlayer(UserStub.Generate(_rand), false));
            
            Assert.Equal(2, _game.Teams[1].Players.Count);
            Assert.Single(_game.Teams[2].Players);
        }
        
        [Fact]
        public void ShouldAllowRemovingOfTeams()
        {
            var captain1 = new PugPlayer(UserStub.Generate(_rand), true);
            var captain2 = new PugPlayer(UserStub.Generate(_rand), true);
            captain1.TeamId = 1;
            captain2.TeamId = 2;
            var captains = new List<PugPlayer> {captain1, captain2};
            _game.CreateTeams(captains);
            
            _game.AddToCaptainsTeam(captain1, new PugPlayer(UserStub.Generate(_rand), false));
            _game.AddToCaptainsTeam(captain2, new PugPlayer(UserStub.Generate(_rand), false));
            var removedPlayers = _game.RemoveTeams();
            
            Assert.Empty(_game.Teams);
            Assert.Equal(4, removedPlayers.Count);
        }
        
        [Fact]
        public void ShouldAllowRepicking()
        {
            var captain1 = new PugPlayer(UserStub.Generate(_rand), true);
            var captain2 = new PugPlayer(UserStub.Generate(_rand), true);
            captain1.TeamId = 1;
            captain2.TeamId = 2;
            var captains = new List<PugPlayer> {captain1, captain2};
            _game.CreateTeams(captains);
            
            _game.AddToCaptainsTeam(captain1, new PugPlayer(UserStub.Generate(_rand), false));
            _game.AddToCaptainsTeam(captain2, new PugPlayer(UserStub.Generate(_rand), false));
            var removedPlayers = _game.Repick();
            
            Assert.Single(_game.Teams[1].Players);
            Assert.Single(_game.Teams[2].Players);
            Assert.NotNull(_game.Teams[2].Captain);
            Assert.NotNull(_game.Teams[2].Captain);
        }

        [Fact]
        public void ShouldReturnFalseTeamsAreFullWhenNoTeams()
        {
            Assert.False(_game.BothTeamsAreFull());
        }

        [Fact]
        public void ShouldReturnTrueWhenTeamsAreFull()
        {
            var captain1 = new PugPlayer(UserStub.Generate(_rand), true);
            var captain2 = new PugPlayer(UserStub.Generate(_rand), true);
            captain1.TeamId = 1;
            captain2.TeamId = 2;
            var captains = new List<PugPlayer> {captain1, captain2};
            _game.CreateTeams(captains);

            // Add 8 more players
            for (int i = 0; i < 8; i++)
            {
                _game.AddToCaptainsTeam(
                    i % 2 == 0 ? captain1 : captain2,
                        new PugPlayer(UserStub.Generate(_rand), false));
            }
            
            Assert.True(_game.BothTeamsAreFull());
        }
    }
}