using Core.Entities;
using Core.Specificatios.Base;
using Core.Specificatios.Params;

namespace Core.Specificatios
{
    public class ReviewSpecification : BaseSpecification<Review>
    {
        public ReviewSpecification(PaginationParams paginationParams)
        {
            ApplyPaging(paginationParams.PageSize * (paginationParams.PageIndex - 1),
                    paginationParams.PageSize);
            AddOrderByDescending(x => x.CreatedAt);
        }

        public ReviewSpecification(PaginationParams paginationParams, int bookId) :
            base(x => (x.BookId == bookId))
        {
            AddInclude("AppUser.Avatar");
            ApplyPaging(paginationParams.PageSize * (paginationParams.PageIndex - 1),
                paginationParams.PageSize);
            AddOrderByDescending(x => x.CreatedAt);
        }
    }
}
