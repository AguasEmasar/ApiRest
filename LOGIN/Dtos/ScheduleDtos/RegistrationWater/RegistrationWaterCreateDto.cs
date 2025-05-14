using System.ComponentModel.DataAnnotations;

namespace LOGIN.Dtos.ScheduleDtos.RegistrationWater
{
    public class RegistrationWaterCreateDto
    {
        [Required]
        public List<Guid> NeighborhoodColoniesId { get; set; } = new List<Guid>();

        [Display(Name = "fecha")]
        [Required(ErrorMessage = "La {0} es requerida")]
        [DataType(DataType.Date, ErrorMessage = "El formato de la fecha es inválido.")]
        public DateTime Date { get; set; }

        [Display(Name = "observacion")]
        [Required(ErrorMessage = "La {0} es requerida")]
        [StringLength(1000, ErrorMessage = "Las observaciones no pueden exceder los 1000 caracteres.")]
        public string Observations { get; set; }
    }
}
