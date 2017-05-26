using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Discord;
using DiscordPugBotcore.Entities;
using DiscordPugBotcore.Enums;
using Xunit;
using Game = Discord.Game;

namespace DiscordPugBotcore.Tests
{
    public class PickupServiceTests
    {
        private IServiceProvider _provider;
        private PickupService _service;
        
        public PickupServiceTests()
        {
            _service = new PickupService(_provider);
        }
        
        [Fact]
        public void ShouldInitializeState()
        {
            Assert.Equal(PickupState.Gathering, _service.PickupState);
        }

        [Fact]
        public void ShouldAddPlayers()
        {
            var user = UserStub.Generate();
            var response = _service.AddPlayer(new PugPlayer(user, true));
            
            Assert.Equal(true, response.Success);
            Assert.Equal(1, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldRemovePlayers()
        {
            var user = UserStub.Generate();
            _service.AddPlayer(new PugPlayer(user, true));
            var response = _service.RemovePlayer(user);

            Assert.True(response.Success);
            Assert.Equal(0, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldNotRemoveIfPlayerNotInPool()
        {
            var user = UserStub.Generate();
            var response = _service.RemovePlayer(user);
            
            Assert.False(response.Success);
            Assert.Equal("John is not in the player list.", response.Message);
            Assert.Equal(0, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldNotAddPlayerIfNotGathering()
        {
            _service.PickupState = PickupState.Starting;
            var user = UserStub.Generate();
            var response = _service.AddPlayer(new PugPlayer(user, true));
            
            Assert.False(response.Success);
            Assert.Equal("State: Starting. New players cannot join at this time",
                response.Message);
            Assert.Equal(0, _service.PlayerPool.Count);
        }
        
        [Fact]
        public void ShouldNotAddPlayerIfAlreadyJoined()
        {
            var user = UserStub.Generate();
            var response1 = _service.AddPlayer(new PugPlayer(user, true));
            Assert.True(response1.Success);
            var response2 = _service.AddPlayer(new PugPlayer(user, true));
            
            Assert.False(response2.Success);
            Assert.Equal("John has already joined.",
                response2.Message);
            Assert.Equal(1, _service.PlayerPool.Count);
        }

        [Fact]
        public void ShouldAllowCaptainEligibility()
        {
            for (var i = 0; i < 5; i++)
            {
                _service.AddPlayer(new PugPlayer(UserStub.Generate(), true));
                Assert.True(_service.PlayerPool.Count == i+1);
            }
//            while (!_service.HasMinimumPlayers)
//            {
//                _service.AddPlayer(new PugPlayer(UserStub.Generate(), false));
//            }
            Assert.True(_service.PlayerPool.Count > 0);
            Assert.True(_service.HasEnoughEligibleCaptains);
            Assert.True(_service.PlayersNeeded <= 0);
        }
    }

    public class UserStub : IUser
    {
        public ulong Id { get; set; }
        public DateTimeOffset CreatedAt { get; }
        public string Mention { get; }
        public Game? Game { get; }
        public UserStatus Status { get; }

        public static UserStub Generate()
        {
            return new UserStub
            {
                Username = "John",
                Id = new ulong()
            };
        }
        
        
        public string GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
        {
            throw new NotImplementedException();
        }

        public Task<IDMChannel> GetDMChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public Task<IDMChannel> CreateDMChannelAsync(RequestOptions options = null)
        {
            throw new NotImplementedException();
        }

        public string AvatarId { get; }
        public string Discriminator { get; }
        public ushort DiscriminatorValue { get; }
        public bool IsBot { get; }
        public bool IsWebhook { get; }
        public string Username { get; set; }
    }
}
