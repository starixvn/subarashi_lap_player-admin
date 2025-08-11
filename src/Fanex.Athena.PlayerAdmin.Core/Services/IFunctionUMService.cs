
namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    using Settings;

    public interface IFunctionUMService
    {
        (bool isUM, string timeFrom, string timeTo) GetUnderMaintenanceInfo(FunctionUMType functionUMType);
    }
}
