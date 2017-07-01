using System.Collections.Generic;

namespace PickupGameBot.Entities.PickModes
{
    public class RandomPickMode : IPickMode
    {
        public int Id { get; set; } = 3;
        public string Name { get; set; } = "Random";
        public Dictionary<int, int> PickMap { get; set; } = new Dictionary<int, int>();
        public override string ToString() => Name;
    }
}