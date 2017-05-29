using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PickupGameBot.Entities;
using PickupGameBot.Extensions;
using PickupGameBot.Preconditions;
using PickupGameBot.Services;

namespace PickupGameBot.Commands
{
    [ElevatedUserPrecondition.RequireElevatedUserAttribute]
    [Group("admin")]
    public class AdminCommands : ModuleBase
    {
        private readonly PickupService _pickupService;

        public AdminCommands(PickupService pickupService)
        {
            _pickupService = pickupService;
        }
        
        [Command("add"), Summary("Force add player")]
        public async Task ForceAdd([Remainder, Summary("The user to force add")] IUser user)
        {
            //TODO: Ensure user not already added, then add
            await ReplyAsync($"{this.Context.User.Username} force added {user.Mention}");
        }
        
        [Command("remove"), Summary("Force add player")]
        [Alias("kick")]
        public async Task ForceRemove([Remainder, Summary("The user to force remove")] IUser user)
        {
            //TODO: Ensure user is in pool, then remove
            await ReplyAsync($"{this.Context.User.Username} force removed {user.Mention}");
        }
        
        [Command("captain"), Summary("Force set user as a captain")]
        public async Task ForceCaptain([Remainder, Summary("The user to set as captain")] IUser user)
        {
            //TODO: Ensure user is in pool and is not set as captain, then set as captain
            await ReplyAsync($"{this.Context.User.Username} force set {user.Mention} as captain");
        }
        
        [Command("repick"), Summary("Go back to gather state")]
        public async Task Repick()
        {
            var response = _pickupService.Repick();
            await ReplyAsync(response.Message);
        }
        
        [Command("reset"), Summary("Go back to gather state and clear player pool")]
        public async Task Reset()
        {
            //TODO: Remove captains, Mention all players in pool, clear player pool, set state to Gather
            var response = _pickupService.Reset();
            var mentionString = response.Item2.ToFormattedList(true);
            
            await ReplyAsync(response.Item1.Message + $"\nNotifying players: {mentionString}");
        }
        
        [Command("pause"), Summary("Pauses the bot")]
        public async Task Pause()
        {
            //TODO: Pause bot in its current state
            await ReplyAsync($"{this.Context.Client.CurrentUser.Username} has been paused");
        }
        
        [Command("unpause"), Summary("Unpauses the bot")]
        public async Task Unpause()
        {
            //TODO: Unpause bot and continue
            await ReplyAsync($"{this.Context.Client.CurrentUser.Username} has been unpaused");
        }
    }
}