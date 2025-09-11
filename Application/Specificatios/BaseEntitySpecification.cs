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
            AddOrderByDescending(x => x.CreatedAt);
            ApplyPaging(paginationParams.PageSize * (paginationParams.PageIndex - 1),
                paginationParams.PageSize);
        }

        public BaseEntitySpecification(int id): base(x => x.Id == id) {}
    }


    public class BooksWithOrderSpecification : BaseSpecification<Book>
    {
        public BooksWithOrderSpecification(int bookId) 
            : base(b => b.Id == bookId && b.OrderItems.Any()) { }
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

    public class GenreByNameSpecification : BaseSpecification<Genre>
    {
        public GenreByNameSpecification(string name)
            : base(g => g.Name.ToLower() == name.ToLower()) { }
    }

    public class AuthorByNameSpecification : BaseSpecification<Author>
    {
        public AuthorByNameSpecification(string name)
            : base(a => a.Name.ToLower() == name.ToLower()) { }
    }

    public class PublisherByNameSpecification : BaseSpecification<Publisher>
    {
        public PublisherByNameSpecification(string name)
            : base(p => p.Name.ToLower() == name.ToLower()) { }
    }

}
