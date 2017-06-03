using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Bogus;
using Discord;
using Discord.Commands;
using PickupGameBot.Tests.Utility;

namespace PickupGameBot.Tests.Stubs
{
    public class CommandContextStub : ICommandContext
    {
        public IDiscordClient Client { get; set; }
        public IGuild Guild { get; set; }
        public IMessageChannel Channel { get; set; }
        public IUser User { get; set; }
        public IUserMessage Message { get; set; }

        public static CommandContextStub Generate(Random rand)
            => new Faker<CommandContextStub>()
                .RuleFor(u => u.Channel, f => MessageChannelStub.Generate(rand))
                .RuleFor(u => u.User, f => UserStub.Generate(rand));
    }
}