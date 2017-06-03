using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PickupGameBot.Entities;
using PickupGameBot.Extensions;
using PickupGameBot.Preconditions;
using PickupGameBot.Services;
using PickupGameBot.Utility;

namespace PickupGameBot.Modules
{
    [ElevatedUserPrecondition.RequireElevatedUserAttribute]
    [Group("admin")]
    public class AdminModule : ModuleBase
    {
        private readonly PickupService _pickupService;

//        private Task BuildMessageAsync(PickupStatus response)
//        {
//            return ReplyAsync(string.Join("\n", response.PickupResponse.Messages),
//                embed: new PickupStatusBuilder(response).Build());
//        }
        
        public AdminModule(PickupService pickupService)
        {
            _pickupService = pickupService;
        }
        
        [Command("enable"), Summary("Enable pickups in current channel")]
        public async Task EnablePickups()
        {
            var response = _pickupService.EnablePickups(Context);
            await ReplyAsync(response.Messages.First());
        }
        
        [Command("disable"), Summary("Disable pickups in current channel")]
        public async Task DisablePickups()
        {
            var response = _pickupService.DisablePickups(Context);
            await ReplyAsync(response.Messages.First());
        }
        
        [Command("add"), Summary("Force add player")]
        public async Task ForceAdd([Remainder, Summary("The user to force add")] IUser user)
        {
            throw new NotImplementedException();
        }
        
        [Command("remove"), Summary("Force add player")]
        [Alias("kick")]
        public async Task ForceRemove([Remainder, Summary("The user to force remove")] IUser user)
        {
            throw new NotImplementedException();
        }
        
        [Command("captain"), Summary("Force set user as a captain")]
        public async Task ForceCaptain([Remainder, Summary("The user to set as captain")] IUser user)
        {
            throw new NotImplementedException();
        }
        
        [Command("repick"), Summary("Go back to gather state")]
        public async Task Repick()
        {
        }
        
        [Command("reset"), Summary("Go back to gather state and clear player pool")]
        public async Task Reset()
        {
            //TODO: Remove captains, Mention all players in pool, clear player pool, set state to Gather
            var response = _pickupService.Reset(Context);
            await ReplyAsync(response.Messages.First());
        }
    }
}