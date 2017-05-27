using System.Collections.Generic;
using DiscordPugBotcore.Enums;

namespace DiscordPugBotcore.Entities
{
    public class PickupStatus
    {
        public PickupState State { get; set; }
        public int MinimumPlayers { get; set; }
        public List<PugPlayer> Captains { get; set; }
        public List<PugPlayer> PlayerPool { get; set; }
        public List<PugPlayer> Team1 { get; set; }
        public List<PugPlayer> Team2 { get; set; }
        public PickupResponse PickupResponse { get; set; }

        public PickupStatus(PickupState state, int minimumPlayers, List<PugPlayer> captains, List<PugPlayer> playerPool,
            PickupResponse puResponse)
        {
            this.State = state;
            this.MinimumPlayers = minimumPlayers;
            this.Captains = captains;
            this.PlayerPool = playerPool;
            this.PickupResponse = puResponse;
        }
    }
}