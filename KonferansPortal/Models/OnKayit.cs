namespace KonferansPortal.Models
{
    public class OnKayit
    {
        public int Id { get; set; }
        public bool isChecked { get; set; }
        public Konferans konferans { get; set; }
        public Uye uye { get; set;  }
        public byte[]? dekontFile { get;  set; }
        public bool isPaid { get; set; }
    }
}
