namespace BrainBenchmarkAPI.Exceptions
{
    public class ForbidException : CustomException
    {
        public ForbidException(string error, string message) : base(StatusCodes.Status403Forbidden, error, message)
        {
        }
    }
}
