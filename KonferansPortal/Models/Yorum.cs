namespace KonferansPortal.Models
{
    public class Yorum
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public Uye Publisher { get; set; }
        public Yorum? Cevaplanan { get; set; }
    }
}
