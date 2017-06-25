using Discord;
using PickupGameBot.Entities;
using PickupGameBot.Enums;
using PickupGameBot.Extensions;

namespace PickupGameBot.Validators
{
    public class RemovePlayerValidator
    {
        public PickupResponse Validate(PickupChannel channel, IUser user)
        {
            if (!channel.PlayerPool.ContainsPlayer(user))
                return PickupResponse.NotInPlayerList(user.Username);
                
            if (channel.PickupState != PickupState.Gathering)
                return PickupResponse.Bad($"State: {channel.PickupState}. Players cannot leave at this time");

            return null;
        }

    }
}