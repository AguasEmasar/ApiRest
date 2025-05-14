using System.ComponentModel.DataAnnotations;

namespace LOGIN.Dtos.ReportDto
{
    public class UpdateReportDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Debe adjuntar al menos un archivo")]
        public List<IFormFile> Files { get; set; }  // Archivos de imagen para actualizar

        [Required]
        [StringLength(20, ErrorMessage = "La clave no puede exceder los 20 caracteres.")]
        public string Key { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre no puede exceder los 150 caracteres.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "El DNI es obligatorio.")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "El DNI debe contener 13 dígitos.")]
        public string DNI { get; set; }

        [Required(ErrorMessage = "El número de celular es obligatorio.")]
        [Phone(ErrorMessage = "Debe ser un número de teléfono válido.")]
        [StringLength(15, ErrorMessage = "El número de celular no puede exceder los 15 caracteres.")]
        public string Cellphone { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Debe ingresar el reporte.")]
        public string Report { get; set; }

        [Required(ErrorMessage = "La dirección es obligatoria.")]
        [StringLength(200, ErrorMessage = "La dirección no puede exceder los 200 caracteres.")]
        public string Direction { get; set; }

        [StringLength(500, ErrorMessage = "La observación no puede exceder los 500 caracteres.")]
        public string Observation { get; set; }

        [Required]
        public Guid StateId { get; set; }
    }
}