using System.ComponentModel.DataAnnotations;

namespace LOGIN.Dtos.UserDTOs
{
    public class CreateUserDto
    {
        public string Id { get; set; }
        [Display(Name = "nombre de usuario")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        [StringLength(50, ErrorMessage = "El {0} no puede exceder los 50 caracteres.")]
        public string UserName { get; set; }

        [Display(Name = "correo electrónico")]
        [Required(ErrorMessage = "El {0} es requerido.")]
        [EmailAddress(ErrorMessage = "El {0} no tiene un formato válido.")]
        public string Email { get; set; }

        [Display(Name = "contraseña")]
        [Required(ErrorMessage = "La {0} es requerida.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "La {0} debe tener entre {2} y {1} caracteres.")]
        public string Password { get; set; }

        [Display(Name = "nombre")]
        [Required(ErrorMessage = "El {0} es requerida.")]
        [StringLength(50, ErrorMessage = "El {0} no puede exceder los 50 caracteres.")]
        public string FirstName { get; set; }

        [Display(Name = "apellido")]
        [Required(ErrorMessage = "El {0} es requerida.")]
        [StringLength(50, ErrorMessage = "El {0} no puede exceder los 50 caracteres.")]
        public string LastName { get; set; }

        [Display(Name = "rol")]
        [Required(ErrorMessage = "Debe asignar al menos un {0}.")]
        public List<string> Roles { get; set; } = new List<string>();
    }
}
