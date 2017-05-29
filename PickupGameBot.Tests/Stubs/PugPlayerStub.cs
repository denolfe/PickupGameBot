using System;
using System.Collections.Generic;
using PickupGameBot.Entities;

namespace PickupGameBot.Tests.Stubs
{
    public static class PugPlayerStub 
    {
        public static PugPlayer EligibleCaptain(Random rand) => new PugPlayer(UserStub.Generate(rand), true);
        public static PugPlayer EligibleCaptain() => new PugPlayer(UserStub.Generate(), true);
        public static PugPlayer NormalPlayer(Random rand) => new PugPlayer(UserStub.Generate(rand), false);
        public static PugPlayer NormalPlayer() => new PugPlayer(UserStub.Generate(), false);

        public static List<PugPlayer> GeneratePlayers(int captains, int normal)
        {
            return PugPlayerStub.GeneratePlayers(captains, normal, new Random());
        }
        
        public static List<PugPlayer> GeneratePlayers(int captains, int normal, Random rand) 
        {
            var playerList = new List<PugPlayer>();
            for (int i = 0; i < captains; i++)
                playerList.Add(PugPlayerStub.EligibleCaptain(rand));

            for (int i = 0; i < normal; i++)
                playerList.Add(PugPlayerStub.NormalPlayer(rand));

            return playerList;          
        }
    }
}