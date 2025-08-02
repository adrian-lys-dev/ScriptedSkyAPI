using Core.Entities.Base;

namespace Core.Entities.User
{
    public class Avatar : BaseEntity
    {
        public required string AvatarPath { get; set; }
    }
}
