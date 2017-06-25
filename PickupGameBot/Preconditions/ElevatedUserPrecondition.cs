using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using PickupGameBot.Utility;

namespace PickupGameBot.Preconditions
{
    public class ElevatedUserPrecondition
    {
        public class RequireElevatedUserAttribute : PreconditionAttribute
        {
            public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider map)
            {
                if (context.Guild == null)
                    return Task.FromResult(PreconditionResult.FromError("This command may only be run in a guild."));

//                var config = map.GetService<Configuration>();
//
//                if (!config.GuildRoleMap.TryGetValue(context.Guild.Id, out IEnumerable<ulong> roles))
//                    return Task.FromResult(PreconditionResult.FromError("This guild does not have a whitelist."));

                // TODO: Ability to add and store whitelisted roles
                var socketGuildUser = context.User as SocketGuildUser;
                return Task.FromResult(
                    socketGuildUser != null && !socketGuildUser.GetPermissions(context.Channel as SocketGuildChannel).ManageChannel 
                        ? PreconditionResult.FromError("You do not have permissions to manage this channel.") 
                        : PreconditionResult.FromSuccess());
            }
        }
    }
}