using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using PickupGameBot.Services;
using PickupGameBot.Utility;

namespace PickupGameBot.Preconditions
{
    public class ElevatedUserPrecondition
    {
        public class RequireElevatedUserAttribute : PreconditionAttribute
        {
#pragma warning disable 649
            private IServiceProvider _provider;
#pragma warning restore 649

            public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services)
            {
                var service = new PickupService(_provider);
                if (context.Guild == null)
                    return Task.FromResult(PreconditionResult.FromError("This command may only be run in a guild."));

//                var config = services.GetService<Configuration>();
//
//                if (!config.GuildRoleMap.TryGetValue(context.Guild.Id, out IEnumerable<ulong> roles))
//                    return Task.FromResult(PreconditionResult.FromError("This guild does not have a whitelist."));

                // TODO: Ability to add and store whitelisted roles
                var socketGuildUser = context.User as SocketGuildUser;
                var permittedRoles = services.GetService<PickupService>().GetPickupChannel(context).AdminGroups;
                return Task.FromResult(
                    socketGuildUser != null 
                    && (!socketGuildUser.GetPermissions(context.Channel as SocketGuildChannel).ManageChannel
                        || !socketGuildUser.Roles.Intersect(permittedRoles).Any())
                        ? PreconditionResult.FromError("You do not have permissions to manage this channel.") 
                        : PreconditionResult.FromSuccess());
            }
        }
    }
}