using Application.Dtos.PublisherDtos;
using Domain.Entities;

namespace Application.Mapping
{
    public static class PublisherMapping
    {
        public static PublisherDto ToDto(this Publisher publisher)
        {
            return new PublisherDto
            {
                Id = publisher.Id,
                Name = publisher.Name,
                CreatedAt = publisher.CreatedAt,
                UpdatedAt = publisher.UpdatedAt
            };
        }

        public static List<PublisherDto> ToDtoList(this IEnumerable<Publisher> publishers)
        {
            return publishers.Select(g => g.ToDto()).ToList();
        }

        public static Publisher ToEntity(this CreatePublisherDto dto)
        {
            return new Publisher
            {
                Name = dto.Name
            };
        }
    }
}
