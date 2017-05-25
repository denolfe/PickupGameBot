using System.Threading.Tasks;
using Discord.Commands;
using DiscordPugBotcore.Enums;

namespace DiscordPugBotcore.Commands
{
    public class PlayerCommands : ModuleBase
    {
        [Command("join"), Summary("Add player to player pool")]
        [Alias("j")]
        public async Task Join([Remainder] string captain = null)
        {
            await ReplyAsync(
                captain?.Trim().ToLower() == "captain"
                    ? $"{this.Context.User.Username} added as eligible captain."
                    : $"{this.Context.User.Username} added.");
        }

        [Command("leave"), Summary("Remove player from player pool")]
        [Alias("remove")]
        public async Task Leave([Remainder] string captain = null)
        {
            await ReplyAsync(
                captain?.Trim().ToLower() == "captain"
                    ? $"{this.Context.User.Username} removed captain eligibility."
                    : $"{this.Context.User.Username} removed.");
        }
        
        [Command("list"), Summary("List players in pool")]
        public async Task List()
        {
            await ReplyAsync($"Player List: ...");
        }
        
        [Command("status"), Summary("Status of Pug")]
        public async Task Status()
        {
            await ReplyAsync($"Status: {PickupState.Gathering}, X more players needed");
        }
    }
}