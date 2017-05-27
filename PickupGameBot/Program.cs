using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using PickupGameBot.Services;
using PickupGameBot.Utility;

namespace PickupGameBot
{
    public class Program
    {
        private DiscordSocketClient _client;
        private ServiceManager _manager;

        public static void Main(string[] args)
            => new Program().StartAsync().GetAwaiter().GetResult();

        public async Task StartAsync()
        {
            PrettyConsole.NewLine("===   Pugbot   ===");
            PrettyConsole.NewLine();

            Configuration.EnsureExists();

            _client = new DiscordSocketClient(new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 1000
            });
            
            await _client.LoginAsync(TokenType.Bot, Configuration.Load().Token.Discord);
            await _client.StartAsync();

            _manager = new ServiceManager(_client);
            await _manager.StartAsync();
            
            await Task.Delay(-1);
        }
    }
}
