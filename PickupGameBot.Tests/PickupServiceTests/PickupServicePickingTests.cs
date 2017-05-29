using System;
using System.Linq;
using PickupGameBot.Entities;
using PickupGameBot.Enums;
using PickupGameBot.Services;
using PickupGameBot.Tests.Stubs;
using Xunit;

namespace PickupGameBot.Tests.PickupServiceTests
{
    public class PickupServicePickingTests
    {
#pragma warning disable 649
        private IServiceProvider _provider;
#pragma warning restore 649
        
        private PickupService _service;
        
        public PickupServicePickingTests()
        {
            _service = new PickupService(_provider);
        }

        private PugPlayer GetRandomPlayer() 
            => _service.PlayerPool.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();

        [Fact]
        public void Pick1ShouldBeCaptain1()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5);
            playerList.ForEach(p => _service.AddPlayer(p));

            var response = _service.StartPicking();
            Assert.True(response.Success);
            
            var firstPickingCaptain = _service.PickingCaptain.User;
            Assert.Equal(1, _service.PickingCaptain.TeamId);
        }
        
        [Fact]
        public void Pick2ShouldBeCaptain2()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5);
            playerList.ForEach(p => _service.AddPlayer(p));

            var response = _service.StartPicking();
            Assert.True(response.Success);
            
            var firstPickingCaptain = _service.PickingCaptain.User;
            var pickResponse = _service.PickPlayer(firstPickingCaptain, GetRandomPlayer().User);
            Assert.True(pickResponse.Success);
            
            // Picking captain should change to 2
            Assert.Equal(2, _service.PickingCaptain.TeamId);
            
            var pickResponse2 = _service.PickPlayer(_service.PickingCaptain.User, GetRandomPlayer().User);
            Assert.True(pickResponse2.Success);
            Assert.Equal(1, _service.Team2.Players.Count);
        }

        [Fact]
        public void Pick3ShouldBeCaptain2()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5);
            playerList.ForEach(p => _service.AddPlayer(p));

            var response = _service.StartPicking();
            Assert.True(response.Success);
            
            var randomPlayerInPool = GetRandomPlayer();

            var firstPickingCaptain = _service.PickingCaptain.User;
            var pickResponse = _service.PickPlayer(firstPickingCaptain, GetRandomPlayer().User);
            Assert.True(pickResponse.Success);
            
            // Pick 2 should be Captain 2
            Assert.Equal(2, _service.PickingCaptain.TeamId);
            
            var pickResponse2 = _service.PickPlayer(_service.PickingCaptain.User, GetRandomPlayer().User);
            Assert.True(pickResponse2.Success);
            Assert.Equal(1, _service.Team2.Players.Count);
            
            // Pick 3 should be Captain 2 AGAIN
            Assert.Equal(2, _service.PickingCaptain.TeamId);
            
            var pickResponse3 = _service.PickPlayer(_service.PickingCaptain.User, GetRandomPlayer().User);
            Assert.True(pickResponse3.Success);
            Assert.Equal(2, _service.Team2.Players.Count);
            
            // Pick 4 should be Captain 1
            Assert.Equal(1, _service.PickingCaptain.TeamId);
            
            var pickResponse4 = _service.PickPlayer(_service.PickingCaptain.User, GetRandomPlayer().User);
            Assert.True(pickResponse4.Success);
            Assert.Equal(2, _service.Team1.Players.Count);
            
            // Pick 5 should be Captain 2
            Assert.Equal(2, _service.PickingCaptain.TeamId);
        }
        
        [Fact]
        public void Pick4ShouldBeCaptain1()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5);
            playerList.ForEach(p => _service.AddPlayer(p));

            var response = _service.StartPicking();
            Assert.True(response.Success);
            
            var randomPlayerInPool = GetRandomPlayer();

            var firstPickingCaptain = _service.PickingCaptain.User;
            var pickResponse = _service.PickPlayer(firstPickingCaptain, GetRandomPlayer().User);
            Assert.True(pickResponse.Success);
            
            // Pick 2 should be Captain 2
            Assert.Equal(2, _service.PickingCaptain.TeamId);
            
            var pickResponse2 = _service.PickPlayer(_service.PickingCaptain.User, GetRandomPlayer().User);
            Assert.True(pickResponse2.Success);
            Assert.Equal(1, _service.Team2.Players.Count);
            
            // Pick 3 should be Captain 2 AGAIN
            Assert.Equal(2, _service.PickingCaptain.TeamId);
            
            var pickResponse3 = _service.PickPlayer(_service.PickingCaptain.User, GetRandomPlayer().User);
            Assert.True(pickResponse3.Success);
            Assert.Equal(2, _service.Team2.Players.Count);
            
            // Pick 4 should be Captain 1
            Assert.Equal(1, _service.PickingCaptain.TeamId);
        }
        
        [Fact]
        public void Pick5ShouldBeCaptain2()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5);
            playerList.ForEach(p => _service.AddPlayer(p));

            var response = _service.StartPicking();
            Assert.True(response.Success);
            
            var randomPlayerInPool = GetRandomPlayer();

            var firstPickingCaptain = _service.PickingCaptain.User;
            var pickResponse = _service.PickPlayer(firstPickingCaptain, GetRandomPlayer().User);
            Assert.True(pickResponse.Success);
            
            // Pick 2 should be Captain 2
            Assert.Equal(2, _service.PickingCaptain.TeamId);
            
            var pickResponse2 = _service.PickPlayer(_service.PickingCaptain.User, GetRandomPlayer().User);
            Assert.True(pickResponse2.Success);
            Assert.Equal(1, _service.Team2.Players.Count);
            
            // Pick 3 should be Captain 2 AGAIN
            Assert.Equal(2, _service.PickingCaptain.TeamId);
            
            var pickResponse3 = _service.PickPlayer(_service.PickingCaptain.User, GetRandomPlayer().User);
            Assert.True(pickResponse3.Success);
            Assert.Equal(2, _service.Team2.Players.Count);
            
            // Pick 4 should be Captain 1
            Assert.Equal(1, _service.PickingCaptain.TeamId);
            
            var pickResponse4 = _service.PickPlayer(_service.PickingCaptain.User, GetRandomPlayer().User);
            Assert.True(pickResponse4.Success);
            Assert.Equal(2, _service.Team1.Players.Count);
            
            // Pick 5 should be Captain 2
            Assert.Equal(2, _service.PickingCaptain.TeamId);
        }

        [Fact]
        public void ShouldNotAllowPickingIfTeamsAreFull()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5);
            playerList.ForEach(p => _service.AddPlayer(p));

            var response = _service.StartPicking();
            Assert.True(response.Success);
            
            for (int i = 1; i <= 10; i++)
            {
                var pickingCaptain = _service.PickingCaptain.User;
                var pickResponse = _service.PickPlayer(pickingCaptain, GetRandomPlayer().User);
            }
            
            var additionalPickAttempt = _service.PickPlayer(_service.PickingCaptain.User, GetRandomPlayer().User);
            Assert.False(additionalPickAttempt.Success);
            Assert.Equal("Teams are full, you cannot pick any more players.", additionalPickAttempt.Message);
        }
    }
}