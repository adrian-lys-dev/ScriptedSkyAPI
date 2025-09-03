using Application.Specificatios.Base;
using Application.Specificatios.Params;
using Domain.Entities;
using Domain.Entities.Base;

namespace Application.Specificatios
{
    public class BaseEntitySpecification<T> : BaseSpecification<T> where T : BaseEntity
    {
        public BaseEntitySpecification(PaginationParams paginationParams) 
        {
            AddOrderBy(x => x.Id);
            ApplyPaging(paginationParams.PageSize * (paginationParams.PageIndex - 1),
                paginationParams.PageSize);
        }

        public BaseEntitySpecification(int id): base(x => x.Id == id) {}
    }

    public class BooksWithGenreSpecification : BaseSpecification<Book>
    {
        public BooksWithGenreSpecification(int genreId)
            : base(b => b.Genre.Any(g => g.Id == genreId)) { }
    }

    public class BooksWithAuthorSpecification : BaseSpecification<Book>
    {
        public BooksWithAuthorSpecification(int authorId)
            : base(b => b.Author.Any(a => a.Id == authorId)) { }
    }

    public class BooksWithPublisherSpecification : BaseSpecification<Book>
    {
        public BooksWithPublisherSpecification(int publisherId)
            : base(b => b.Publisher.Id == publisherId) { }
    }

}
