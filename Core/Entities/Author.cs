using Core.Entities.Base;

namespace Core.Entities
{
    public class Author : BaseEntity
    {
        public required string Name { get; set; }
        public List<Book>? Book { get; set; }
    }
}
