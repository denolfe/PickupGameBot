using System.Collections.Generic;

namespace PickupGameBot.Entities.PickModes
{
    public class SecondPicksTwicePickMode : IPickMode
    {
        public int Id { get; set; } = 2;
        public string Name { get; set; } = "Second Picks Twice";

        public Dictionary<int, int> PickMap { get; set; } = new Dictionary<int, int>
        {
            {1, 1}, // Should never happen, set initially to 1
            {2, 2},
            {3, 2},
            {4, 1},
            {5, 2},
            {6, 1},
            {7, 2},
            {8, 1},
            {9, 2},
            {10, 1},
            {11, 1},
            {12, 1}
        };

        public override string ToString() => Name;
    }
}