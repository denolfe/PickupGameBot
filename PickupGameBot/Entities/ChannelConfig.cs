using System.ComponentModel.DataAnnotations;
using Discord;
using PickupGameBot.Entities.PickModes;

namespace PickupGameBot.Entities
{
    public class ChannelConfig
    {
        [Key]
        public ulong ChannelId { get; set; }
        public bool Enabled { get; set; }
        public int PickModeId { get; set; }
        public int TeamSize { get; set; }

        public static ChannelConfig GetDefault(ulong channelId)
        {
            return new ChannelConfig
            {
                ChannelId = channelId,
                Enabled = false,
                PickModeId = 1,
                TeamSize = 5
            };
        }
    }
}