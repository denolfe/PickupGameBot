using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using PickupGameBot.Entities;
using PickupGameBot.Services;
using PickupGameBot.Tests.Stubs;
using Xunit;

namespace PickupGameBot.Tests.Tests
{
    public class PickupChannelTests
    {
#pragma warning disable 649
        private IServiceProvider _provider;
        private IMessageChannel _messageChannel;
#pragma warning restore 649

        private PickupChannel _channel;
        private Random _rand;
        
        public PickupChannelTests()
        {
            _rand = new Random();
            _messageChannel = MessageChannelStub.Generate(_rand);
            _channel = new PickupChannel(_messageChannel);
        }

        private PugPlayer GetRandomPlayer() 
            => _channel.PlayerPool.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();

        [Fact]
        public void ShouldInitializeGameAndPlayerPool()
        {
            Assert.NotNull(_channel.CurrentGame);
            Assert.Equal(0, _channel.PlayerPool.Count);
        }
        
#region Add/Remove Players
        [Fact]
        public void ShouldAddPlayersToPool()
        {
            var user = UserStub.Generate(_rand);
            var response = _channel.AddPlayerToPool(user, false);
            Assert.True(response.Success);
            Assert.Contains("joined", response.JoinedMessages);
            Assert.Equal(1, _channel.PlayerPool.Count);
        }
        
        [Fact]
        public void ShouldAddPlayersToPoolAndFlagAsEligibleCaptain()
        {
            var user = UserStub.Generate(_rand);
            var response = _channel.AddPlayerToPool(user, true);
            Assert.True(response.Success);
            Assert.Contains("joined", response.JoinedMessages);
            Assert.Equal(1, _channel.PlayerPool.Where(p => p.WantsCaptain).ToList().Count);
        }
        
        [Fact]
        public void ShouldNotAddPlayerIfAlreadyJoined()
        {
            var user = UserStub.Generate(_rand);
            var response1 = _channel.AddPlayerToPool(user, false);
            Assert.True(response1.Success);
            var response2 = _channel.AddPlayerToPool(user, false);
            Assert.False(response2.Success);
            Assert.Contains("already joined", response2.JoinedMessages);
        }
        
        [Fact]
        public void ShouldRemovePlayers()
        {
            var user = UserStub.Generate(_rand);
            var response1 = _channel.AddPlayerToPool(user, false);
            var response2 = _channel.RemovePlayerFromPool(user);
            Assert.True(response2.Success);
            Assert.Contains("removed", response2.JoinedMessages);
            Assert.Equal(0, _channel.PlayerPool.Count);
        }

        [Fact]
        public void ShouldNotRemoveIfPlayerNotInPool()
        {
            var user = UserStub.Generate(_rand);
            var response = _channel.RemovePlayerFromPool(user);
            Assert.False(response.Success);
            Assert.Contains("not in the player list", response.JoinedMessages);
            Assert.Equal(0, _channel.PlayerPool.Count);
        }

        [Fact]
        public void ShouldNotAddPlayerIfNotGathering()
        {
            for (int i = 0; i < 10; i++)
            {
                var user = UserStub.Generate(_rand);
                var response = _channel.AddPlayerToPool(user, true);
            }

            var latePlayer = UserStub.Generate(_rand);
            var joinResponse = _channel.AddPlayerToPool(latePlayer, false);
            
            Assert.False(joinResponse.Success);
            Assert.Contains("cannot join", joinResponse.JoinedMessages);
        }
        
        [Fact]
        public void ShouldAllowCaptainEligibility()
        {
            for (int i = 0; i < 9; i++) // Only add nine to check captain eligibility before picking
            {
                var user = UserStub.Generate(_rand);
                var response = _channel.AddPlayerToPool(user, i < 2); // 2 captains
            }
            
            Assert.Equal(2, _channel.PlayerPool.Count(p => p.WantsCaptain));
        }

        [Fact]
        public void ShouldSelectTwoCaptains()
        {
            for (int i = 0; i < 10; i++)
            {
                var user = UserStub.Generate(_rand);
                var response = _channel.AddPlayerToPool(user, i < 2); // 2 captains
            }
            
            Assert.True(_channel.HasEnoughEligibleCaptains);
            Assert.Equal(2, _channel.Captains.Count);
        }

