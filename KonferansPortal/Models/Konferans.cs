namespace KonferansPortal.Models
{
    public class Konferans
    {
        public string Name { set; get; }
        public int Id { set; get; }
        public string Description { set; get; }
        public int Price { set; get; }
        public List<Uye> Katilimcilar { set; get; } = new List<Uye>();
        public List<Egitmen>? Egitmenler { set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string? ImageUrl { set; get; }
        public List<Paylasim>? Paylasimlar { set; get; }
        public List<Tartisma>? Tartismalar { set; get; } 

        public byte[]? KonferansImage { set; get;  }
    }
}
