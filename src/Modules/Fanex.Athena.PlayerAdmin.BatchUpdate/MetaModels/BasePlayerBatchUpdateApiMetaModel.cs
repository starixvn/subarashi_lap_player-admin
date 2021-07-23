using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Core.Utils;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels
{
    public class BasePlayerBatchUpdateApiMetaModel : IValidationMetaModel, ISiteNameMetaModel,
        ILicenseeIdMetaModel, ILanguageMetaModel, IClientIPMetaModel, IUserIdMetaModel
    {
        public string SiteName { get; set; }

        public string CustIds { get; set; }

        public string CustNames { get; set; }

        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public virtual bool IsValid
            => !string.IsNullOrWhiteSpace(CustIds) &&
            !CustIds.Split(new[] { ',' }).Any(s => !int.TryParse(s, out _));
    }
}