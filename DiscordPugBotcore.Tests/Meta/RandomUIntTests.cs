using System;
using DiscordPugBotcore.Tests.Utility;
using Xunit;

namespace DiscordPugBotcore.Tests.Meta
{
    public class RandomUIntTests
    {
        private Random _rand;

        public RandomUIntTests()
        {
            _rand = new Random();
        }
        
        [Fact]
        public void TestRandomUIntGenerator()
        {
            
            var uint1 = _rand.NextUInt64();
            var uint2 = _rand.NextUInt64();
            Assert.NotEqual(uint1, uint2);
        }
    }
}