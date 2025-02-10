namespace BrainBenchmarkAPI.Exceptions
{
    public class UserNotFoundException : CustomException
    {
        public UserNotFoundException(string error, string message) 
            : base(StatusCodes.Status404NotFound, error, message)
        {

        }
    }
}
