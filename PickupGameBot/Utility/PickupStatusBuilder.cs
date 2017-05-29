using System.Collections.Generic;
using System.Linq.Expressions;
using Discord;
using PickupGameBot.Entities;
using PickupGameBot.Enums;

namespace PickupGameBot.Utility
{
    public class PickupStatusBuilder
    {
        private EmbedBuilder _builder;
        
        public PickupStatusBuilder(PickupStatus status)
        {
            _builder = new EmbedBuilder()
                .WithColor(new Color(0,255,0))
                .WithDescription($"_> {status.PickupResponse.Message}_");

            if (status.State == PickupState.Picking)
            {
                _builder
                    .WithTitle($"Pickup Status: {status.State}");
                
                if (status.Team1.Players.Count > 0)
                {
                    _builder
                        .AddField(new EmbedFieldBuilder()
                            .WithName("Team 1")
                            .WithValue($"[{status.Team1.Players.Count}] {string.Join(",", status.Team1)}"
                            ));    
                }
                if (status.Team2.Players.Count > 0)
                {
                    _builder
                        .AddField(new EmbedFieldBuilder()
                            .WithName("Team 2")
                            .WithValue($"[{status.Team2.Players.Count}] {string.Join(",", status.Team2)}"
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