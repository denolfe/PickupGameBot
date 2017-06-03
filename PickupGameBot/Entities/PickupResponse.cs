using System.Collections.Generic;
using System.Linq;
using Discord;
using PickupGameBot.Enums;
using PickupGameBot.Utility;

namespace PickupGameBot.Entities
{
    public class PickupResponse
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }

        public string JoinedMessages =>
            string.Join("\n", Messages);

        public PickupResponse(bool success, string message)
        {
            Success = success;
            Messages = new List<string> { message };
            PrettyConsole.Log(LogSeverity.Info, "Response", message);
        }
        
        public PickupResponse(bool success, List<string> messages)
        {
            Success = success;
            Messages = messages;
            PrettyConsole.Log(LogSeverity.Info, "Response", messages.First());
        }
        
        public static PickupResponse Bad(string message) => new PickupResponse(false, message);
        public static PickupResponse Bad(List<string> messages) => new PickupResponse(false, messages);
        public static PickupResponse Good(string message) => new PickupResponse(true, message);
        public static PickupResponse Good(List<string> messages) => new PickupResponse(true, messages);
        
        // Good
        public static PickupResponse RemovedFromList(string username) => new PickupResponse(true,
            $"{username} removed from list.");
        
        public static PickupResponse PickingCompleted(string teamFormatted) 
            => new PickupResponse(true, $"Picking Completed!\n{teamFormatted}");
        
        public static PickupResponse NoPlayersInPool => new PickupResponse(true, 
            "_No players in player pool. Type **!join** to be added to the player pool._");
        public static PickupResponse PickingToStart(string joinString) 
            => new PickupResponse(true, $"{joinString}\nPicking is about to start!");
        public static PickupResponse PickingRestarted => new PickupResponse(true, "Picking has restarted.");
        public static PickupResponse PickupReset => new PickupResponse(true, "Pickup game has been reset.");
        public static PickupResponse PickupsEnabled(string channelName) 
            => new PickupResponse(true, $"Pickups enabled for #{channelName}");
        public static PickupResponse PickupsDisabled(string channelName) 
            => new PickupResponse(true, $"Pickups were disabled for #{channelName}");
        
        // Bad
        public static PickupResponse NotInPlayerList(string username) => new PickupResponse(false,
            $"{username} is not in the player list.");
        
        public static PickupResponse TeamsFull => new PickupResponse(false,
            "Teams are full, you cannot pick any more players.");
        public static PickupResponse NoPlayersLeftInPool => new PickupResponse(false,
            "No players left in the pool.");
        public static PickupResponse NotInPickingState => new PickupResponse(false,
            $"Cannot pick when not in Picking state.");
        public static PickupResponse NotInPickingStateRepick => new PickupResponse(false,
            "Cannot repick when not in Picking state.");
        public static PickupResponse NotCaptain(string username) => new PickupResponse(false,
            $"{username} is not a captain!");
        public static PickupResponse NotPick(string username) => new PickupResponse(false,
            $"It is not {username}'s pick!");
        public static PickupResponse AlreadyJoined(string username) => new PickupResponse(false,
            $"{username} has already joined.");
        public static PickupResponse NoPickupsForChannel => new PickupResponse(false, 
            "No pickups found for this channel. An admin must type **!enable** to allow pickups in this channel.");
        public static PickupResponse PickupsWereNotEnabled => new PickupResponse(false,
            "Pickups were not enabled for this channel.");
        public static PickupResponse PickupsWereAlreadyEnabled => new PickupResponse(false,
            "Pickups were already enabled for this channel.");
    }
}