namespace KonferansPortal.Models
{
    public class Konferans
    {
        public string Name { set; get; }
        public int Id { set; get; }
        public string Description { set; get; }
        public int Price { set; get; }
        public List<Uye> Katilimcilar { set; get; }
        public List<Egitmen> Egitmenler {  set; get; }
        public DateTime StartDate { set; get; }
        public DateTime EndDate { set; get; }
        public string ImageUrl { set; get; }
    }
}
