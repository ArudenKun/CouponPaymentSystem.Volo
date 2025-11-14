using Volo.Abp.Web.Results.Filters;

namespace Volo.Abp.Web;

public class AbpWebOptions
{
    public bool SendAllExceptionsToClients { get; set; }

    public WrapResultFilterCollection WrapResultFilters { get; } = new WrapResultFilterCollection();
}
