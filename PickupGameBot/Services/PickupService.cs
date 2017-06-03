using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Discord;
using Discord.Commands;
using PickupGameBot.Entities;
using PickupGameBot.Enums;
using PickupGameBot.Extensions;
using PickupGameBot.Utility;

namespace PickupGameBot.Services
{
    public class PickupService
    {
        public List<PickupChannel> PickupChannels { get; set; } = new List<PickupChannel>();

        private readonly IServiceProvider _provider;

        public PickupChannel GetPickupChannel(ICommandContext context) 
            => PickupChannels.FirstOrDefault(c => c.MessageChannel.Id == context.Channel.Id);


        public PickupService(IServiceProvider provider)
        {
            this._provider = provider;
        }
        
        public PickupResponse EnablePickups(ICommandContext context) 
        {
            var channel = GetPickupChannel(context);
            if (channel != null) 
                return PickupResponse.PickupsWereAlreadyEnabled;
            
            PickupChannels.Add(new PickupChannel(context.Channel));
            return PickupResponse.PickupsEnabled(context.Channel.Name);
        }
        
        public PickupResponse DisablePickups(ICommandContext context)
        {
            var channel = GetPickupChannel(context);
            if (channel == null) 
                return PickupResponse.PickupsWereNotEnabled;
            
            PickupChannels.RemoveAll(c => c.MessageChannel.Id == context.Channel.Id);
            return PickupResponse.PickupsDisabled(context.Channel.Name);
        }

        public PickupResponse AddPlayer(ICommandContext context, bool wantsCaptain)
        {
            try { return GetPickupChannel(context).AddPlayerToPool(context.User, wantsCaptain); }
            catch(Exception e ) { return PickupResponse.NoPickupsForChannel;}
        }

        public PickupResponse RemovePlayer(ICommandContext context)
        {
            try { return GetPickupChannel(context).RemovePlayerFromPool(context.User); }
            catch(Exception e ) { return PickupResponse.NoPickupsForChannel;}
        }
        
        public PickupResponse PickPlayer(ICommandContext context, IUser user)
        {
            try { return GetPickupChannel(context).PickPlayer(context.User, user); }
            catch(Exception e ) { return PickupResponse.NoPickupsForChannel;}
        }

        public PickupResponse Status(ICommandContext context)
        {
            try {return GetPickupChannel(context).GetStatus();}
            catch(Exception e ) { return PickupResponse.NoPickupsForChannel; }
        }

//        private PickupStatus BuildPickupStatus(PickupResponse puResponse)
//        {
//            return new PickupStatus(
//                this.PickupState,
//                this._minimumPlayers,
//                this.Captains,
//                this.PlayerPool,
//                this.Team1,
//                this.Team2,
//                this.BothTeamsAreFull(),
//                puResponse
//            );
//        }

//        public PickupStatus Repick()
//        {
//            if (this.PickupState != PickupState.Picking)
//                return BuildPickupStatus(PickupResponse.NotInPickingStateRepick);
//
//
//            if (this.Team1?.Players.Count > 0)
//                this.PlayerPool.AddRange(this.Team1.PopAll());
//            if (this.Team2?.Players.Count > 0)
//                this.PlayerPool.AddRange(this.Team2.PopAll());
//
//            // Remove captains from normal player pool
//            foreach (var pugPlayer in this.Captains)
//            {
//                if (this.PlayerPool.ContainsPlayer(pugPlayer))
//                    this.PlayerPool = this.PlayerPool.RemovePlayer(pugPlayer);
//            }
//
//            this.PickingCaptain = this.Team1?.Captain;
//
//            return BuildPickupStatus(PickupResponse.PickingRestarted);
//        }
//
        // TODO: Mention all players that were in pool
        public PickupResponse Reset(ICommandContext context)
        {
            var channel = GetPickupChannel(context);
            if (channel == null) 
                return PickupResponse.PickupsWereNotEnabled;

            var response = channel.Reset();
            
            return PickupResponse.Good(response.Messages.First());
        }
    }
}