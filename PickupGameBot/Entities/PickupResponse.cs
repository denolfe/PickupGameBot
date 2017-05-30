using System.Collections.Generic;
using Discord;
using PickupGameBot.Utility;

namespace PickupGameBot.Entities
{
    public class PickupResponse
    {
        public bool Success { get; set; }
        public List<string> Messages { get; set; }

        public PickupResponse(bool success, string message)
        {
            Success = success;
            Messages = new List<string> { message };
            PrettyConsole.Log(LogSeverity.Info, "Response", message);
        }
        
        public PickupResponse(bool success, List<string> messageses)
        {
            Success = success;
            Messages = messageses;
            foreach (var message in messageses)
            {
                PrettyConsole.Log(LogSeverity.Info, "Response", message);
            }
        }
        
        public static PickupResponse Bad(string message) => new PickupResponse(false, message);
        public static PickupResponse Bad(List<string> messages) => new PickupResponse(false, messages);
        public static PickupResponse Good(string message) => new PickupResponse(true, message);
        public static PickupResponse Good(List<string> messages) => new PickupResponse(true, messages);
    }
}