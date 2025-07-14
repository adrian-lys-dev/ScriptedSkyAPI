using Core.Entities;
using Core.Specificatios.Base;
using Core.Specificatios.Params;

namespace Core.Specificatios
{
    public class BookCatalogSpecification : BaseSpecification<Book>
    {
        public BookCatalogSpecification(BookSpecParams bookSpecParams) : base(p =>
            (string.IsNullOrEmpty(bookSpecParams.Search) || p.Title.ToLower().Contains(bookSpecParams.Search)) && 
            (bookSpecParams.GenreIds == null || bookSpecParams.GenreIds.Count == 0 || p.Genre!.Any(x => bookSpecParams.GenreIds.Contains(x.Id))) && 
            (bookSpecParams.AuthorIds == null || bookSpecParams.AuthorIds.Count == 0 || p.Author!.Any(x => bookSpecParams.AuthorIds.Contains(x.Id))) && 
            (bookSpecParams.PublisherIds == null || bookSpecParams.PublisherIds.Count == 0 || bookSpecParams.PublisherIds.Contains(p.PublisherId)))
        {
            AddInclude(p => p.Author);
            AddInclude(p => p.Genre);
            AddInclude(p => p.Publisher!);

            ApplyPaging(bookSpecParams.PageSize * (bookSpecParams.PageIndex - 1), bookSpecParams.PageSize);

            switch (bookSpecParams.Sort)
            {
                case "priceAsc":
                    AddOrderBy(p => p.Price);
                    break;
                case "priceDesc":
                    AddOrderByDescending(p => p.Price);
                    break;
                default:
                    AddOrderBy(p => p.Title);
                    break;
            }
        }
    }
}
