namespace KonferansPortal.Models
{
    public class Tartisma
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public Uye Publisher { get; set; }
        public List<Yorum>? Yorumlar { get; set; }
        public Konferans Konferans { get; set; }
    }
}
