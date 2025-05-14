using System.ComponentModel.DataAnnotations;

namespace LOGIN.Dtos.RolDTOs
{
    public class CreateRoleDto
    {
        [Display(Name = "RoleName")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(maximumLength: 20, MinimumLength = 3, ErrorMessage = "El {0} debe tener entre {2} y {1} caracteres")]
        public string RoleName { get; set; }
    }
}
