using Application.Specificatios.Base;
using Domain.Entities;

namespace Application.Specificatios
{
    public class TopRatedBooksSpecification : BaseSpecification<Book>
    {
        public TopRatedBooksSpecification()
        {
            AddInclude(x => x.Author);
            AddOrderByDescending(b => b.Rating);
            ApplyLimit(6);
        }
    }
}
