using Application.Common;
using Application.Common.Result;
using Application.Dtos.CatalogDtos;
using Application.Specificatios.Params;

namespace Application.Interfaces.Services
{
    public interface IBookService
    {
        Task<Result<Pagination<CatalogBookDto>>> GetBooksAsync(BookSpecParams bookSpecParams);
        Task<Result<SingleBookDto>> GetBookByIdAsync(int id);
        Task<Result<decimal>> GetBookRatingAsync(int id);
        Task<Result<IReadOnlyList<CatalogBookDto>>> GetTopRatedBooksAsync();
        Task<Result<IReadOnlyList<CatalogBookDto>>> GetNewestBooksAsync();
    }
}
