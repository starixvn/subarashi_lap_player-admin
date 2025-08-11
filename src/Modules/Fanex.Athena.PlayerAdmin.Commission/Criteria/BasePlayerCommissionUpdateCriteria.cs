namespace Fanex.Athena.PlayerAdmin.Commission.Criteria
{
    using Data;
    using MetaModels;

    public class BasePlayerCommissionUpdateCriteria
    {
        public BasePlayerCommissionUpdateCriteria(BasePlayerCommissionUpdateQuery query)
        {
            LicenseeId = query.LicenseeId;
            CustIds = query.CustIds;
            UserId = query.UserId;
            IP = query.Ip;
            Language = query.Language;
        }

        public int LicenseeId { get; set; }

        public string CustIds { get; set; }

        public int UserId { get; set; }

        public string IP { get; set; }

        public string Language { get; set; }

        public virtual string GetSPName() => string.Empty;

        public virtual void SetParameterValues(IObjectDb db)
        {
            db.SetParameterValue("LicenseeId", LicenseeId);
            db.SetParameterValue("UserId", UserId);
            db.SetParameterValue("Ip", IP);
            db.SetParameterValue("langkey", Language);
            db.SetParameterValue("strCustIDOriginal", CustIds);
            db.SetParameterValue("ErrCode", null);
            db.SetParameterValue("ErrMsg", null);
        }
    }
}