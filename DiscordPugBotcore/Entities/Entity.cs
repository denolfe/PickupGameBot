using System.ComponentModel.DataAnnotations;

namespace DiscordPugBotcore.Entities
{
    public abstract class Entity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}