namespace BrainBenchmarkAPI.Data
{
    public class AttemptDb
    {
        public Guid Id { get; set; }
        public UserDb Player { get; set; }
        public GameDb Game { get; set; }
        public int Result { get; set; }

        public AttemptDb()
        {
            Id = Guid.NewGuid();
        }

        public AttemptDb(UserDb player, GameDb game, int result)
        {
            Player = player;
            Game = game;
            Result = result;
        }
    }
}
