using System;
using System.Linq;
using PickupGameBot.Services;
using PickupGameBot.Tests.Stubs;
using Xunit;

namespace PickupGameBot.Tests.PickupServiceTests
{
    public class PickupServiceAdminTests
    {
#pragma warning disable 649
        private IServiceProvider _provider;
#pragma warning restore 649
        
        private PickupService _service;
        private Random _rand;
        
        public PickupServiceAdminTests()
        {
            _service = new PickupService(_provider);
            _rand = new Random();
        }

        [Fact]
        public void ResetShouldRemoveAllPlayersFromPool()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5, _rand);
            playerList.ForEach(p => _service.AddPlayer(p));

            var response = _service.Reset();
            
            Assert.True(response.Item1.Success);
        }

        [Fact]
        public void RepickShouldRemoveAllPlayersFromTeams()
        {
            var playerList = PugPlayerStub.GeneratePlayers(5, 5, _rand);
            playerList.ForEach(p => _service.AddPlayer(p));

            var response = _service.StartPicking();
            Assert.True(response.Success);
            
            var randomPlayerInPool = _service.PlayerPool.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();

            var firstPickingCaptain = _service.PickingCaptain.User;
            var pickResponse = _service.PickPlayer(firstPickingCaptain, randomPlayerInPool.User);
            Assert.True(pickResponse.Success);
            
            var randomPlayerInPool2 = _service.PlayerPool.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
            var pickResponse2 = _service.PickPlayer(_service.PickingCaptain.User, randomPlayerInPool2.User);
            Assert.True(pickResponse2.Success);

            var repickResponse = _service.Repick();
            Assert.True(repickResponse.Success);
            
            // Picking captain should reset
            Assert.Equal(1, _service.PickingCaptain.TeamId);
            
            // Should not reset captains
            Assert.NotNull(_service.Team1.Captain);
            Assert.NotNull(_service.Team2.Captain);
            
            // 8 players left after 2 captains already on a team
            Assert.Equal(8, _service.PlayerPool.Count);
        }
    }
}