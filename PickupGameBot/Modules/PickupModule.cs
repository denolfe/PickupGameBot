using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using PickupGameBot.Entities;
using PickupGameBot.Services;
using PickupGameBot.Utility;

namespace PickupGameBot.Modules
{
    public class PickupModule : ModuleBase
    {
        private readonly PickupService _pickupService;

//        private Task BuildMessageAsync(PickupStatus response)
//        {
//            return ReplyAsync(string.Join("\n", response.PickupResponse.Messages),
//                embed: new PickupStatusBuilder(response).Build());
//        }
        
        public PickupModule(PickupService pickupService)
        {
            _pickupService = pickupService;
        }
        
        [Command("pick"), Summary("Pick a certain player for team")]
        public async Task PickPlayer([Remainder] IUser user)
        {
            var response = _pickupService.PickPlayer(Context, user);
            await ReplyAsync(response.Messages.First());
        }
        
        [Command("join"), Summary("Add player to player pool")]
        [Alias("j")]
        public async Task Join([Remainder] string captain = null)
        {
            var wantsCaptain = captain?.Trim().ToLower() == "captain";
            var response = _pickupService.AddPlayer(Context, wantsCaptain);
            await ReplyAsync(response.Messages.First());
        }

        [Command("leave"), Summary("Remove player from player pool")]
        [Alias("remove")]
        public async Task Leave([Remainder] string captain = null)
        {
            var response = _pickupService.RemovePlayer(Context);
            await ReplyAsync(response.Messages.First());
        }
        
        [Command("status"), Summary("Show information about current pickup game")]
        [Alias("list")]
        public async Task List()
        {
            var response = _pickupService.Status(Context);
            await ReplyAsync("", embed: new EmbedBuilder().WithColor(new Color(0, 255, 0))
                .AddField(new EmbedFieldBuilder()
                    .WithName("Player Pool")
                    .WithValue(response.Messages.First())));
        }

        [Command("settings"), Summary("Show currently configured settings for channel")]
        public async Task Settings()
        {
            var response = _pickupService.GetSettings(Context);
            await ReplyAsync("", embed: new EmbedBuilder().WithColor(new Color(0, 255, 0))
                .AddField(new EmbedFieldBuilder()
                    .WithName("Current Channel Settings:")
                    .WithValue(response.Messages.First())));
        }

        [Command("help"), Summary("Show basic commands")]
        public async Task Help()
        {
            await ReplyAsync("", embed: StaticMessages.Help());
        }
    }
}