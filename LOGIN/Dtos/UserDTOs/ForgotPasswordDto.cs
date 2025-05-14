using System.ComponentModel.DataAnnotations;

namespace LOGIN.Dtos.UserDTOs
{
    public class ForgotPasswordDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
