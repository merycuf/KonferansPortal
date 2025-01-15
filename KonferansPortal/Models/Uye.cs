using Microsoft.AspNetCore.Identity;

namespace KonferansPortal.Models
{
    public class Uye : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Discriminator { get; set; }
        public List<Konferans> katilinanKonferanslar { get; set; }

    }
}
