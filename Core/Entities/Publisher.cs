using Core.Entities.Base;

namespace Core.Entities
{
    public class Publisher : BaseEntity
    {
        public required string Name { get; set; }
        public List<Book>? Books { get; set; }
    }
}
