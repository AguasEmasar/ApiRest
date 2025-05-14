using System.ComponentModel.DataAnnotations;

namespace LOGIN.Dtos.ScheduleDtos.NeighborhoodsColonies
{
    public class NeighborhoodsColoniesCreateDto
    {
        [Display(Name = "nombre")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(maximumLength: 30, MinimumLength = 3, ErrorMessage = "El {0} debe tener entre {2} y {1} caracteres")]
        public string Name { get; set; }

        [Required]
        public Guid BlockId { get; set; }
    }
}
