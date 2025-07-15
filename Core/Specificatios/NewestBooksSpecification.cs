using Core.Entities;
using Core.Specificatios.Base;

namespace Core.Specificatios
{
    public class NewestBooksSpecification : BaseSpecification<Book>
    {
        public NewestBooksSpecification()
        {
            AddOrderByDescending(b => b.CreatedAt);
            ApplyLimit(6);
        }
    }
}
