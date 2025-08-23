using Application.Common;
using Application.Common.Result;
using Application.Dtos.CatalogDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mapping;
using Application.Specificatios;
using Application.Specificatios.Params;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services
{
    public class BookService(IUnitOfWork unit, ILogger<BookService> logger) : IBookService
    {
        public async Task<Result<Pagination<CatalogBookDto>>> GetBooksAsync(BookSpecParams bookSpecParams)
        {
            logger.LogInformation("Fetching books with params: {@Params}", bookSpecParams);

            var spec = new BookCatalogSpecification(bookSpecParams);
            var books = await unit.Repository<Book>().ListWithSpecAsync(spec);
            var count = await unit.Repository<Book>().CountSpecAsync(spec);

            var booksDto = books.Select(CatalogMapping.MapBookToDto).ToList();
            var pagination = new Pagination<CatalogBookDto>(bookSpecParams.PageIndex, bookSpecParams.PageSize, count, booksDto);

            return Result<Pagination<CatalogBookDto>>.SuccessResult(pagination);
        }

        public async Task<Result<SingleBookDto>> GetBookByIdAsync(int id)
        {
            var spec = new SingleBookSpecification(id);
            var book = await unit.Repository<Book>().GetEntityWithSpec(spec);

            if (book is null)
                return Result<SingleBookDto>.Failure(new Error(ErrorType.NotFound, "Book not found"));

            var dto = CatalogMapping.MapBookToSingleDto(book);
            return Result<SingleBookDto>.SuccessResult(dto);
        }

        public async Task<Result<decimal>> GetBookRatingAsync(int id)
        {
            var book = await unit.Repository<Book>().GetByIdAsync(id);

            if (book == null)
                return Result<decimal>.Failure(new Error(ErrorType.NotFound, "Book not found"));

            return Result<decimal>.SuccessResult(book.Rating);
        }

        public async Task<Result<IReadOnlyList<CatalogBookDto>>> GetTopRatedBooksAsync()
        {
            var spec = new TopRatedBooksSpecification();
            var books = await unit.Repository<Book>().ListWithSpecAsync(spec);
            var dtos = books.Select(CatalogMapping.MapBookToDto).ToList();
            return Result<IReadOnlyList<CatalogBookDto>>.SuccessResult(dtos);
        }

        public async Task<Result<IReadOnlyList<CatalogBookDto>>> GetNewestBooksAsync()
        {
            var spec = new NewestBooksSpecification();
            var books = await unit.Repository<Book>().ListWithSpecAsync(spec);
            var dtos = books.Select(CatalogMapping.MapBookToDto).ToList();
            return Result<IReadOnlyList<CatalogBookDto>>.SuccessResult(dtos);
        }
    }
}
