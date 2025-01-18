using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BrainBenchmarkAPI.Models
{
    public class UserRegisterModel
    {
        [Required]
        [StringLength(1000, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [AllowNull]
        public DateTime? Birthday { get; set; }
        [Required]
        public Gender Gender { get; set; }
    }
}
