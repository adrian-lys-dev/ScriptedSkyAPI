namespace Application.Specificatios.Params
{
    public class BookSpecParams
    {
        private const int MaxPageSize = 50;
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 6;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
        }
        public List<int>? AuthorIds { get; set; }
        public List<int>? GenreIds { get; set; }
        public List<int>? PublisherIds { get; set; }
        public string? Sort { get; set; }

        private string? _search;
        public string Search
        {
            get => _search ?? "";
            set => _search = value;
        }
    }

}
