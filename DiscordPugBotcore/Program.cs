using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordPugBotcore.Services;
using DiscordPugBotcore.Utility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace DiscordPugBotcore
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
