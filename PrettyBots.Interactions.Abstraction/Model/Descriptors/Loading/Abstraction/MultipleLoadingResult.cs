using System.Collections.ObjectModel;

namespace PrettyBots.Interactions.Abstraction.Model.Descriptors.Loading.Abstraction;

public class MultipleLoadingResult<TEntity> : ILoadingResult
    where TEntity : ILoadingResult
{
    public bool Loaded { get; }

    public string? EntityName => null;
    public Exception? LoadingException { get; }
    public IReadOnlyList<TEntity>? Entities { get; }

    private MultipleLoadingResult(bool loaded, IList<TEntity>? entities = null, 
        Exception? exception = null)
    {
        Loaded      = loaded;
        LoadingException = exception;

        if (entities is not null) {
            Entities         = new ReadOnlyCollection<TEntity>(entities);
        }
    }

    public static MultipleLoadingResult<TEntity> FromSuccess(IList<TEntity> entities)
    {
        return new MultipleLoadingResult<TEntity>(true, entities);
    }

    public static MultipleLoadingResult<TEntity> FromFailure(Exception exception)
    {
        return new MultipleLoadingResult<TEntity>(false, exception: exception);
    }
}
