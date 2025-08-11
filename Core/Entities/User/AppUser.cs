using Core.Entities.OrderAggregate;
using Microsoft.AspNetCore.Identity;

namespace Core.Entities.User
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public List<Order> Order { get; set; } = [];
        public List<Review> Reviews { get; set; } = [];
        public DateTime CreatedAt { get; set; }
        public Avatar Avatar { get; set; } = null!;
        public int AvatarId { get; set; }
    }
}