using System.ComponentModel.DataAnnotations;

namespace Core5Identity.Models
{
    public class LoginModel
    {
        [Required]
        [UIHint("emaill")]
        public string Email { get; set; }

        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }
}
