using System.Collections.Generic;
using Fanex.Athena.PlayerAdmin.Commission.MetaModels;
using Fanex.Data;

namespace Fanex.Athena.PlayerAdmin.Commission.Criteria
{
    public class BasePlayerCommissionUpdateCriteria
    {
        public BasePlayerCommissionUpdateCriteria(BasePlayerCommissionUpdateQuery query)
        {
            LicenseeId = query.LicenseeId;
            CustIds = query.CustIds;
            UserId = query.UserId;
            IP = query.Ip;
            Language = query.Language;
            Permissions = query.Permissions;
        }

        public int LicenseeId { get; set; }

        public string CustIds { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public IDictionary<string, bool> Permissions { get; set; }

        public virtual string GetSPName() => string.Empty;

        public virtual void SetParameterValues(IObjectDb db)
        {
            db.SetParameterValue("LicenseeId", LicenseeId);
            db.SetParameterValue("UserId", UserId);
            db.SetParameterValue("Ip", IP);
            db.SetParameterValue("langkey", Language);
            db.SetParameterValue("strCustIDOriginal", CustIds);
            db.SetParameterValue("strCustID", null);
            db.SetParameterValue("ErrCode", null);
            db.SetParameterValue("ErrMsg", null);
        }
    }
}
