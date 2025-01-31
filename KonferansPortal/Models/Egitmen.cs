using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace KonferansPortal.Models
{
    public class Egitmen
    {
        public int EgitmenId { get; set; }
        public string UyeId { get; set; }
        public Uye UyeModel;
        public List<Konferans> EgitilenKonferans { get; set; }

    }
}
