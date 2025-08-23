using Application.Common.Result;
using Application.Dtos.FilteringDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mapping;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class FilteringService(IUnitOfWork unit, ILogger<FilteringService> logger) : IFilteringService
    {
        public async Task<Result<IReadOnlyList<FilteringDto>>> GetGenresAsync()
        {
            var genres = await unit.Repository<Genre>().ListAllAsync();
            var dtos = genres.Select(FilteringMapping.EntityToFilteringDto).ToList();

            logger.LogInformation("Returned {Count} genres", dtos.Count);
            return Result<IReadOnlyList<FilteringDto>>.SuccessResult(dtos);
        }

        public async Task<Result<IReadOnlyList<FilteringDto>>> GetAuthorsAsync()
        {
            var authors = await unit.Repository<Author>().ListAllAsync();
            var dtos = authors.Select(FilteringMapping.EntityToFilteringDto).ToList();

            logger.LogInformation("Returned {Count} authors", dtos.Count);
            return Result<IReadOnlyList<FilteringDto>>.SuccessResult(dtos);
        }

        public async Task<Result<IReadOnlyList<FilteringDto>>> GetPublishersAsync()
        {
            var publishers = await unit.Repository<Publisher>().ListAllAsync();
            var dtos = publishers.Select(FilteringMapping.EntityToFilteringDto).ToList();

            logger.LogInformation("Returned {Count} publishers", dtos.Count);
            return Result<IReadOnlyList<FilteringDto>>.SuccessResult(dtos);
        }
    }
}
