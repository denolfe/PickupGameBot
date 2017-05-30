using System.Collections.Generic;
using System.Linq.Expressions;
using Discord;
using PickupGameBot.Entities;
using PickupGameBot.Enums;
using PickupGameBot.Extensions;

namespace PickupGameBot.Utility
{
    public class PickupStatusBuilder
    {
        private EmbedBuilder _builder;
        public EmbedBuilder Build() => _builder;
        
        public PickupStatusBuilder(PickupStatus status)
        {
            _builder = new EmbedBuilder()
                .WithColor(new Color(0, 255, 0));

            if (status.State == PickupState.Picking)
            {
                _builder
                    .WithTitle($"Pickup Status: {status.State}");
                
                if (status.Team1.Players.Count > 0)
                {
                    _builder
                        .AddField(new EmbedFieldBuilder()
                            .WithName("Team 1")
                            .WithValue($"[{status.Team1.Players.Count}] {status.Team1.Players.ToFormattedList()}"
                            ));    
                }
                if (status.Team2.Players.Count > 0)
                {
                    _builder
                        .AddField(new EmbedFieldBuilder()
                            .WithName("Team 2")
                            .WithValue($"[{status.Team2.Players.Count}] {status.Team2.Players.ToFormattedList()}"
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

            _builder.WithDescription(status.PlayerPool.Count == 0
                ? $"Playes [0/{status.MinimumPlayers}]: _No players in the pool_"
                : $"Players [{status.PlayerPool.Count}/{status.MinimumPlayers}]: {status.PlayerPool.ToFormattedList()}");
        }
    }
}