namespace BrainBenchmarkAPI.Data
{
    public class SavedAttemptDb
    {
        public Guid Id { get; set; }
        public UserDb Player { get; set; }

        public SavedAttemptDb()
        {
            Id = Guid.NewGuid();
        }
        public SavedAttemptDb(Guid id, UserDb user)
        {
            Id = id;
            Player = user;
        }
    }
}
