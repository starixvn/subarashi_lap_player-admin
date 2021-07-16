using Fanex.Athena.PlayerAdmin.BatchUpdate.MetaModels;
using Fanex.Data;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.Criteria
{
    public class BasePlayerBatchUpdateCriteria
    {
        public BasePlayerBatchUpdateCriteria(BasePlayerBatchUpdateApiMetaModel metaModel, string custId)
        {
            LicenseeId = metaModel.LicenseeId;
            SiteName = metaModel.SiteName;
            CustId = custId;
            UserId = metaModel.UserId;
            Ip = metaModel.Ip;
            Language = metaModel.Language;
        }

        public int LicenseeId { get; set; }

        public string SiteName { get; set; }

        public string CustId { get; set; }

        public int UserId { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public virtual string GetSPName() => string.Empty;

        public virtual void SetParameterValues(IObjectDb db)
        {
            db.SetParameterValue("CustId", CustId);
            db.SetParameterValue("Ip", Ip);
            db.SetParameterValue("langkey", Language);
            db.SetParameterValue("ErrCode", null);
            db.SetParameterValue("ErrMsg", null);
        }
    }
}