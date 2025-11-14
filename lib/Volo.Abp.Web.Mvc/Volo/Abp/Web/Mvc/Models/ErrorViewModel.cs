using Volo.Abp.Web.Models;

namespace Volo.Abp.Web.Mvc.Models;

public class ErrorViewModel
{
    public ErrorInfo? ErrorInfo { get; set; }

    public Exception? Exception { get; set; }

    public ErrorViewModel(ErrorInfo errorInfo, Exception? exception = null)
    {
        ErrorInfo = errorInfo;
        Exception = exception;
    }
}
