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
        
        [Command("teamsize"), Summary("Set team size")]
        public async Task SetTeamSize([Remainder, Summary("The teamsize value")] string value)
        {
            var response = _pickupService.SetTeamSize(Context, value);
            await ReplyAsync(response.Messages.First());
        }
        
        [Command("fadd"), Summary("Force add player")]
        public async Task ForceAdd([Remainder, Summary("The user to force add")] IUser user)
        {
            //TODO: Force adding captains
            var response = _pickupService.ForceAddPlayer(Context, user);
            await ReplyAsync(response.Messages.First());
        }
        
        [Command("fremove"), Summary("Force add player")]
        [Alias("kick")]
        public async Task ForceRemove([Remainder, Summary("The user to force remove")] IUser user)
        {
            var response = _pickupService.ForceRemovePlayer(Context, user);
            await ReplyAsync(response.Messages.First());
        }
        
        [Command("fcaptain"), Summary("Force set user as a captain")]
        public async Task ForceCaptain([Remainder, Summary("The user to set as captain")] IUser user)
        {
            throw new NotImplementedException();
        }
        
        [Command("repick"), Summary("Go back to gather state")]
        public async Task Repick()
        {
            var response = _pickupService.Repick(Context);
            await ReplyAsync(response.JoinedMessages);
        }
        
        [Command("reset"), Summary("Go back to gather state and clear player pool")]
        public async Task Reset()
        {
            var response = _pickupService.Reset(Context);
            await ReplyAsync(response.JoinedMessages);
        }
    }
}