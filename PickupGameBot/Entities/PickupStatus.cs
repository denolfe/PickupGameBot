using System.Collections.Generic;
using PickupGameBot.Enums;

namespace PickupGameBot.Entities
{
    public class PickupStatus
    {
        public PickupState State { get; set; }
        public int MinimumPlayers { get; set; }
        public bool BothTeamsAreFull { get; set; }
        public List<PugPlayer> Captains { get; set; }
        public List<PugPlayer> PlayerPool { get; set; }
        public Team Team1 { get; set; }
        public Team Team2 { get; set; }
        public PickupResponse PickupResponse { get; set; }

        public PickupStatus(PickupState state, int minimumPlayers, 
            List<PugPlayer> captains,
            List<PugPlayer> playerPool,
            bool bothTeamsFull,
            PickupResponse puResponse)
        {
            this.State = state;
            this.MinimumPlayers = minimumPlayers;
            this.Captains = captains;
            this.PlayerPool = playerPool;
            this.BothTeamsAreFull = bothTeamsFull;
            this.PickupResponse = puResponse;
        }

        public PickupStatus(
            PickupState state,
            int minimumPlayers,
            List<PugPlayer> captains,
            List<PugPlayer> playerPool,
            Team team1,
            Team team2,
            bool bothTeamsFull,
            PickupResponse puResponse)
        {

            this.State = state;
            this.MinimumPlayers = minimumPlayers;
            this.Captains = captains;
            this.PlayerPool = playerPool;
            this.Team1 = team1;
            this.Team2 = team2;
            this.BothTeamsAreFull = bothTeamsFull;
            this.PickupResponse = puResponse;
        }
    }
}