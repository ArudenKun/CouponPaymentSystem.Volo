using System.Reflection;

namespace Abp.Authorization;

public interface IAuthorizationHelper
{
    Task AuthorizeAsync(IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes);

    void Authorize(IEnumerable<IAbpAuthorizeAttribute> authorizeAttributes);

    Task AuthorizeAsync(MethodInfo methodInfo, Type type);

    void Authorize(MethodInfo methodInfo, Type type);
}
