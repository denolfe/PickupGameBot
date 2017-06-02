using System.Text;
using Discord;

namespace PickupGameBot.Entities
{
    public class PugPlayer
    {
        public IUser User { get; set; }
        public bool WantsCaptain { get; set; } = false;
        public bool IsCaptain { get; set; } = false;
        public int TeamId { get; set; } = 0;

        public PugPlayer(IUser user, bool wantsCaptain)
        {
            this.User = user;
            this.WantsCaptain = wantsCaptain;
            this.IsCaptain = false;
        }
        
        public void SetCaptain() => 
            this.IsCaptain = true;

        public override string ToString()
        {
            var captainFlag = WantsCaptain || IsCaptain ? "*" : string.Empty;
            return $"{User.Username}{captainFlag}";
        }
    }
}