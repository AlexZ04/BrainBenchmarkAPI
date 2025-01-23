namespace BrainBenchmarkAPI.Models
{
    public class GuidResponseModel
    {
        public Guid Id { get; set; }

        public GuidResponseModel(Guid id)
        {
            Id = id;
        }
    }
}
