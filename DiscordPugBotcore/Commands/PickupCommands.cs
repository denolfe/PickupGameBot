using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordPugBotcore.Commands
{
    public class PickupCommands : ModuleBase
    {
        [Command("gather"), Summary("Set state to gather")]
        public async Task Gather()
        {
            await ReplyAsync($"Players can now !join");
        }
        
        [Command("start"), Summary("Set state to picking")]
        public async Task StartPicking()
        {
            //TODO: Check players and at least 2 eligible captains
            //TODO: Choose captains
            //TODO: Set GameState
            //TODO: Notify all players in pool
            await ReplyAsync($"Picking has started");
        }
    }
}