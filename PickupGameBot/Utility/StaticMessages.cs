using Discord;

namespace PickupGameBot.Utility
{
    public static class StaticMessages
    {
        public static EmbedBuilder Welcome() 
            => new EmbedBuilder()
                .WithColor(new Color(0, 255, 0))
    
                .AddField(new EmbedFieldBuilder()
                    .WithName("Admin Setup Commands")
                    .WithValue("!teamsize, !pickmode, !captainmode"))
                
                .AddField(new EmbedFieldBuilder()
                    .WithName("Player Commands")
                    .WithValue("!join, !leave, !status"));
    }
}