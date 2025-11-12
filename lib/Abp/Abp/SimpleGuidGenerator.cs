namespace Abp;

/// <summary>
/// Implements <see cref="IGuidGenerator"/> by using <see cref="GuidPolyfill.CreateVersion7()"/>.
/// </summary>
public class SimpleGuidGenerator : IGuidGenerator
{
    public static SimpleGuidGenerator Instance { get; } = new();

    public virtual Guid Create()
    {
        return GuidPolyfill.CreateVersion7();
    }
}
