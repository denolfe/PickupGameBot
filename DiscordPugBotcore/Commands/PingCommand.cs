using System.Threading.Tasks;
using Discord.Commands;

namespace DiscordPugBotcore.Commands
{
    public class PingCommand : ModuleBase
    {
        [Command("ping"), Summary("Pings the bot.")]
        public async Task PingBot()
        {
            await ReplyAsync("Pong!");
        }
    }
}