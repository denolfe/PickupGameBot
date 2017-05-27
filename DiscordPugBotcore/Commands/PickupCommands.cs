using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using DiscordPugBotcore.Entities;
using DiscordPugBotcore.Services;

namespace DiscordPugBotcore.Commands
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
            //TODO: Check players and at least 2 eligible captains
            //TODO: Choose captains
            //TODO: Set GameState
            //TODO: Notify all players in pool
            await ReplyAsync(response.Message);
        }

        [Command("pick"), Summary("Pick a certain player for team")]
        public async Task PickPlayer([Remainder] IUser user)
        {
            var response = _pickupService.PickPlayer(this.Context.User, user);
            await ReplyAsync(response.Message);
        }
    }
}