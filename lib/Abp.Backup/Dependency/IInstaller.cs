using Stashbox;

namespace Abp.Dependency;

public interface IInstaller
{
    void Install(IStashboxContainer container);
}
