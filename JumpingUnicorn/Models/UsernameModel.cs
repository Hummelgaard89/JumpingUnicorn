using System.ComponentModel.DataAnnotations;

namespace JumpingUnicorn.Models
{
    public class UsernameModel
    {
        [Required]
        [RegularExpression(@"[A-z0-9_-]{6,}", ErrorMessage = "You typed wronged")]
        public string Username { get; set; }
    }
}
