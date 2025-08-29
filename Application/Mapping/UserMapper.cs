using Application.Dtos.IdentityDtos;
using Application.Helpers;
using Domain.Entities.User;

namespace Application.Mapping
{
    public static class UserMapper
    {
        public static UserInfoDto ToUserInfoDto(AppUser appUser, IList<string> roles)
        {
            return new UserInfoDto
            {
                Id = appUser.Id,
                FirstName = appUser.FirstName,
                LastName = appUser.LastName,
                Email = appUser.Email!,
                Avatar = UrlHelper.BuildImageUrl(appUser.Avatar.AvatarPath),
                Roles = roles.ToList()
            };
        }
    }
}
