using System.Collections.Generic;
using System.Linq.Expressions;
using Discord;
using DiscordPugBotcore.Entities;
using DiscordPugBotcore.Enums;

namespace DiscordPugBotcore.Utility
{
    public class PickupStatusBuilder
    {
        private EmbedBuilder _builder;
        
        public PickupStatusBuilder(PickupStatus status)
        {
            _builder = new EmbedBuilder()
                .WithColor(new Color(44,47,51))
                .WithDescription($"_> {status.PickupResponse.Message}_");

            if (status.State == PickupState.Picking)
            {
                _builder
                    .WithTitle($"Pickup Status: {status.State}");
                
                if (status.Team1.Count > 0)
                {
                    _builder
                        .AddField(new EmbedFieldBuilder()
                            .WithName("Team 1")
                            .WithValue($"[{status.Team1.Count}] {string.Join(",", status.Team1)}"
                            ));    
                }
                if (status.Team2.Count > 0)
                {
                    _builder
                        .AddField(new EmbedFieldBuilder()
                            .WithName("Team 2")
                            .WithValue($"[{status.Team2.Count}] {string.Join(",", status.Team2)}"
                            ));    
                }
                _builder.WithFooter(new EmbedFooterBuilder()
                    .WithText("Captain Commands: !pick"));
            } 
            else if (status.State == PickupState.Gathering)
            {
                _builder
                    .WithTitle($"Pickup Status: {status.State}")
                    .WithFooter(new EmbedFooterBuilder()
                        .WithText("Commands: !join, !leave, !status"));
            }
            
            _builder
                .AddField(new EmbedFieldBuilder()
                    .WithName($"Player Pool [{status.PlayerPool.Count}/{status.MinimumPlayers}]")
                    .WithValue(string.Join(",", status.PlayerPool)
                    ));
        }

        public EmbedBuilder Build()
        {
            return _builder;
        }
    }
}