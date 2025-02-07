namespace BrainBenchmarkAPI.Servises
{
    public interface ITokenService
    {
        public string CreateTokenById(Guid id);
    }
}
