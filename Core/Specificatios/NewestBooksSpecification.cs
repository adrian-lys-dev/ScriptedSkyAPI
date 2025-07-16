using Core.Entities;
using Core.Specificatios.Base;

namespace Core.Specificatios
{
    public class NewestBooksSpecification : BaseSpecification<Book>
    {
        public NewestBooksSpecification()
        {
            AddInclude(x => x.Author);
            AddOrderByDescending(b => b.CreatedAt);
            ApplyLimit(6);
        }
    }
}
