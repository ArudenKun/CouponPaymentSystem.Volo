using System.Web.Mvc;
using Autofac.Integration.Mvc;
using Volo.Abp.Timing;

namespace Volo.Abp.Web.ModelBinding.Binders;

[ModelBinderType(typeof(DateTime))]
public class AbpMvcDateTimeBinder : DefaultModelBinder
{
    private readonly IClock _clock;

    public AbpMvcDateTimeBinder(IClock clock)
    {
        _clock = clock;
    }

    public override object? BindModel(
        ControllerContext controllerContext,
        ModelBindingContext bindingContext
    )
    {
        if (base.BindModel(controllerContext, bindingContext) is not DateTime date)
            return null;

        if (bindingContext.ModelMetadata.ContainerType != null)
        {
            if (
                bindingContext.ModelMetadata.ContainerType.IsDefined(
                    typeof(DisableDateTimeNormalizationAttribute),
                    true
                )
            )
            {
                return date;
            }

            var property = bindingContext.ModelMetadata.ContainerType.GetProperty(
                bindingContext.ModelName
            );

            if (
                property != null
                && property.IsDefined(typeof(DisableDateTimeNormalizationAttribute), true)
            )
            {
                return date;
            }
        }

        // Note: currently DisableDateTimeNormalizationAttribute is not supported for MVC action parameters.
        return _clock.Normalize(date);
    }
}
