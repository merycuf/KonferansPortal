using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace KonferansPortal.Models
{
    public class Egitmen : Uye
    {
        public Egitmen fillEgitmen(Uye uye)
        {
            Egitmen egitmen = new Egitmen();
            egitmen.Name = uye.Name ;
            egitmen.Surname = uye.Surname ;
            egitmen.Id = uye.Id ;
            egitmen.Email = uye.Email;
            //Username = egitmen.UserName;
            egitmen.Discriminator = "Egitmen";
            return egitmen;
        }
        public List<Konferans> EgitilenKonferans { get; set; }

    }
}
