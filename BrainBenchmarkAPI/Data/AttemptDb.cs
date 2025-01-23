namespace BrainBenchmarkAPI.Data
{
    public class AttemptDb
    {
        public Guid Id { get; set; }
        public UserDb Player { get; set; }
        public GameDb Game { get; set; }
        public int Result { get; set; }
        public DateTime AttemptDate { get; set; }

        public AttemptDb()
        {
            Id = Guid.NewGuid();
            AttemptDate = DateTime.Now.ToUniversalTime();
        }

        public AttemptDb(UserDb player, GameDb game, int result)
        {
            Id = Guid.NewGuid();
            Player = player;
            Game = game;
            Result = result;
            AttemptDate = DateTime.Now.ToUniversalTime();
        }
    }
}
