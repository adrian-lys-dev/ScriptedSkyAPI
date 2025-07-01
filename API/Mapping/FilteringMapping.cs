using API.Dtos.FilteringDtos;
using Core.Entities;

namespace API.Mapping
{
    public static class FilteringMapping
    {
        public static FilteringDto EntityToFilteringDto(Genre genre)
        {
            return new FilteringDto
            {
                Id = genre.Id,
                Name = genre.Name
            };
        }

        public static FilteringDto EntityToFilteringDto(Author author)
        {
            return new FilteringDto
            {
                Id = author.Id,
                Name = author.Name
            };
        }

        public static FilteringDto EntityToFilteringDto(Publisher publisher)
        {
            return new FilteringDto
            {
                Id = publisher.Id,
                Name = publisher.Name
            };
        }
    }
}
