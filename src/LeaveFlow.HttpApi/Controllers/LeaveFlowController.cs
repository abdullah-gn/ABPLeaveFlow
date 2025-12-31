using LeaveFlow.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace LeaveFlow.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class LeaveFlowController : AbpControllerBase
{
    protected LeaveFlowController()
    {
        LocalizationResource = typeof(LeaveFlowResource);
    }
}
