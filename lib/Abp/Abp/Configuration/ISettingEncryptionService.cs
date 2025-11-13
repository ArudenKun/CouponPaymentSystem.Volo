using JetBrains.Annotations;

namespace Abp.Configuration;

public interface ISettingEncryptionService
{
    string? Encrypt([NotNull] SettingDefinition settingDefinition, string? plainValue);

    string? Decrypt([NotNull] SettingDefinition settingDefinition, string? encryptedValue);
}
