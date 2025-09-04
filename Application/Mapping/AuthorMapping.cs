using Application.Dtos.AdminDtos.AuthorDtos;
using Domain.Entities;

namespace Application.Mapping
{
    public static class AuthorMapping
    {
        public static AuthorDto ToDto(this Author author)
        {
            return new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                CreatedAt = author.CreatedAt,
                UpdatedAt = author.UpdatedAt
            };
        }

        public static List<AuthorDto> ToDtoList(this IEnumerable<Author> authors)
        {
            return authors.Select(g => g.ToDto()).ToList();
        }

        public static Author ToEntity(this CreateAuthorDto dto)
        {
            return new Author
            {
                Name = dto.Name
            };
        }
    }
}
