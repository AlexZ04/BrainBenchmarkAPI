namespace BrainBenchmarkAPI.Exceptions
{
    public class CredentialsException : Exception
    {
        public string Error { get; }
        public string Message { get; }
        public CredentialsException(string error, string message) : base(message) 
        {
            Error = error;
            Message = message;
        }
    }
}
