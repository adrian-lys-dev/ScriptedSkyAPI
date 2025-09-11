using Application.Common;
using Application.Common.Result;
using Application.Dtos.BookDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mapping;
using Application.Specificatios;
using Application.Specificatios.Params;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services.Admin
{
    public class AdminBookService(IUnitOfWork unit, ILogger<BookService> logger, IImageService imageService) : IAdminBookService
    {
        public async Task<Result<Pagination<BookDto>>> GetAllBooksAsync(PaginationParams paginationParams)
        {
            var spec = new BookSpecification(paginationParams);
            var books = await unit.Repository<Book>().ListWithSpecAsync(spec);
            var count = await unit.Repository<Book>().CountSpecAsync(spec);

            var booksDto = books.Select(BookMapping.ToDto).ToList();
            var pagination = new Pagination<BookDto>(paginationParams.PageIndex, paginationParams.PageSize, count, booksDto);

            return Result<Pagination<BookDto>>.SuccessResult(pagination);
        }

        public async Task<Result<BookDetailsDto>> GetBookByIdAsync(int bookId)
        {
            var spec = new BookSpecification(bookId);
            var book = await unit.Repository<Book>().GetEntityWithSpec(spec);

            if (book is null)
                return Result<BookDetailsDto>.Failure(new Error(ErrorType.NotFound, "Book not found"));

            var dto = BookMapping.ToDetailsDto(book);
            return Result<BookDetailsDto>.SuccessResult(dto);
        }

        public async Task<Result<Book>> CreateBookAsync(CreateBookDto createBookDto)
        {
            var spec = new BookSpecification(createBookDto.Title);
            var existBook = await unit.Repository<Book>().GetEntityWithSpec(spec);

            if (existBook != null)
                return Result<Book>.Failure(new Error(ErrorType.BadRequest, "There is a book with such a name already"));

            if (createBookDto.Picture == null)
                return Result<Book>.Failure(new Error(ErrorType.BadRequest, "Picture required"));

            var relationsResult = await GetBookRelationsAsync(createBookDto);

            var (authors, genres, publisher) = relationsResult.Value;

            var pictureUrl = await imageService.SaveImageAsync(createBookDto.Picture);

            if (!pictureUrl.Success)
                return Result<Book>.Failure(pictureUrl.Error!);

            var book = BookMapping.ToEntity(createBookDto, authors, genres, publisher, pictureUrl.Value!);

            unit.Repository<Book>().Add(book);

            if (!await unit.Complete())
                return Result<Book>.Failure(new Error(ErrorType.BadRequest, "Problem creating the book"));

            return Result<Book>.SuccessResult(book);
        }

        public async Task<Result<Book>> UpdateBookAsync(int bookId, CreateBookDto updateDto)
        {
            var bookSpec = new SingleBookSpecification(bookId);
            var book = await unit.Repository<Book>().GetEntityWithSpec(bookSpec);

            if (book == null)
                return Result<Book>.Failure(new Error(ErrorType.NotFound, "Book not found"));

            var titleSpec = new BookSpecification(updateDto.Title);
            var existingBook = await unit.Repository<Book>().GetEntityWithSpec(titleSpec);
            if (existingBook != null && existingBook.Id != bookId)
            {
                return Result<Book>.Failure(new Error(ErrorType.BadRequest, "Another book with the same title already exists"));
            }

            var relationsResult = await GetBookRelationsAsync(updateDto);
            if (!relationsResult.Success)
                return Result<Book>.Failure(relationsResult.Error!);

            var (authors, genres, publisher) = relationsResult.Value;

            if (updateDto.Picture != null)
            {
                var pictureUrl = await imageService.SaveImageAsync(updateDto.Picture);
                if (!pictureUrl.Success)
                    return Result<Book>.Failure(pictureUrl.Error!);

                book.PictureURL = pictureUrl.Value!;
            }

            BookMapping.UpdateEntity(book, updateDto, authors, genres, publisher);

            unit.Repository<Book>().Update(book);

            if (!await unit.Complete())
                return Result<Book>.Failure(new Error(ErrorType.BadRequest, "Problem updating the book"));

            return Result<Book>.SuccessResult(book);
        }

        public async Task<Result> DeleteBookAsync(int bookId)
        {
            var book = await unit.Repository<Book>().GetByIdAsync(bookId);
            if (book == null)
                return Result.Failure(new Error(ErrorType.NotFound, "Book not found"));

            var spec = new BooksWithOrderSpecification(bookId);
            var count = await unit.Repository<Book>().CountSpecAsync(spec);

            if (count > 0)
            {
                logger.LogWarning("Cannot delete book Id={Id}, it is used in orders", bookId);
                return Result<bool>.Failure(
                    new Error(ErrorType.BadRequest, $"Cannot delete book Id={bookId}, it is used in orders")
                );
            }

            unit.Repository<Book>().Delete(book);

            if (!await unit.Complete())
                return Result.Failure(new Error(ErrorType.BadRequest, "Problem deleting the book"));

            return Result.SuccessResult();
        }

        private async Task<Result<(Author[] authors, Genre[] genres, Publisher publisher)>> GetBookRelationsAsync(CreateBookDto createBookDto)
        {
            var authors = new List<Author>();
            foreach (var id in createBookDto.AuthorIds)
            {
                var author = await unit.Repository<Author>().GetByIdAsync(id);
                if (author == null)
                {
                    logger.LogWarning("Author not found for id {Id}", id);
                    return Result<(Author[], Genre[], Publisher)>.Failure(
                        new Error(ErrorType.BadRequest, $"Author not found: {id}"));
                }
                authors.Add(author);
            }

            var genres = new List<Genre>();
            foreach (var id in createBookDto.GenreIds)
            {
                var genre = await unit.Repository<Genre>().GetByIdAsync(id);
                if (genre == null)
                {
                    logger.LogWarning("Genre not found for id {Id}", id);
                    return Result<(Author[], Genre[], Publisher)>.Failure(
                        new Error(ErrorType.BadRequest, $"Genre not found: {id}"));
                }
                genres.Add(genre);
            }

            var publisher = await unit.Repository<Publisher>().GetByIdAsync(createBookDto.PublisherId);
            if (publisher == null)
            {
                logger.LogWarning("Publisher not found for id {Id}", createBookDto.PublisherId);
                return Result<(Author[], Genre[], Publisher)>.Failure(
                    new Error(ErrorType.BadRequest, $"Publisher not found: {createBookDto.PublisherId}"));
            }

            return Result<(Author[], Genre[], Publisher)>.SuccessResult(
                (authors.ToArray(), genres.ToArray(), publisher));
        }

    }
}
