using Application.Specificatios.Base;
using Domain.Entities;

namespace Application.Specificatios
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
