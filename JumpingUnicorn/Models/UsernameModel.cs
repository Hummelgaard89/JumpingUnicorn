using System.ComponentModel.DataAnnotations;

namespace JumpingUnicorn.Models
{
    public class UsernameModel
    {
        [Required]
        [RegularExpression(@"[A-z0-9_-]{6,}", ErrorMessage = "Error: Invalid input. Please enter a string that contains at least 6 consecutive characters that are either uppercase letters A-Z, lowercase letters a-z, digits 0-9")]
        public string Username { get; set; }
    }
}
