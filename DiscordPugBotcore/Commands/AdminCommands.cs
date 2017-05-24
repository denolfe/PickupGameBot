using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace DiscordPugBotcore.Commands
{
    [Group("admin")]
    public class AdminCommands : ModuleBase
    {
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
        
        [Command("restart"), Summary("Go back to gather state")]
        public async Task Restart()
        {
            //TODO: Remove captains, set state to Gather
            await ReplyAsync($"{this.Context.Client.CurrentUser.Username} is now in gathering state");
        }
        
        [Command("clear"), Summary("Go back to gather state and clear player pool")]
        public async Task Clear()
        {
            //TODO: Remove captains, Mention all players in pool, clear player pool, set state to Gather
            await ReplyAsync($"All players have been removed, {this.Context.Client.CurrentUser.Username} is now in gathering state");
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