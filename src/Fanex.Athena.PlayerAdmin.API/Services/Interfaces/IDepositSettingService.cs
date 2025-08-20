namespace Fanex.Athena.PlayerAdmin.API.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using DTO;
    using ViewModels;

    public interface IDepositSettingService
    {
        Task<IEnumerable<RiskLevel>> GetRiskLevelList(string language);

        Task<IEnumerable<CommGroupViewModel>> GetCommGroupList(int licenseeId);
    }
}