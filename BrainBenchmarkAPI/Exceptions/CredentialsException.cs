﻿namespace BrainBenchmarkAPI.Exceptions
{
    public class CredentialsException : Exception
    {
        public int Code { get; } = StatusCodes.Status400BadRequest;
        public string Error { get; }
        public string Message { get; }
        public CredentialsException(string error, string message) : base(message) 
        {
            Error = error;
            Message = message;
        }
    }
}
