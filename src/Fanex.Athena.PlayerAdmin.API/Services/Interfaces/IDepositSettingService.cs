using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.Services
{
    public interface IDepositSettingService
    {
        Task<IEnumerable<RiskLevel>> GetRiskLevelList(string language);

        Task<IEnumerable<CommGroupViewModel>> GetCommGroupList(int licenseeId);
    }
}