using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PickupGameBot.Enums;
using PickupGameBot.Services;
using PickupGameBot.Tests.Stubs;
using Xunit;

namespace PickupGameBot.Tests.Tests
{
    public class PickupServiceAdminTests
    {
#pragma warning disable 649
        private IServiceProvider _provider;
#pragma warning restore 649
        private readonly PickupService _service;
        private readonly Random _rand;
        private readonly CommandContextStub _context;

        public PickupServiceAdminTests()
        {
            _service = new PickupService(_provider);
            _rand = new Random();
            _context = CommandContextStub.Generate(_rand);
            var response = _service.EnablePickups(_context);
        }

        [Fact]
        public void ResetShouldRemoveAllPlayersFromPool()
        {
            // 9 Players
            for (var i = 0; i < 9; i++)
            {
                var user = UserStub.Generate(_rand);
                _context.User = user;
                _service.AddPlayer(_context, true);
            }
            Assert.Equal(9, _service.GetPickupChannel(_context).PlayerPool.Count);
            
            var response = _service.Reset(_context);

            Assert.True(response.Success);
            Assert.Equal(0, _service.GetPickupChannel(_context).PlayerPool.Count);
        }

        [Fact]
        public void RepickShouldRemoveAllPlayersFromTeams()
        {
            for (var i = 0; i < 10; i++)
            {
                var user = UserStub.Generate(_rand);
                _context.User = user;
                _service.AddPlayer(_context, true);
            }

            var randomPlayerInPool = _service
                .GetPickupChannel(_context)
                .PlayerPool
                .OrderBy(x => Guid.NewGuid())
                .Take(1)
                .FirstOrDefault();

            var pickupChannel = _service.GetPickupChannel(_context);
            Assert.Equal(2, pickupChannel.Captains.Count);
            var firstPickingCaptain = pickupChannel.PickingCaptain.User;
            var pickResponse = pickupChannel.PickPlayer(firstPickingCaptain, randomPlayerInPool.User);
            Assert.True(pickResponse.Success);

            var randomPlayerInPool2 = pickupChannel.PlayerPool.OrderBy(x => Guid.NewGuid()).Take(1).FirstOrDefault();
            var pickResponse2 = pickupChannel
                .PickPlayer(pickupChannel.PickingCaptain.User, randomPlayerInPool2.User);
            Assert.True(pickResponse2.Success);

            var repickResponse = pickupChannel.Repick();
            Assert.True(repickResponse.Success);

            // Picking captain should reset
            Assert.Equal(1, pickupChannel.PickingCaptain.TeamId);

            // Should not reset captains
            Assert.Equal(2, pickupChannel.Captains.Count);

            // 8 players left after 2 captains already on a team
            Assert.Equal(8, pickupChannel.PlayerPool.Count);
        }

        [Fact]
        public void RepickShouldNotBeAllowedDuringGather()
        {
            for (var i = 0; i < 9; i++)
            {
                var user = UserStub.Generate(_rand);
                _context.User = user;
                _service.AddPlayer(_context, true);
            }

            var pickupChannel = _service.GetPickupChannel(_context);
            var repickResponse = pickupChannel.Repick();
            Assert.False(repickResponse.Success);
        }

        [Fact]
        public void ShouldSetTeamSize()
        {
            var user = UserStub.Generate(_rand);
            _context.User = user;
            _service.AddPlayer(_context, true);

            _service.SetTeamSize(_context, "6");
            
            var pickupChannel = _service.GetPickupChannel(_context);
            Assert.Equal(12, pickupChannel.CurrentGame.MinimumPlayers);
        }

        // TODO: Implement this test
//        [Fact]
//        public void CanAddAdminRole()
//        {
//            var user = UserStub.Generate(_rand);
//            _context.User = user;
//            _service.EnablePickups(_context);
//            _service.AddAdminRole();
//        }
    }
}