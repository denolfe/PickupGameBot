using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordPugBotcore.Entities;
using DiscordPugBotcore.Enums;
using DiscordPugBotcore.Services;
using DiscordPugBotcore.Utility;

namespace DiscordPugBotcore.Commands
{
    public class PlayerCommands : ModuleBase
    {
        private readonly PickupService _pickupService;

        public PlayerCommands(PickupService pickupService)
        {
            _pickupService = pickupService;
        }
        
        [Command("join"), Summary("Add player to player pool")]
        [Alias("j")]
        public async Task Join([Remainder] string captain = null)
        {
            var response = _pickupService.AddPlayer(new PugPlayer(this.Context.User, captain?.Trim().ToLower() == "captain"));
            await ReplyAsync(response.Message);
        }

        [Command("leave"), Summary("Remove player from player pool")]
        [Alias("remove")]
        public async Task Leave([Remainder] string captain = null)
        {
            var response = _pickupService.RemovePlayer(this.Context.User);
            await ReplyAsync(response.Message);
        }
        
        [Command("status"), Summary("Show information about current pickup game")]
        [Alias("list")]
        public async Task List()
        {
            var response = _pickupService.Status();
            if (response.PlayerPool.Count == 0)
                await ReplyAsync(response.PickupResponse.Message);
            else
                await ReplyAsync("", embed: new PickupStatusBuilder(response).Build());
        }
        
//        [Command("status"), Summary("Status of Pug")]
//        public async Task Status()
//        {
//            await ReplyAsync($"Status: {PickupState.Gathering}, {_pickupService.FormattedPlayersNeeded()}");
//        }
    }
}