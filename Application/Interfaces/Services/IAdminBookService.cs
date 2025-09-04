using Application.Common;
using Application.Common.Result;
using Application.Dtos.BookDtos;
using Application.Specificatios.Params;
using Domain.Entities;

namespace Application.Interfaces.Services
{
    public interface IAdminBookService
    {
        Task<Result<Pagination<BookDto>>> GetAllBooksAsync(PaginationParams paginationParams);
        Task<Result<BookDetailsDto>> GetBookByIdAsync(int bookId);
        Task<Result<Book>> CreateBookAsync(CreateBookDto createBookDto);
        Task<Result<Book>> UpdateBookAsync(int bookId, CreateBookDto updateDto);
        Task<Result> DeleteBookAsync(int bookId);
    }
}
