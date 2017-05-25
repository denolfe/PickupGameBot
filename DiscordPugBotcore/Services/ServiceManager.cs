using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordPugBotcore.Entities;
using DiscordPugBotcore.Utility;
using Microsoft.Extensions.DependencyInjection;

namespace DiscordPugBotcore.Services
{
    public class ServiceManager
    {
        private readonly DiscordSocketClient _client;

        public ServiceManager(DiscordSocketClient client)
        {
            _client = client;
            _client.Log += OnLogAsync;
        }

        public async Task StartAsync()
        {
            var provider = ConfigureServices();

            var handler = provider.GetService<CommandHandler>();
            await handler.StartAsync();
        }

        private IServiceProvider ConfigureServices()
        {
            var config = Configuration.Load();

            var services = new ServiceCollection()
                .AddSingleton<CommandHandler>()
                .AddSingleton<PickupService>()
                .AddSingleton(_client)
                .AddSingleton(config)
                .AddSingleton(new CommandService(new CommandServiceConfig()
                {
                    DefaultRunMode = RunMode.Async,
                    LogLevel = LogSeverity.Info,
                    ThrowOnError = false
                }));
            
            var provider = new DefaultServiceProviderFactory().CreateServiceProvider(services);
            provider.GetService<CommandHandler>();
            provider.GetService<PickupService>();
            
            return provider;
        }

        private Task OnLogAsync(LogMessage msg)
            => PrettyConsole.LogAsync(msg.Severity, msg.Source, msg.Exception?.ToString() ?? msg.Message);
    }
}