namespace PrettyBots.Interactions.Exceptions;

public class CriticalServiceException : Exception
{
    public CriticalServiceException(string message) : base(message) { }
}