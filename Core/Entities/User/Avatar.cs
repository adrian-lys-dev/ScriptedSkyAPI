using Domain.Entities.Base;

namespace Domain.Entities.User
{
    public class Avatar : BaseEntity
    {
        public required string AvatarPath { get; set; }
    }
}
