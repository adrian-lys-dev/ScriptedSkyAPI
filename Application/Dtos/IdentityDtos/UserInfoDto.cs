namespace Application.Dtos.IdentityDtos
{
    public class UserInfoDto
    {
        public string Id { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Avatar { get; set; }
        public List<string> Roles { get; set; } = new();
    }

}
