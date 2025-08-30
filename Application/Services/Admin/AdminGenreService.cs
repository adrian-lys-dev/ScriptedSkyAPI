using Application.Common;
using Application.Common.Result;
using Application.Dtos.GenreDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mapping;
using Application.Specificatios.Params;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services.Admin
{
    public class AdminGenreService(IUnitOfWork unit, ILogger<AdminOrderService> logger) : IAdminGenreService
    {
        public async Task<Result<Pagination<GenreDto>>> GetAllGenresAsync(PaginationParams paginationParams)
        {
            var genres = await unit.Repository<Genre>().ListAllAsync();
            var count = genres.Count;

            if (genres == null || !genres.Any())
            {
                logger.LogWarning("Genres not found");
                return Result<Pagination<GenreDto>>.Failure(new Error(ErrorType.NotFound, "Genres not found"));
            }

            var pagedGenres = genres
                .Skip((paginationParams.PageIndex - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToDtoList();

            logger.LogInformation("Returning {Count} genres out of {Total}", pagedGenres.Count, count);

            var pagination = new Pagination<GenreDto>(
                paginationParams.PageIndex,
                paginationParams.PageSize,
                count,
                pagedGenres
            );

            return Result<Pagination<GenreDto>>.SuccessResult(pagination);
        }

        public async Task<Result<GenreDto>> GetGenreByIdAsync(int genreId)
        {
            var genre = await unit.Repository<Genre>().GetByIdAsync(genreId);

            if(genre == null)
            {
                logger.LogWarning("Genre not found");
                return Result<GenreDto>.Failure(new Error(ErrorType.NotFound, "Genre not found"));
            }

            return Result<GenreDto>.SuccessResult(genre.ToDto());
        }
    }
}
