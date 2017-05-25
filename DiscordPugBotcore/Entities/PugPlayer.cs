using System.Text;
using Discord;

namespace DiscordPugBotcore.Entities
{
    public class PugPlayer
    {
        public IUser User { get; set; }
        public bool WantsCaptain { get; set; }
        public bool IsCaptain { get; set; }

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
            var wantsCaptainFlag = WantsCaptain ? "*" : string.Empty;
            return $"{User.Username}{wantsCaptainFlag}";
        }
    }
}