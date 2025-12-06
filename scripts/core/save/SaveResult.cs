
namespace SlayTheSpireLike.scripts.save;

/// <summary>
/// SaveResult
/// </summary>
public class SaveResult<T>
{
    public T Payload { get; set; }

    public static SaveResult<T> Of(T payload)
    {
        return new SaveResult<T> { Payload = payload };
    }
}