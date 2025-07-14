using Core.Entities.Base;
using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class Genre : BaseEntity
    {
        public required string Name { get; set; }
        [JsonIgnore]
        public List<Book> Book { get; set; } = [];
    }
}
