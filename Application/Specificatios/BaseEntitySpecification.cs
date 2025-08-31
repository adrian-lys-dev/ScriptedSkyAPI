using Application.Specificatios.Base;
using Application.Specificatios.Params;
using Domain.Entities.Base;

namespace Application.Specificatios
{
    public class BaseEntitySpecification<T> : BaseSpecification<T> where T : BaseEntity
    {
        public BaseEntitySpecification(PaginationParams paginationParams) 
        {
            AddOrderBy(x => x.Id);
            ApplyPaging(paginationParams.PageSize * (paginationParams.PageIndex - 1),
                paginationParams.PageSize);
        }

        public BaseEntitySpecification(int id): base(x => x.Id == id) {}
    }
}
