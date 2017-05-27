using System.ComponentModel.DataAnnotations;

namespace PickupGameBot.Entities
{
    public abstract class Entity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}