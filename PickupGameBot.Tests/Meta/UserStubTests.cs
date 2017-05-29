using System;
using PickupGameBot.Tests.Stubs;
using Xunit;

namespace PickupGameBot.Tests.Meta
{
    public class UserStubTests
    {
        [Fact]
        public void RandIdTest()
        {
            var user1 = UserStub.Generate();
            var user2 = UserStub.Generate();
            Assert.NotEqual(user1.Id, user2.Id);
        }

        [Fact]
        public void RandIntTestNoParam()
        {
            var user1 = UserStub.Generate();
            var user2 = UserStub.Generate();
            Assert.NotEqual(user1.Id, user2.Id);
        }
    }
}