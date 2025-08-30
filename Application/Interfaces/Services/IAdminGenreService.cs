using Application.Common;
using Application.Common.Result;
using Application.Dtos.GenreDtos;
using Application.Specificatios.Params;

namespace Application.Interfaces.Services
{
    public interface IAdminGenreService
    {
        Task<Result<Pagination<GenreDto>>> GetAllGenresAsync(PaginationParams paginationParams);
        Task<Result<GenreDto>> GetGenreByIdAsync(int genreId);
    }
}
