

namespace KonferansPortal.Models
{
    public class PagedDuyuruViewModel
    {
        public IEnumerable<Duyurular> Duyurular { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}
