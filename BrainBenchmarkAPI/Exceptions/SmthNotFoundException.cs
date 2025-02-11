namespace BrainBenchmarkAPI.Exceptions
{
    public class SmthNotFoundException : CustomException
    {
        public SmthNotFoundException(string error, string message) 
            : base(StatusCodes.Status404NotFound, error, message)
        {

        }
    }
}
