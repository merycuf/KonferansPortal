using System.ComponentModel.DataAnnotations;

namespace KonferansPortal.Models
{
    public class TartismaViewModel
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int KonferansId { get; set; }
    }
}