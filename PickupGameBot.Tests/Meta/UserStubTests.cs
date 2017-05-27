using System;
using PickupGameBot.Tests.Stubs;
using Xunit;

namespace PickupGameBot.Tests.Meta
{
    public class UserStubTests
    {
        private Random _rand;
        
        public UserStubTests()
        {
            _rand = new Random();
        }
        
        [Fact]
        public void RandIdTest()
        {
            var user1 = UserStub.Generate(_rand);
            var user2 = UserStub.Generate(_rand);
            Assert.NotEqual(user1.Id, user2.Id);
        }
    }
}