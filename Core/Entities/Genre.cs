using Core.Entities.Base;

namespace Core.Entities
{
    public class Genre : BaseEntity
    {
        public required string Name { get; set; }
        public List<Book>? Book { get; set; }
    }
}
