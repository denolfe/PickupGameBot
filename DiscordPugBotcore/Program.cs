using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Abstractions;

namespace DiscordPugBotcore
{
    public class Program
    {
        private DiscordSocketClient _client;
        private readonly CommandService _commands = new CommandService();
        private readonly IServiceCollection _map = new ServiceCollection();
        private IServiceProvider _services;
        
        public static void Main(string[] args)
            => new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Logger;

            _client.Ready += () =>
            {
                Logger(new LogMessage(LogSeverity.Info, "Bot", "Connected"));
                return Task.CompletedTask;
            };

            await InitCommands();  
            
            var token = "MjcyODI1NDAyMzc5MjA2NjU3.DAYOWA.ysI92xmFdenYCrQ_PLUDgEG6W4Y"; // Remember to keep this private!
            await _client.LoginAsync(TokenType.Bot, token);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }    
        
        private async Task InitCommands()
        {
            // Repeat this for all the service classes
            // and other dependencies that your commands might need.
            // _map.AddSingleton(new SomeServiceClass());

            // Either search the program and add all Module classes that can be found:
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
            // Or add Modules manually if you prefer to be a little more explicit:
            // await _commands.AddModuleAsync<SomeModule>();

            // When all your required services are in the collection, build the container.
            // Tip: There's an overload taking in a 'validateScopes' bool to make sure
            // you haven't made any mistakes in your dependency graph.
            _services = _map.BuildServiceProvider();

            // Subscribe a handler to see if a message invokes a command.
            _client.MessageReceived += HandleCommandAsync;
        }
        
        public async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Bail out if it's a System Message.
            var msg = messageParam as SocketUserMessage;
            if (msg == null) return;
            
            if (msg.Content[0] == '!')
                await Logger(new LogMessage(LogSeverity.Info, "Bot",
                    $"Command Received -> {msg.Content} by {msg.Author}"));

            // Create a number to track where the prefix ends and the command begins
            int pos = 0;
            // Replace the '!' with whatever character
            // you want to prefix your commands with.
            // Uncomment the second half if you also want
            // commands to be invoked by mentioning the bot instead.
            if (msg.HasCharPrefix('!', ref pos) /* || msg.HasMentionPrefix(_client.CurrentUser, ref pos) */)
            {
                // Create a Command Context
                var context = new SocketCommandContext(_client, msg);
            
                // Execute the command. (result does not indicate a return value, 
                // rather an object stating if the command executed succesfully).
                var result = await _commands.ExecuteAsync(context, pos, _services);

                // Uncomment the following lines if you want the bot
                // to send a message if it failed (not advised for most situations).
                //if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                //    await msg.Channel.SendMessageAsync(result.ErrorReason);
            }
        }

        // Create a named logging handler, so it can be re-used by addons
        // that ask for a Func<LogMessage, Task>.
        private static Task Logger(LogMessage message)
        {
            var cc = Console.ForegroundColor;
            switch (message.Severity)
            {
                case LogSeverity.Critical:
                case LogSeverity.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case LogSeverity.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case LogSeverity.Info:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case LogSeverity.Verbose:
                case LogSeverity.Debug:
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    break;
            }
            Console.WriteLine($"{DateTime.Now,-19} [{message.Severity,8}] {message.Source,7}: {message.Message}");
            Console.ForegroundColor = cc;
            return Task.CompletedTask;
        }
    }
}
