using Core.Entities;
using Core.Specificatios.Base;

namespace Core.Specificatios
{
    public class TopRatedBooksSpecification : BaseSpecification<Book>
    {
        public TopRatedBooksSpecification()
        {
            AddOrderByDescending(b => b.Rating);
            ApplyLimit(6);
        }
    }
}
