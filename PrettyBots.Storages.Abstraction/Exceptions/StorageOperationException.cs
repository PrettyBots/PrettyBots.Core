namespace PrettyBots.Storages.Abstraction.Exceptions;

public class StorageOperationException : Exception
{
    public StorageOpType OperationType { get; }
    public Exception? UnderlyingException { get; }

    
    public StorageOperationException(StorageOpType operationType, Exception? underlyingException = null)
    {
        OperationType       = operationType;
        UnderlyingException = underlyingException;
    }
}