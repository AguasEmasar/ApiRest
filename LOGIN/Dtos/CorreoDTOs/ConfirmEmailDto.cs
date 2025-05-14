using System.ComponentModel.DataAnnotations;

public class ConfirmEmailDto
{
    [Display(Name = "userId")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string UserId { get; set; }

    [Display(Name = "token")]
    [Required(ErrorMessage = "El campo {0} es obligatorio.")]
    public string Token { get; set; }
}
