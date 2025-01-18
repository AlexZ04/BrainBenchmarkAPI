using System.ComponentModel.DataAnnotations;

namespace BrainBenchmarkAPI.Models
{
    public class ResponseModel
    {
        [Required]
        public string Verdict { get; set; }
        public string Message { get; set; }

        public ResponseModel(string verdict, string message)
        {
            Verdict = verdict;
            Message = message;
        }
    }
}
