using System;
using PickupGameBot.Services;
using PickupGameBot.Tests.Stubs;
using Xunit;

namespace PickupGameBot.Tests.Tests
{
    public class PickupServiceTests
    {
        private readonly IServiceProvider _provider;
        private PickupService _service;
        private Random _rand;
        
        public PickupServiceTests()
        {
            _service = new PickupService(_provider);
            _rand = new Random();
        }

        [Fact]
        public void ShouldBeAbleToEnablePickupsOnAChannel()
        {
            var response = _service.EnablePickups(CommandContextStub.Generate(_rand));
            Assert.Single(_service.PickupChannels);
        }
        
        [Fact]
        public void ShouldBeAbleToDisablePickupsOnAChannel()
        {
            _service.EnablePickups(CommandContextStub.Generate(_rand));
            var response = _service.DisablePickups(CommandContextStub.Generate(_rand));
            Assert.Empty(_service.PickupChannels);
        }

        [Fact]
        public void ShouldNotAllowSameChannelToBeEnabledIfAlreadyIs()
        {
            var channel = CommandContextStub.Generate(_rand);
            _service.EnablePickups(channel);
            var response = _service.EnablePickups(channel);
            Assert.False(response.Success);
            Assert.Single(_service.PickupChannels);
        }
        
        [Fact]
        public void ShouldNotAllowDisablingAChannelThatHasNotBeenEnabled()
        {
            var channel = CommandContextStub.Generate(_rand);
            var response = _service.DisablePickups(channel);
            Assert.False(response.Success);
            Assert.Empty(_service.PickupChannels);
        }
        
    }
}