using Fanex.Athena.PlayerAdmin.Infrastructure.Models;
using Fanex.Core.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Fanex.Athena.PlayerAdmin.Commission.MetaModels
{
    public class BasePlayerCommissionUpdateQuery : IValidationMetaModel, IClientIPMetaModel, ILicenseeIdMetaModel, ILanguageMetaModel, IUserIdMetaModel, IUsernameMetaModel
    {
        public string CustIds { get; set; }

        public string CustNames { get; set; }

        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public string Username { get; set; }

        public IDictionary<string, bool> Permissions { get; set; }
        public string CustIdsStr { get; set; }

        public virtual bool IsValid
            => !string.IsNullOrWhiteSpace(CustIds) &&
            !CustIds.Split(new[] { ',' }).Any(s => !int.TryParse(s, out _));
    }
}
