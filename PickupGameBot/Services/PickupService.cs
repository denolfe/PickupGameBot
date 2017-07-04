using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Discord;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using PickupGameBot.Databases;
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
        private readonly ChannelDatabase _db;

        public PickupChannel GetPickupChannel(ICommandContext context)
        {
            var channel = PickupChannels.FirstOrDefault(c => c.Config.ChannelId == context.Channel.Id);
            if (channel != null)
                return channel;
            
            // Check if previously enabled
            var channelConfig = CheckForPreviousConfig(context);
            if (channelConfig.Enabled)
            {
                var newChannel = new PickupChannel(_provider, context.Channel, channelConfig);
                PickupChannels.Add(newChannel);
                return newChannel;
            }

            return null;
        }

        public ChannelConfig CheckForPreviousConfig(ICommandContext context)
        {
            var channelConfig = _db.GetChannelConfig(context.Channel.Id);
            return channelConfig ?? ChannelConfig.GetDefault(context.Channel.Id);
        }

        public PickupService(IServiceProvider provider)
        {
            _provider = provider;
            _db = _provider.GetService<ChannelDatabase>();
        }
        
        public PickupResponse EnablePickups(ICommandContext context)
        {
            var channel = GetPickupChannel(context);
            if (channel != null) 
                return PickupResponse.PickupsWereAlreadyEnabled;

            var config = _db.GetChannelConfig(context.Channel.Id);

            if (config == null)
            {
                PickupChannels.Add(new PickupChannel(_provider, context.Channel));
            }
            else
            {
                config.Enabled = true;
                var newChannel = new PickupChannel(_provider, context.Channel, config);
                PickupChannels.Add(newChannel);
                
                _db.ChannelConfigs.Update(config);
                _db.SaveChanges();
            }
            
            return PickupResponse.PickupsEnabled(context.Channel.Name);
        }
        
        public PickupResponse DisablePickups(ICommandContext context)
        {
            var channel = GetPickupChannel(context);
            if (channel == null) 
                return PickupResponse.PickupsWereNotEnabled;
            
            PickupChannels.RemoveAll(c => c.Config.ChannelId == context.Channel.Id);
            channel.Config.Enabled = false;
            _db.ChannelConfigs.Update(channel.Config);
            _db.SaveChanges();
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
        
        public PickupResponse SetPickMode(ICommandContext context, int value)
        {
            try {return GetPickupChannel(context).SetPickMode(value);}
            catch(Exception e ) { return PickupResponse.NoPickupsForChannel; }
        }
        
        public PickupResponse SetCaptainMode(ICommandContext context, string value)
        {
            try {return GetPickupChannel(context).SetCaptainMode(value);}
            catch(Exception e ) { return PickupResponse.NoPickupsForChannel; }
        }
        
        public PickupResponse AddAdminRole(ICommandContext context, IRole role)
        {
            try {return GetPickupChannel(context).AddAdminGroup(role);}
            catch(Exception e ) { return PickupResponse.NoPickupsForChannel; }
        }

        public PickupResponse Repick(ICommandContext context)
        {
            var channel = GetPickupChannel(context);
            return channel == null 
                ? PickupResponse.PickupsWereNotEnabled 
                : PickupResponse.PickingRestarted;
        }

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