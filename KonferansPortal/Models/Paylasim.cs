namespace KonferansPortal.Models
{
    public class Paylasim
    {
        public String Title { get; set; }
        public String? Content { get; set; }
        public int Id { set; get; }

        public DateTime Date { get; set; }
        public Uye Publisher { get; set; }
        public byte[]? ContentFile { set; get; }

        public List<Yorum> Yorumlar { get; set; }
        public Konferans PaylasilanKonferans { get; set; }
    }
}
