using Application.Common;
using Application.Common.Result;
using Application.Dtos.AdminDtos.GenreDtos;
using Application.Specificatios.Params;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IAdminGenreService
    {
        Task<Result<Pagination<GenreDto>>> GetAllGenresAsync(PaginationParams paginationParams);
        Task<Result<GenreDto>> GetGenreByIdAsync(int genreId);
        Task<Result<Genre>> CreateGenreAsync(CreateGenreDto dto);
        Task<Result<Genre>> UpdateGenreAsync(CreateGenreDto dto, int id);
        Task<Result<bool>> DeleteGenreAsync(int id);
    }
}
