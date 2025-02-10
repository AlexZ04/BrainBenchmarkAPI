namespace BrainBenchmarkAPI.Exceptions
{
    public class CustomException : Exception
    {
        public int Code { get; } = StatusCodes.Status400BadRequest;
        public string Error { get; }
        public string Message { get; }
        public CustomException(int code, string error, string message) : base(message)
        {
            Code = code;
            Error = error;
            Message = message;
        }
    }
}
