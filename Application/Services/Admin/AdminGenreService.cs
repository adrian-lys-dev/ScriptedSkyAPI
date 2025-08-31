using Application.Common;
using Application.Common.Result;
using Application.Dtos.GenreDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mapping;
using Application.Specificatios;
using Application.Specificatios.Params;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services.Admin
{
    public class AdminGenreService(IUnitOfWork unit, ILogger<AdminOrderService> logger) : IAdminGenreService
    {
        public async Task<Result<Pagination<GenreDto>>> GetAllGenresAsync(PaginationParams paginationParams)
        {
            var spec = new BaseEntitySpecification<Genre>(paginationParams);
            var books = await unit.Repository<Genre>().ListWithSpecAsync(spec);
            var count = await unit.Repository<Genre>().CountSpecAsync(spec);

            var genres = books.Select(GenreMapping.ToDto).ToList();
            var pagination = new Pagination<GenreDto>(paginationParams.PageIndex, paginationParams.PageSize, count, genres);

            return Result<Pagination<GenreDto>>.SuccessResult(pagination);
        }

        public async Task<Result<GenreDto>> GetGenreByIdAsync(int genreId)
        {
            var spec = new BaseEntitySpecification<Genre>(genreId);
            var genre = await unit.Repository<Genre>().GetEntityWithSpec(spec);

            if (genre is null)
                return Result<GenreDto>.Failure(new Error(ErrorType.NotFound, "Genre not found"));

            var dto = GenreMapping.ToDto(genre);
            return Result<GenreDto>.SuccessResult(dto);
        }

        public async Task<Result<Genre>> CreateGenreAsync(CreateGenreDto dto)
        {
            var genre = GenreMapping.ToEntity(dto);

            unit.Repository<Genre>().Add(genre);

            if (await unit.Complete())
            {
                logger.LogInformation("Created new genre with Id={Id}, Name={Name}", genre.Id, genre.Name);
                return Result<Genre>.SuccessResult(genre);
            }

            logger.LogError("Failed to create genre with Name={Name}", dto.Name);
            return Result<Genre>.Failure(new Error(ErrorType.BadRequest, "Failed to create genre"));
        }

        public async Task<Result<Genre>> UpdateGenreAsync(CreateGenreDto dto, int id)
        {
            var genre = await unit.Repository<Genre>().GetByIdAsync(id);
            if (genre == null)
            {
                logger.LogWarning("Genre with Id={Id} not found for update", id);
                return Result<Genre>.Failure(new Error(ErrorType.NotFound, "Genre not found"));
            }

            genre.Name = dto.Name;

            unit.Repository<Genre>().Update(genre);

            if (await unit.Complete())
            {
                logger.LogInformation("Updated genre with Id={Id}, new Name={Name}", genre.Id, genre.Name);
                return Result<Genre>.SuccessResult(genre);
            }

            logger.LogError("Failed to update genre with Id={Id}, Name={Name}", genre.Id, genre.Name);
            return Result<Genre>.Failure(new Error(ErrorType.BadRequest, "Failed to update genre"));
        }

        public async Task<Result<bool>> DeleteGenreAsync(int id)
        {
            var genre = await unit.Repository<Genre>().GetByIdAsync(id);
            if (genre == null)
            {
                logger.LogWarning("Genre with Id={Id} not found for deletion", id);
                return Result<bool>.Failure(new Error(ErrorType.NotFound, "Genre not found"));
            }

            unit.Repository<Genre>().Delete(genre);

            if (await unit.Complete())
            {
                logger.LogInformation("Deleted genre with Id={Id}, Name={Name}", genre.Id, genre.Name);
                return Result<bool>.SuccessResult(true);
            }

            logger.LogError("Failed to delete genre with Id={Id}, Name={Name}", genre.Id, genre.Name);
            return Result<bool>.Failure(new Error(ErrorType.BadRequest, "Failed to delete genre"));
        }

    }
}
