namespace Fanex.Athena.PlayerAdmin.BetLimit.MetaModel
{
    using System.Collections.Generic;

    public class PlayerBetLimitLogMetaModel
    {
        public int CustId { get; set; }

        public int UserId { get; set; }

        public int LicenseeCustId { get; set; }

        public string SiteName { get; set; }

        public IEnumerable<PlayerBetLimitDetailedLogMetaModel> Detail { get; set; }
    }
}
