using Application.Common;
using Application.Common.Result;
using Application.Dtos.AdminDtos.AuthorDtos;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Application.Mapping;
using Application.Specificatios;
using Application.Specificatios.Params;
using Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Application.Services.Admin
{
    public class AdminAuthorService(IUnitOfWork unit, ILogger<AdminAuthorService> logger) : IAdminAuthorService
    {
        public async Task<Result<Pagination<AuthorDto>>> GetAllAuthorsAsync(PaginationParams paginationParams)
        {
            var spec = new BaseEntitySpecification<Author>(paginationParams);
            var authors = await unit.Repository<Author>().ListWithSpecAsync(spec);
            var count = await unit.Repository<Author>().CountSpecAsync(spec);

            var authorsDto = authors.Select(AuthorMapping.ToDto).ToList();
            var pagination = new Pagination<AuthorDto>(paginationParams.PageIndex, paginationParams.PageSize, count, authorsDto);

            return Result<Pagination<AuthorDto>>.SuccessResult(pagination);
        }

        public async Task<Result<AuthorDto>> GetAuthorByIdAsync(int authorId)
        {
            var spec = new BaseEntitySpecification<Author>(authorId);
            var author = await unit.Repository<Author>().GetEntityWithSpec(spec);

            if (author is null)
                return Result<AuthorDto>.Failure(new Error(ErrorType.NotFound, "Author not found"));

            var dto = AuthorMapping.ToDto(author);
            return Result<AuthorDto>.SuccessResult(dto);
        }

        public async Task<Result<Author>> CreateAuthorAsync(CreateAuthorDto dto)
        {
            var author = AuthorMapping.ToEntity(dto);

            unit.Repository<Author>().Add(author);

            if (await unit.Complete())
            {
                logger.LogInformation("Created new author with Id={Id}, Name={Name}", author.Id, author.Name);
                return Result<Author>.SuccessResult(author);
            }

            logger.LogError("Failed to create author with Name={Name}", dto.Name);
            return Result<Author>.Failure(new Error(ErrorType.BadRequest, "Failed to create author"));
        }

        public async Task<Result<Author>> UpdateAuthorAsync(CreateAuthorDto dto, int id)
        {
            var author = await unit.Repository<Author>().GetByIdAsync(id);
            if (author == null)
            {
                logger.LogWarning("Author with Id={Id} not found for update", id);
                return Result<Author>.Failure(new Error(ErrorType.NotFound, "Author not found"));
            }

            author.Name = dto.Name;

            unit.Repository<Author>().Update(author);

            if (await unit.Complete())
            {
                logger.LogInformation("Updated author with Id={Id}, new Name={Name}", author.Id, author.Name);
                return Result<Author>.SuccessResult(author);
            }

            logger.LogError("Failed to update author with Id={Id}, Name={Name}", author.Id, author.Name);
            return Result<Author>.Failure(new Error(ErrorType.BadRequest, "Failed to update author"));
        }

        public async Task<Result<bool>> DeleteAuthorAsync(int id)
        {
            var author = await unit.Repository<Author>().GetByIdAsync(id);
            if (author == null)
            {
                logger.LogWarning("Author with Id={Id} not found for deletion", id);
                return Result<bool>.Failure(new Error(ErrorType.NotFound, "Author not found"));
            }

            var spec = new BooksWithAuthorSpecification(id);
            var count = await unit.Repository<Book>().CountSpecAsync(spec);

            if (count > 0)
            {
                logger.LogWarning("Cannot delete author Id={Id}, it is used in {Count} books", id, count);
                return Result<bool>.Failure(
                    new Error(ErrorType.BadRequest, $"Cannot delete author because it is used in {count} books")
                );
            }

            unit.Repository<Author>().Delete(author);

            if (await unit.Complete())
            {
                logger.LogInformation("Deleted author with Id={Id}, Name={Name}", author.Id, author.Name);
                return Result<bool>.SuccessResult(true);
            }

            logger.LogError("Failed to delete author with Id={Id}, Name={Name}", author.Id, author.Name);
            return Result<bool>.Failure(new Error(ErrorType.BadRequest, "Failed to delete author"));
        }
    }
}
