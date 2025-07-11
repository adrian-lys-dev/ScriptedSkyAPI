using Core.Entities.OrderAggregate;
using Core.Specificatios.Base;

namespace Core.Specificatios
{
    public class OrderSpecification : BaseSpecification<Order>
    {
        public OrderSpecification(string id) : base(x => x.User.Id == id)
        {
            AddInclude("OrderItem.Book");
            AddInclude(x => x.DeliveryMethod);
            AddOrderByDescending(x => x.CreatedAt);
        }

        public OrderSpecification(int orderId) : base(x => x.Id == orderId)
        {
            AddInclude("OrderItem.Book");
            AddInclude(x => x.DeliveryMethod);
        }
    }
}
