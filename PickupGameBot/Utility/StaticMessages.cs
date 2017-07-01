using System;
using Discord;

namespace PickupGameBot.Utility
{
    public static class StaticMessages
    {
        public static EmbedBuilder Welcome()
            => new EmbedBuilder()
                .WithColor(new Color(0, 255, 0))
                .WithDescription("**Admin Commands**")
                .AddField("!teamsize <int>",
                    "Set size of teams (default: 5)")
                .AddField("!addadminrole",
                    "Add role as admin for pickup games")
                .AddField("!pickmode <1|2|3>",
                    $"1 - Every Other (default){Environment.NewLine}" +
                    $"2 - Second Captain Picks Twice{Environment.NewLine}" +
                    $"3 - Completely Random Teams")


                .AddField("!join", "Join the player pool")
                .AddField("!leave", "Leave the player pool")
                .AddField("!status", "Show current status");

        public static EmbedBuilder Help()
            => new EmbedBuilder()
                .WithColor(new Color(0, 255, 0))
                .WithDescription("**Player Commands**")
                .AddField("!join", "Join the player pool")
                .AddField("!leave", "Leave the player pool")
                .AddField("!status", "Show current status");

    }
}