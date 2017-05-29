using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PickupGameBot.Entities;
using PickupGameBot.Enums;
using PickupGameBot.Services;
using PickupGameBot.Utility;

namespace PickupGameBot.Commands
{
    public class PickupCommands : ModuleBase
    {
        private readonly PickupService _pickupService;
        
        public PickupCommands(PickupService pickupService)
        {
            _pickupService = pickupService;
        }
        
        [Command("gather"), Summary("Set state to gather")]
        public async Task Gather()
        {
//            _pickupService.Gather();
            await ReplyAsync($"Players can now !join");
        }
        
        [Command("start"), Summary("Set state to picking")]
        public async Task StartPicking()
        {
            var response = _pickupService.StartPicking();
            await ReplyAsync("", embed: new PickupStatusBuilder(response).Build());
        }

        [Command("pick"), Summary("Pick a certain player for team")]
        public async Task PickPlayer([Remainder] IUser user)
        {
            var response = _pickupService.PickPlayer(this.Context.User, user);
            
            if (response.State == PickupState.Starting
                && response.BothTeamsAreFull)
                await ReplyAsync("", embed: new PickupStatusBuilder(response).Build());
            else
                await ReplyAsync(response.PickupResponse.Message);
        }
    }
}