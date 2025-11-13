namespace Abp.Configuration;

public interface ISettingClientVisibilityProvider
{
    Task<bool> CheckVisible(IScopedIocResolver scope);
}