        [Fact]
        public void ShouldRandomlySelectCaptainsIfNotEnough()
        {
            for (int i = 0; i < 10; i++)
            {
                var user = UserStub.Generate(_rand);
                var response = _channel.AddPlayerToPool(user, false); // No eligible captains
            }
            
            Assert.True(_channel.HasEnoughEligibleCaptains);
            Assert.Equal(2, _channel.Captains.Count);
        }

        [Fact]
        public void ShouldAssignCaptainsToTeams()
        {
            for (int i = 0; i < 10; i++)
            {
                var user = UserStub.Generate(_rand);
                var response = _channel.AddPlayerToPool(user, false); // No eligible captains
            }
            
            Assert.True(_channel.Captains.Count(c => c.TeamId == 1) == 1);
            Assert.True(_channel.Captains.Count(c => c.TeamId == 2) == 1);
        }

#endregion
        
#region Picking
        [Fact]
        public void ShouldBeAbleToPickPlayersToTeam()
        {
            for (int i = 0; i < 10; i++)
            {
                var user = UserStub.Generate(_rand);
                var response = _channel.AddPlayerToPool(user, i < 5);
            }
            
            var randomPlayerInPool = _channel.PlayerPool.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();

            var pickResponse = _channel.PickPlayer(_channel.PickingCaptain.User, randomPlayerInPool.User);
            Assert.True(pickResponse.Success);
            Assert.Equal(2, _channel.CurrentGame.Teams[1].Players.Count); // Captain + 1 Player
            Assert.Equal(7, _channel.PlayerPool.Count);
        }

        [Fact]
        public void ShouldAlternatePicksCorrectly()
        {
            for (int i = 0; i < 10; i++)
            {
                var user = UserStub.Generate(_rand);
                var response = _channel.AddPlayerToPool(user, i < 5);
            }
            
            // Picking Order: Team 1 pick
            //                Team 2 pick
            //                Team 2 pick
            //                Team 1 pick
            //                Alternate...
            
            var randomPlayerInPool = _channel.PlayerPool.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
            var pickResponse = _channel.PickPlayer(_channel.PickingCaptain.User, randomPlayerInPool.User);
            Assert.True(pickResponse.Success);
            Assert.Equal(2, _channel.CurrentGame.Teams[1].Players.Count); // Captain + 1 Player
            Assert.Equal(7, _channel.PlayerPool.Count);
            
            var randomPlayerInPool2 = _channel.PlayerPool.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
            var pickResponse2 = _channel.PickPlayer(_channel.PickingCaptain.User, randomPlayerInPool2.User);
            Assert.True(pickResponse2.Success);
            Assert.Equal(2, _channel.CurrentGame.Teams[2].Players.Count); // Captain + 1 Player
            Assert.Equal(6, _channel.PlayerPool.Count);
            
            var randomPlayerInPool3 = _channel.PlayerPool.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
            var pickResponse3 = _channel.PickPlayer(_channel.PickingCaptain.User, randomPlayerInPool3.User);
            Assert.True(pickResponse3.Success);
            Assert.Equal(3, _channel.CurrentGame.Teams[2].Players.Count); // Captain + 2 Player
            Assert.Equal(5, _channel.PlayerPool.Count);
            
            var randomPlayerInPool4 = _channel.PlayerPool.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
            var pickResponse4 = _channel.PickPlayer(_channel.PickingCaptain.User, randomPlayerInPool4.User);
            Assert.True(pickResponse4.Success);
            Assert.Equal(3, _channel.CurrentGame.Teams[1].Players.Count); // Captain + 2 Player
            Assert.Equal(3, _channel.CurrentGame.Teams[2].Players.Count); // Captain + 2 Player
            Assert.Equal(4, _channel.PlayerPool.Count);
        }
        
        [Fact]
        public void ShouldNotAllowPickingIfTeamsAreFull()
        {
            // 10 players
            for (int i = 0; i < 10; i++)
            {
                var user = UserStub.Generate(_rand);
                var response = _channel.AddPlayerToPool(user, i < 5);
            }
            
            // Pick all players
            for (int i = 0; i < 8; i++)
            {
                var pickResponse = _channel.PickPlayer(_channel.PickingCaptain.User, GetRandomPlayer().User);
                Assert.True(pickResponse.Success);
            }
            
            var additionalPickAttempt = _channel.PickPlayer(_channel.PickingCaptain.User, UserStub.Generate(_rand));
            Assert.False(additionalPickAttempt.Success);
            Assert.Contains("Teams are full", additionalPickAttempt.JoinedMessages);
        }
#endregion
    }
}
