
using Fanex.Athena.PlayerAdmin.Core.Settings;

namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    public interface IFunctionUMService
    {
        (bool isUM, string timeFrom, string timeTo) GetUnderMaintenanceInfo(FunctionUMType functionUMType);
    }
}
