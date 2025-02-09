namespace BrainBenchmarkAPI.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public int Code { get; } = StatusCodes.Status404NotFound;
        public string Error { get; }
        public string Message { get; }
        public UserNotFoundException(string error, string message) : base(message)
        {
            Error = error;
            Message = message;
        }
    }
}
