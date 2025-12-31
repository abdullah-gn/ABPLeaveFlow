using System;
using System.Collections.Generic;
using System.Text;
using LeaveFlow.Localization;
using Volo.Abp.Application.Services;

namespace LeaveFlow;

/* Inherit your application services from this class.
 */
public abstract class LeaveFlowAppService : ApplicationService
{
    protected LeaveFlowAppService()
    {
        LocalizationResource = typeof(LeaveFlowResource);
    }
}
