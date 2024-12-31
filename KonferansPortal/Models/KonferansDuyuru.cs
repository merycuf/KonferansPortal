namespace KonferansPortal.Models
{
    public class KonferansDuyuru
    {
        public Konferans Konferans { get; set; }
        public String Title { get; set; }
        public String Content { get; set; }
        public int Id { set; get; }

        public DateTime Date { get; set; }
        public Uye Publisher { get; set; }
        public byte[]? FileData { get; set; }



    }
}
