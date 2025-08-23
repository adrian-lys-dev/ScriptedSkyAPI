using Application.Common.Result;
using Application.Dtos.FilteringDtos;

namespace Application.Interfaces.Services
{
    public interface IFilteringService
    {
        Task<Result<IReadOnlyList<FilteringDto>>> GetGenresAsync();
        Task<Result<IReadOnlyList<FilteringDto>>> GetAuthorsAsync();
        Task<Result<IReadOnlyList<FilteringDto>>> GetPublishersAsync();
    }
}
