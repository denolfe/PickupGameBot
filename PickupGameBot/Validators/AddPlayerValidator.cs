using Discord;
using PickupGameBot.Entities;
using PickupGameBot.Enums;
using PickupGameBot.Extensions;

namespace PickupGameBot.Validators
{
    public class AddPlayerValidator
    {
        public PickupResponse Validate(PickupChannel channel, PugPlayer pugPlayer)
        {
            if (channel.PickupState != PickupState.Gathering)
                return PickupResponse.Bad($"State: {channel.PickupState}. New players cannot join at this time");

            if (channel.PlayerPool.ContainsPlayer(pugPlayer))
                return PickupResponse.AlreadyJoined(pugPlayer.User.Username);

            return null;
        }
    }
}