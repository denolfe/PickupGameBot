using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using PickupGameBot.Entities;
using PickupGameBot.Utility;
using Microsoft.Extensions.DependencyInjection;

namespace PickupGameBot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _provider;
        private readonly Configuration _config;

        public CommandHandler(IServiceProvider provider)
        {
            _provider = provider;
            _client = _provider.GetService<DiscordSocketClient>();
            _commands = _provider.GetService<CommandService>();
            _config = _provider.GetService<Configuration>();
        }

        public async Task StartAsync()
        {
            PrettyConsole.Log(LogSeverity.Info, "Commands", $"Loading commands");

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            await _commands.AddModulesAsync(typeof(Entity<>).GetTypeInfo().Assembly);

            _commands.Log += OnLogAsync;

            _client.MessageReceived += HandleCommandAsync;
            PrettyConsole.Log(LogSeverity.Info, "Commands", $"Ready, loaded {_commands.Commands.Count()} commands");
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
//            // TODO: Phase this out and implement !enable/!disable to add new channels
//            if (_config.ChannelWhitelist.All(id => id != s.Channel.Id))
//                return;
            
            var timer = new Stopwatch();
            timer.Start();

            var msg = s as SocketUserMessage;
            if (msg == null)
                return;

            var context = new SocketCommandContext(_client, msg);
            string prefix = "!";

            int argPos = 0;
            bool hasStringPrefix = msg.HasStringPrefix(prefix, ref argPos);

            if (hasStringPrefix || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _provider);
                timer.Stop();
                
                PrettyConsole.Log(LogSeverity.Info, "Commands", $"{context.Message.Content} " +
                                                                $"by {context.Message.Author.Username} - " +
                                                                $"[{timer.ElapsedMilliseconds}ms]");
                
                if (!result.IsSuccess)
                {
                    if (result is ExecuteResult r)
                        Console.WriteLine(r.Exception.ToString());
                    else if (result.Error == CommandError.UnknownCommand)
                    {
                        await PrettyConsole.LogAsync(LogSeverity.Warning, "Command",
                            $"Command '{context.Message.Content}' not recognized by {context.Message.Author.Username} in " +
                            $"{context.Message.Channel}");
//                        await context.Channel.SendMessageAsync("Command not recognized");
                    }
                    else
                        await context.Channel.SendMessageAsync(result.ToString());
                }
            }
        }

        private Task OnLogAsync(LogMessage msg)
            => PrettyConsole.LogAsync(msg.Severity, msg.Source, msg.Exception?.ToString() ?? msg.Message);
    }
}
