using Application.Dtos.GenreDtos;
using Domain.Entities;

namespace Application.Mapping
{
    public static class GenreMapping
    {
        public static GenreDto ToDto(this Genre genre)
        {
            return new GenreDto
            {
                Id = genre.Id,
                Name = genre.Name,
                CreatedAt = genre.CreatedAt,
                UpdatedAt = genre.UpdatedAt
            };
        }

        public static List<GenreDto> ToDtoList(this IEnumerable<Genre> genres)
        {
            return genres.Select(g => g.ToDto()).ToList();
        }
    }
}
