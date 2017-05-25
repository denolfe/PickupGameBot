namespace DiscordPugBotcore.Entities
{
    public class PickupResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }

        public PickupResponse(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        
        public static PickupResponse Bad(string message) => new PickupResponse(false, message);
        public static PickupResponse Good(string message) => new PickupResponse(true, message);
    }
}