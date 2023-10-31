namespace Domain.Models
{
    public class PagedList<T>
    {
        public IEnumerable<T> Result { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
        public int Page { get; set; }
        public int Count { get; set; }
        public int PageSize { get; set; }
    }
}
