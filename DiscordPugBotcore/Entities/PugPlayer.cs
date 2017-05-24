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
        }

        public void SetCaptain() => 
            this.IsCaptain = this.WantsCaptain ? true : false;
    }
}