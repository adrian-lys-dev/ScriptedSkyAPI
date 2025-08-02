using API.Dtos.UserProfileDtos;
using API.Helpers;
using Core.Entities.User;

namespace API.Mapping
{
    public static class AvatarMapping
    {
        public static AvatarDto ToDto(Avatar avatar)
        {
            return new AvatarDto
            {
                Id = avatar.Id,
                AvatarPath = UrlHelper.BuildImageUrl(avatar.AvatarPath),
            };
        }

    }
}
