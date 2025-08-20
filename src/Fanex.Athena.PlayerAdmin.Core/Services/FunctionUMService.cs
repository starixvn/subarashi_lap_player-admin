namespace Fanex.Athena.PlayerAdmin.Core.Services
{
    using System.Linq;
    using Microsoft.Extensions.Options;
    using Settings;

    public class FunctionUMService : IFunctionUMService
    {
        private readonly FunctionUMSetting _setting;

        public FunctionUMService(IOptions<FunctionUMSetting> setting)
            => _setting = setting.Value;

        public (bool isUM, string timeFrom, string timeTo) GetUnderMaintenanceInfo(FunctionUMType functionUMType)
        {
            var umInfo = _setting.Functions.FirstOrDefault(_ => _.Type == functionUMType);
            return umInfo != null ? (umInfo.IsUM, umInfo.TimeFrom, umInfo.TimeTo) : (false, null, null);
        }
    }
}
