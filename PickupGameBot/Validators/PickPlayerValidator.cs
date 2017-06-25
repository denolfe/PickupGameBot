using System.Collections.Generic;
using Discord;
using PickupGameBot.Entities;
using PickupGameBot.Enums;
using PickupGameBot.Extensions;

namespace PickupGameBot.Validators
{
    public class PickPlayerValidator
    {
        public PickupResponse Validate(PickupChannel channel, IUser captain, IUser user)
        {
            if (channel.CurrentGame.BothTeamsAreFull())
                return PickupResponse.TeamsFull;

            if (user == null)
                return PickupResponse.NoPlayersLeftInPool;

            if (channel.PickupState != PickupState.Picking)
                return PickupResponse.NotInPickingState;

            if (!channel.Captains.ContainsPlayer(captain))
                return PickupResponse.NotCaptain(captain.Username);

            if (channel.Captains.GetPlayer(captain).TeamId != channel.PickingCaptain.TeamId)
                return PickupResponse.NotPick(user.Username);

            if (!channel.PlayerPool.ContainsPlayer(user))
                return PickupResponse.NotInPlayerList(user.Username);

            return null;
        }
    }
}