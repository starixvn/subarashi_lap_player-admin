namespace Fanex.Athena.PlayerAdmin.Commission.MetaModels
{
    using System;
    using Core.Models;
    using Fanex.Core.Utils;

    public class BasePlayerCommissionUpdateQuery : IValidationMetaModel, IClientIPMetaModel, ILicenseeIdMetaModel, ILanguageMetaModel, IUserIdMetaModel, IUsernameMetaModel
    {
        public string CustIds { get; set; }

        public string CustNames { get; set; }

        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public string Username { get; set; }

        public string CustIdsStr { get; set; }

        public virtual bool IsValid
            => !string.IsNullOrWhiteSpace(CustIds)
               && Array.TrueForAll(CustIds.Split(','), s => !string.IsNullOrWhiteSpace(s) && s.Trim() == s && int.TryParse(s, out _));
    }
}