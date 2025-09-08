using Application.Specificatios.Base;
using Application.Specificatios.Params;
using Domain.Entities;

namespace Application.Specificatios
{
    public class BookSpecification : BaseSpecification<Book>
    {
        public BookSpecification(PaginationParams paginationParams) 
        {
            AddInclude(p => p.Author);
            AddInclude(p => p.Genre);
            AddInclude(p => p.Publisher);

            ApplyPaging(paginationParams.PageSize * (paginationParams.PageIndex - 1), paginationParams.PageSize);
            AddOrderByDescending(p => p.CreatedAt);
            EnableSplitQuery();
        }

        public BookSpecification(int id) : base(x => x.Id == id) 
        {
            AddInclude(p => p.Author);
            AddInclude(p => p.Genre);
            AddInclude(p => p.Publisher);
        }

        public BookSpecification(string title) : base(x => x.Title == title){}
    }
}
