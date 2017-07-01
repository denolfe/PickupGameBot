using System.Collections.Generic;

namespace PickupGameBot.Entities.PickModes
{
    public interface IPickMode
    {
        int Id { get; set; }
        string Name { get; set; }
        Dictionary<int, int> PickMap { get; set; }
    }
}