using System.ComponentModel.DataAnnotations;

namespace BrainBenchmarkAPI.Models
{
    public class TokenResponseModel
    {
        [Required]
        [MinLength(1)]
        public string Token { get; set; }

        public TokenResponseModel(string token) 
        {
            Token = token;
        }
    }
}
