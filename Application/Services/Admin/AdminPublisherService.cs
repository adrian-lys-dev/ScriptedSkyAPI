using Application.Common;
using Application.Common.Result;
using Application.Dtos.AdminDtos.PublisherDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mapping;
using Application.Specificatios;
using Application.Specificatios.Params;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services.Admin
{
    public class AdminPublisherService(IUnitOfWork unit, ILogger<AdminPublisherService> logger) : IAdminPublisherService
    {
        public async Task<Result<Pagination<PublisherDto>>> GetAllPublishersAsync(PaginationParams paginationParams)
        {
            var spec = new BaseEntitySpecification<Publisher>(paginationParams);
            var publishers = await unit.Repository<Publisher>().ListWithSpecAsync(spec);
            var count = await unit.Repository<Publisher>().CountSpecAsync(spec);

            var publishersDto = publishers.Select(PublisherMapping.ToDto).ToList();
            var pagination = new Pagination<PublisherDto>(paginationParams.PageIndex, paginationParams.PageSize, count, publishersDto);

            return Result<Pagination<PublisherDto>>.SuccessResult(pagination);
        }

        public async Task<Result<PublisherDto>> GetPublisherByIdAsync(int publisherId)
        {
            var spec = new BaseEntitySpecification<Publisher>(publisherId);
            var publisher = await unit.Repository<Publisher>().GetEntityWithSpec(spec);

            if (publisher is null)
                return Result<PublisherDto>.Failure(new Error(ErrorType.NotFound, "Publisher not found"));

            var dto = PublisherMapping.ToDto(publisher);
            return Result<PublisherDto>.SuccessResult(dto);
        }

        public async Task<Result<Publisher>> CreatePublisherAsync(CreatePublisherDto dto)
        {
            var publisher = PublisherMapping.ToEntity(dto);

            unit.Repository<Publisher>().Add(publisher);

            if (await unit.Complete())
            {
                logger.LogInformation("Created new publisher with Id={Id}, Name={Name}", publisher.Id, publisher.Name);
                return Result<Publisher>.SuccessResult(publisher);
            }

            logger.LogError("Failed to create publisher with Name={Name}", dto.Name);
            return Result<Publisher>.Failure(new Error(ErrorType.BadRequest, "Failed to create author"));
        }

        public async Task<Result<Publisher>> UpdatePublisherAsync(CreatePublisherDto dto, int id)
        {
            var publisher = await unit.Repository<Publisher>().GetByIdAsync(id);
            if (publisher == null)
            {
                logger.LogWarning("Publisher with Id={Id} not found for update", id);
                return Result<Publisher>.Failure(new Error(ErrorType.NotFound, "Publisher not found"));
            }

            publisher.Name = dto.Name;

            unit.Repository<Publisher>().Update(publisher);

            if (await unit.Complete())
            {
                logger.LogInformation("Updated publisher with Id={Id}, new Name={Name}", publisher.Id, publisher.Name);
                return Result<Publisher>.SuccessResult(publisher);
            }

            logger.LogError("Failed to update publisher with Id={Id}, Name={Name}", publisher.Id, publisher.Name);
            return Result<Publisher>.Failure(new Error(ErrorType.BadRequest, "Failed to update publisher"));
        }
        public async Task<Result<bool>> DeletePublisherAsync(int id)
        {
            var publisher = await unit.Repository<Publisher>().GetByIdAsync(id);
            if (publisher == null)
            {
                logger.LogWarning("Publisher with Id={Id} not found for deletion", id);
                return Result<bool>.Failure(new Error(ErrorType.NotFound, "Publisher not found"));
            }

            unit.Repository<Publisher>().Delete(publisher);

            if (await unit.Complete())
            {
                logger.LogInformation("Deleted publisher with Id={Id}, Name={Name}", publisher.Id, publisher.Name);
                return Result<bool>.SuccessResult(true);
            }

            logger.LogError("Failed to delete publisher with Id={Id}, Name={Name}", publisher.Id, publisher.Name);
            return Result<bool>.Failure(new Error(ErrorType.BadRequest, "Failed to delete publisher"));
        }
    }
}
