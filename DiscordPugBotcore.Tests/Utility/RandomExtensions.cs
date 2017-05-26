using System;

namespace DiscordPugBotcore.Tests.Utility
{
    public static class RandomExtensions
    {
        public static UInt64 NextUInt64(this Random rnd)
        {
            var buffer = new byte[sizeof(UInt64)];
            rnd.NextBytes(buffer);
            return BitConverter.ToUInt64(buffer, 0);
        }
    }
}