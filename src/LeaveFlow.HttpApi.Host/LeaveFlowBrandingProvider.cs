using Microsoft.Extensions.Localization;
using LeaveFlow.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace LeaveFlow;

[Dependency(ReplaceServices = true)]
public class LeaveFlowBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<LeaveFlowResource> _localizer;

    public LeaveFlowBrandingProvider(IStringLocalizer<LeaveFlowResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
