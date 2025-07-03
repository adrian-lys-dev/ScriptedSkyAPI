using Core.Entities.Base;
using System.Text.Json.Serialization;

namespace Core.Entities
{
    public class Publisher : BaseEntity
    {
        public required string Name { get; set; }
        [JsonIgnore]
        public List<Book>? Books { get; set; }
    }
}
