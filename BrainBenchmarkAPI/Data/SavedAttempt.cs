namespace BrainBenchmarkAPI.Data
{
    public class SavedAttempt
    {
        public Guid Id { get; set; }

        public SavedAttempt()
        {
            Id = Guid.NewGuid();
        }
        public SavedAttempt(Guid id)
        {
            Id = id;
        }
    }
}
