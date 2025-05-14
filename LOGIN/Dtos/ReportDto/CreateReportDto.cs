using System.ComponentModel.DataAnnotations;

namespace LOGIN.Dtos.ReportDto
{
    public class CreateReportDto
    {
        [Display(Name = "clave Catastral")]
        [Required(ErrorMessage = "La {0} es Requerida")]
        public string Key { get; set; }

        [Display(Name = "nombre del abonado")]
        [Required(ErrorMessage = "El {0} es Requerido")]
        public string Name { get; set; }

        [Display(Name = "identidad")]
        [Required(ErrorMessage = "La {0} es Requerida")]
        public string DNI { get; set; }

        [Display(Name = "telefono celular")]
        [Required(ErrorMessage = "El {0} es Requerido")]
        public string Cellphone { get; set; }

        [Display(Name = "fecha")]
        [Required(ErrorMessage = "La {0} es Requerida")]
        public DateTime Date { get; set; }

        [Display(Name = "reporte")]
        [Required(ErrorMessage = "El {0} es Requerido")]
        public string Report { get; set; }

        [Display(Name = "direccion exacta")]
        [Required(ErrorMessage = "La {0} es Requerida")]
        public string Direction { get; set; }

        [Display(Name = "observacion")]
        [Required(ErrorMessage = "La {0} es Requerida")]
        public string Observation { get; set; }

        public List<IFormFile> Files { get; set; }

    }
}