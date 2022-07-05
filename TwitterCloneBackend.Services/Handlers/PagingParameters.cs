namespace TwitterCloneBackend.Services.Handlers
{
    public class PagingParameters
    {
        const int maxPageSize = 50;
        public int PageNumber { get; set; } = 1;

        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}

// Usage

//return FindAll()
//    .OrderBy(on => on.Name)
//    .Skip((PagingParameters.PageNumber - 1) * PagingParameters.PageSize)
//    .Take(PagingParameters.PageSize)
//    .ToList();