namespace BrainBenchmarkAPI.Exceptions
{
    public class UserHasNoAttemptsException : CustomException
    {
        public UserHasNoAttemptsException(string error, string message) : base(StatusCodes.Status200OK, error, message)
        {
        }
    }
}
