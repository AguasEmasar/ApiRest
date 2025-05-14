using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace LOGIN.Dtos.Communicates
{
    public class CreateCommunicateDto
    {
        [Display(Name = "título")]
        [Required(ErrorMessage = "El {0} es Requerido")]
        public string Tittle { get; set; }

        [Display(Name = "fecha")]
        [Required(ErrorMessage = "La {0} es Requerido")]
        public DateTime Date { get; set; }

        [Display(Name = "tipo de comunicado")]
        [Required(ErrorMessage = "El {0} es Requerido")]
        public string Type_Statement { get; set; }

        [Display(Name = "contenido")]
        [Required(ErrorMessage = "El {0} es Requerido")]
        public string Content { get; set; }

    }
}
