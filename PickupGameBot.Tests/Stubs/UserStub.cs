using System;
using System.Threading.Tasks;
using Bogus;
using Discord;
using PickupGameBot.Tests.Utility;

namespace PickupGameBot.Tests.Stubs
{
    public class UserStub : IUser
    {
        public ulong Id { get; set; }
        public DateTimeOffset CreatedAt { get; }
        public string Mention { get; set; }
        public Game? Game { get; }
        public UserStatus Status { get; }

        public static UserStub Generate(Random rand)
            => new Faker<UserStub>()
                .RuleFor(u => u.Username, f => f.Name.FirstName())
                .RuleFor(u => u.Mention, (f, u) => $"@{u.Username}")
                .RuleFor(u => u.Id, f => rand.NextUInt64());
        
        public string GetAvatarUrl(ImageFormat format = ImageFormat.Auto, ushort size = 128)
            => throw new NotImplementedException();

        public Task<IDMChannel> GetOrCreateDMChannelAsync(RequestOptions options = null) 
            => throw new NotImplementedException();

        public Task<IDMChannel> GetDMChannelAsync(CacheMode mode = CacheMode.AllowDownload, RequestOptions options = null) 
            => throw new NotImplementedException();

        public Task<IDMChannel> CreateDMChannelAsync(RequestOptions options = null) 
            => throw new NotImplementedException();

        public string AvatarId { get; }
        public string Discriminator { get; }
        public ushort DiscriminatorValue { get; }
        public bool IsBot { get; }
        public bool IsWebhook { get; }
        public string Username { get; set; }
    }

}