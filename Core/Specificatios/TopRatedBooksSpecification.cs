using Core.Entities;
using Core.Specificatios.Base;

namespace Core.Specificatios
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
