using Application.Specificatios.Base;
using Application.Specificatios.Params;
using Domain.Entities.OrderAggregate;

namespace Application.Specificatios
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(string id, PaginationParams paginationParams) : base(x => x.User.Id == id)
        {
            AddInclude("OrderItem.Book");
            AddInclude(x => x.DeliveryMethod);
            AddOrderByDescending(x => x.CreatedAt);
            ApplyPaging(paginationParams.PageSize * (paginationParams.PageIndex - 1),
                paginationParams.PageSize);
        }

        public OrderSpecification(PaginationParams paginationParams)
        {
            AddInclude("OrderItem.Book");
            AddInclude(x => x.DeliveryMethod);
            AddOrderByDescending(x => x.CreatedAt);
            ApplyPaging(paginationParams.PageSize * (paginationParams.PageIndex - 1),
                paginationParams.PageSize);
        }

        public OrderSpecification(int orderId) : base(x => x.Id == orderId)
        {
            AddInclude("OrderItem.Book");
            AddInclude(x => x.DeliveryMethod);
            AddInclude(x => x.User);
        }
    }
}
