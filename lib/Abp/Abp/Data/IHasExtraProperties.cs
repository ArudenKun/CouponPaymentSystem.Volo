namespace Abp.Data;

public interface IHasExtraProperties
{
    ExtraPropertyDictionary? ExtraProperties { get; }
}
