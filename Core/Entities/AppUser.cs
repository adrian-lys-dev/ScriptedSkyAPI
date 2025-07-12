using Core.Entities.OrderAggregate;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public List<Order> Order { get; set; } = [];
        public List<Review> Reviews { get; set; } = [];
    }
}