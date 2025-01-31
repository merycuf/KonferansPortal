using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace KonferansPortal.Models
{
    public class Egitmen : Uye
    {
        public Egitmen fillEgitmen(Uye uye)
        {
            Egitmen egitmen = new Egitmen();
            egitmen.Id = uye.Id ;
            egitmen.Discriminator = "Egitmen";
            egitmen.Name = uye.Name;
            egitmen.Surname = uye.Surname;
            egitmen.Email = uye.Email;
            egitmen.Phone = uye.Phone;
            egitmen.UserName = uye.UserName;
            egitmen.NormalizedUserName = uye.NormalizedUserName;
            egitmen.NormalizedEmail = uye.NormalizedEmail;
            egitmen.EmailConfirmed = uye.EmailConfirmed;
            egitmen.PasswordHash = uye.PasswordHash;
            egitmen.SecurityStamp = uye.SecurityStamp;
            egitmen.ConcurrencyStamp = uye.ConcurrencyStamp;
            egitmen.PhoneNumber = uye.PhoneNumber;
            egitmen.PhoneNumberConfirmed = uye.PhoneNumberConfirmed;
            egitmen.TwoFactorEnabled = uye.TwoFactorEnabled;
            egitmen.LockoutEnd = uye.LockoutEnd;
            egitmen.LockoutEnabled = uye.LockoutEnabled;
            egitmen.AccessFailedCount = uye.AccessFailedCount;
            return egitmen;
        }
        public List<Konferans> EgitilenKonferans { get; set; }

    }
}
