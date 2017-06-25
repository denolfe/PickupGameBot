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
            _provider = provider;
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
        
        public PickupResponse ForceAddPlayer(ICommandContext context, IUser user)
        {
            try { return GetPickupChannel(context).AddPlayerToPool(user, false); }
            catch(Exception e ) { return PickupResponse.NoPickupsForChannel;}
        }

        public PickupResponse RemovePlayer(ICommandContext context)
        {
            try { return GetPickupChannel(context).RemovePlayerFromPool(context.User); }
            catch(Exception e ) { return PickupResponse.NoPickupsForChannel;}
        }
        
        public PickupResponse ForceRemovePlayer(ICommandContext context, IUser user)
        {
            try { return GetPickupChannel(context).RemovePlayerFromPool(user); }
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

        public PickupResponse SetTeamSize(ICommandContext context, string value)
        {
            try {return GetPickupChannel(context).SetTeamSize(value);}
            catch(Exception e ) { return PickupResponse.NoPickupsForChannel; }
        }

//        private PickupStatus BuildPickupStatus(PickupResponse puResponse)
//        {
//            return new PickupStatus(
//                PickupState,
//                _minimumPlayers,
//                Captains,
//                PlayerPool,
//                Team1,
//                Team2,
//                BothTeamsAreFull(),
//                puResponse
//            );
//        }

        public PickupResponse Repick(ICommandContext context)
        {
            var channel = GetPickupChannel(context);
            return channel == null 
                ? PickupResponse.PickupsWereNotEnabled 
                : PickupResponse.PickingRestarted;
        }

        // TODO: Mention all players that were in pool
        public PickupResponse Reset(ICommandContext context)
        {
            var channel = GetPickupChannel(context);
            if (channel == null) 
                return PickupResponse.PickupsWereNotEnabled;

            var response = channel.Reset();
            
            return PickupResponse.Good(response.Messages);
        }
    }
}