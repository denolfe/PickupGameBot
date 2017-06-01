using System.Threading.Tasks;
using Discord.Commands;

namespace PickupGameBot.Modules
{
    public class PingModule : ModuleBase
    {
        [Command("ping"), Summary("Pings the bot.")]
        public async Task PingBot()
        {
            await ReplyAsync("Pong!");
        }
    }
}