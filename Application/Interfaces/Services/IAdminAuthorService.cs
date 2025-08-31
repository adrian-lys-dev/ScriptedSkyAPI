using Application.Common;
using Application.Common.Result;
using Application.Dtos.AdminDtos.AuthorDtos;
using Application.Specificatios.Params;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IAdminAuthorService
    {
        Task<Result<Pagination<AuthorDto>>> GetAllAuthorsAsync(PaginationParams paginationParams);
        Task<Result<AuthorDto>> GetAuthorByIdAsync(int genreId);
        Task<Result<Author>> CreateAuthorAsync(CreateAuthorDto dto);
        Task<Result<Author>> UpdateAuthorAsync(CreateAuthorDto dto, int id);
        Task<Result<bool>> DeleteAuthorAsync(int id);
    }
}
