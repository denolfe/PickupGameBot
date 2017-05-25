using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordPugBotcore.Entities;
using DiscordPugBotcore.Enums;

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
            _pickupService.AddPlayer(new PugPlayer(this.Context.User, captain?.Trim().ToLower() == "captain"));
            
            await ReplyAsync(
                captain?.Trim().ToLower() == "captain"
                    ? $"{this.Context.User.Username} joined as eligible captain."
                    : $"{this.Context.User.Username} joined.");
        }

        [Command("leave"), Summary("Remove player from player pool")]
        [Alias("remove")]
        public async Task Leave([Remainder] string captain = null)
        {
            _pickupService.RemovePlayer(this.Context.User);
            
            await ReplyAsync(
                captain?.Trim().ToLower() == "captain"
                    ? $"{this.Context.User.Username} removed captain eligibility."
                    : $"{this.Context.User.Username} has left.");
        }
        
        [Command("list"), Summary("List players in pool")]
        public async Task List()
        {
            var playerList = _pickupService.PlayerPool
                .Select(p => p.ToString())
                .ToList();

            await ReplyAsync(
                playerList.Count <= 0
                    ? "_No players in player pool. Type !join to be added to the player pool._"
                    : $"Player Pool {_pickupService.FormattedPlayerNumbers()}: {string.Join(",", playerList)}"
            );
        }
        
        [Command("status"), Summary("Status of Pug")]
        public async Task Status()
        {
            await ReplyAsync($"Status: {PickupState.Gathering}, {_pickupService.FormattedPlayersNeeded()}");
        }
    }
}