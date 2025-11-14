using System.Reflection;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp.Web.Security;

namespace Volo.Abp.Web.Mvc.Security;

public class AbpMvcAntiForgeryManager : AbpAntiForgeryManager
{
    private static readonly Lazy<object> AntiForgeryWorkerObject = new(() =>
    {
        var antiForgeryWorkerField = typeof(AntiForgery).GetField(
            "_worker",
            BindingFlags.NonPublic | BindingFlags.Static
        );
        if (antiForgeryWorkerField == null)
        {
            throw new AbpException(
                "Can not get _worker field of System.Web.Helpers.AntiForgery class. It's internal implementation might be changed. Please create an issue on GitHub repository to solve this."
            );
        }

        return antiForgeryWorkerField.GetValue(null);
    });

    private static readonly Lazy<MethodInfo> GetFormInputElementMethod = new(() =>
        AntiForgeryWorkerObject
            .Value.GetType()
            .GetMethod("GetFormInputElement", BindingFlags.Public | BindingFlags.Instance)
    );

    private readonly ILogger<AbpMvcAntiForgeryManager> _logger;

    public AbpMvcAntiForgeryManager(
        IOptions<AbpAntiForgeryOptions> options,
        ILogger<AbpMvcAntiForgeryManager> logger
    )
        : base(options)
    {
        _logger = logger;
    }

    public override string GenerateToken()
    {
        /* Getting Token from input element, like done in views.
         * We are using reflection because some types/methods are internal!
         */

        var tagBuilder = (TagBuilder)
            GetFormInputElementMethod.Value.Invoke(
                AntiForgeryWorkerObject.Value,
                [new HttpContextWrapper(HttpContext.Current)]
            );

        return tagBuilder.Attributes["value"];
    }

    public override bool IsValid(string cookieValue, string tokenValue)
    {
        try
        {
            AntiForgery.Validate(
                HttpContext.Current.Request.Cookies[AntiForgeryConfig.CookieName]?.Value
                ?? cookieValue,
                tokenValue
            );

            return true;
        }
        catch (HttpAntiForgeryException ex)
        {
            _logger.LogException(ex, LogLevel.Warning);
            return false;
        }
    }
}
