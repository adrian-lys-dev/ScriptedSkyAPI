using Domain.Entities.Base;
using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Publisher : BaseEntity
    {
        public required string Name { get; set; }
        [JsonIgnore]
        public List<Book> Book { get; set; } = [];
    }
}
