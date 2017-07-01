using System.Collections.Generic;

namespace PickupGameBot.Entities.PickModes
{
    public class EveryOtherPickMode : IPickMode
    {
        public int Id { get; set; } = 1;
        public string Name { get; set; } = "EveryOther";
        
        public Dictionary<int, int> PickMap { get; set; } = new Dictionary<int, int>
        {
            {1, 1}, // Should never happen, set initially to 1
            {2, 2},
            {3, 1},
            {4, 2},
            {5, 1},
            {6, 2},
            {7, 1},
            {8, 2},
            {9, 1},
            {10, 2},
            {11, 1},
            {12, 2}
        };

        public override string ToString() => Name;
    }
}