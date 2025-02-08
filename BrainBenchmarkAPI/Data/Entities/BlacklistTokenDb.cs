namespace BrainBenchmarkAPI.Data.Entities
{
    public class BlacklistTokenDb
    {
        public string? Token { get; set; }
        public DateTime AddingTime { get; set; } = DateTime.Now.ToUniversalTime();

        public BlacklistTokenDb() { }

        public BlacklistTokenDb(string? token)
        {
            Token = token;
        }
    }
}
