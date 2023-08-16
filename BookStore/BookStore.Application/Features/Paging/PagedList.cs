namespace BookStore.Application.Features.Paging
{
    public class PagedList<T> : List<T>
    {
        public PagesData PagesData { get; set; } = default!;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            PagesData = new PagesData
            {
                TotalCount = count,
                PageSize = pageSize,
                CurrentPage = pageNumber,
                TotalPages = (int)Math.Ceiling(count / (double)pageSize),
            };

            AddRange(items);  
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}