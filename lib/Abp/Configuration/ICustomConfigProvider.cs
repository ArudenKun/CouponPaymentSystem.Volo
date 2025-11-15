namespace Abp.Configuration;

public interface ICustomConfigProvider
{
    Dictionary<string, object> GetConfig(CustomConfigProviderContext customConfigProviderContext);
}
