namespace KonferansPortal.Models
{
    public class Duyurular
    {
        public String Title { get; set; }
        public String Content { get; set; }
        public int Id { set; get; }

        public DateTime Date { get; set; }
        public String? ImageUrl { get; set; }
    }
}
