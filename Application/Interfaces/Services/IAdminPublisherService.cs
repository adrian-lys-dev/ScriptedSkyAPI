using Application.Common;
using Application.Common.Result;
using Application.Dtos.PublisherDtos;
using Application.Specificatios.Params;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IAdminPublisherService
    {
        Task<Result<Pagination<PublisherDto>>> GetAllPublishersAsync(PaginationParams paginationParams);
        Task<Result<PublisherDto>> GetPublisherByIdAsync(int publisherId);
        Task<Result<Publisher>> CreatePublisherAsync(CreatePublisherDto dto);
        Task<Result<Publisher>> UpdatePublisherAsync(CreatePublisherDto dto, int id);
        Task<Result<bool>> DeletePublisherAsync(int id);
    }
}
