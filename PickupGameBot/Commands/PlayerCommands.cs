using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using PickupGameBot.Entities;
using PickupGameBot.Enums;
using PickupGameBot.Services;
using PickupGameBot.Utility;

namespace PickupGameBot.Commands
{
    public class PlayerCommands : ModuleBase
    {
        private readonly PickupService _pickupService;
        
        private Task BuildMessageAsync(PickupStatus response)
        {
            return ReplyAsync(string.Join("\n", response.PickupResponse.Messages),
                embed: new PickupStatusBuilder(response).Build());
        }
        
        public PlayerCommands(PickupService pickupService)
        {
            _pickupService = pickupService;
        }
        
        [Command("join"), Summary("Add player to player pool")]
        [Alias("j")]
        public async Task Join([Remainder] string captain = null)
        {
            var response = _pickupService.AddPlayer(new PugPlayer(this.Context.User, captain?.Trim().ToLower() == "captain"));
            await BuildMessageAsync(response);
        }

        [Command("leave"), Summary("Remove player from player pool")]
        [Alias("remove")]
        public async Task Leave([Remainder] string captain = null)
        {
            var response = _pickupService.RemovePlayer(this.Context.User);
            await BuildMessageAsync(response);
        }
        
        [Command("status"), Summary("Show information about current pickup game")]
        [Alias("list")]
        public async Task List()
        {
            var response = _pickupService.Status();
            await BuildMessageAsync(response);
        }
    }
}