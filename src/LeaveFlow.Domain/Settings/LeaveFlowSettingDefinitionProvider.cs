using Volo.Abp.Settings;

namespace LeaveFlow.Settings;

public class LeaveFlowSettingDefinitionProvider : SettingDefinitionProvider
{
    public override void Define(ISettingDefinitionContext context)
    {
        //Define your own settings here. Example:
        //context.Add(new SettingDefinition(LeaveFlowSettings.MySetting1));
    }
}
