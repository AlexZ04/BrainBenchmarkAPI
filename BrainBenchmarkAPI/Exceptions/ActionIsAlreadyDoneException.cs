namespace BrainBenchmarkAPI.Exceptions
{
    public class ActionIsAlreadyDoneException : CustomException
    {
        public ActionIsAlreadyDoneException(string error, string message) : base(StatusCodes.Status400BadRequest, error, message)
        {
        }
    }
}
