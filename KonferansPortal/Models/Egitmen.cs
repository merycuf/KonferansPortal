using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace KonferansPortal.Models
{
    public class Egitmen : Uye
    {
        public Egitmen(Uye egitmen)
        {
            Name = egitmen.Name;
            Surname = egitmen.Surname;
            Id = egitmen.Id;
            Email = egitmen.Email ;
            //Username = egitmen.UserName;
            Discriminator = "Egitmen" ;

        }

        public List<Konferans> EgitilenKonferans { get; set; }

    }
}
