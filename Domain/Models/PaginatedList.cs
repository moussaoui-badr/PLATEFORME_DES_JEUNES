using Microsoft.EntityFrameworkCore;

namespace Domain.Models
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);

            this.AddRange(items);
        }

        public bool HasPreviousPage => PageIndex > 1;

        public bool HasNextPage => PageIndex < TotalPages;

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, pageIndex, pageSize);
        }

        public List<int> DisplayPages
        {
            get
            {
                var pages = new List<int>();

                var start = Math.Max(1, PageIndex - 2);
                var end = Math.Min(TotalPages, PageIndex + 2);

                for (var i = start; i <= end; i++)
                {
                    pages.Add(i);
                }

                return pages;
            }
        }

    }
}
