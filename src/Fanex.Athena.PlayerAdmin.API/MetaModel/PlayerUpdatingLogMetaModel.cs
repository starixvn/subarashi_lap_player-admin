namespace Fanex.Athena.PlayerAdmin.API.MetaModel
{
    public class PlayerUpdatingLogMetaModel
    {
        public int LicenseeCustId { get; set; }

        public int UserId { get; set; }

        public int CustId { get; set; }

        public int ActionId { get; set; }

        public string SiteName { get; set; }

        public int ActionDetailId { get; set; }

        public string Status { get; set; }

        public string NewValue { get; set; }

        public string OldValue { get; set; }
    }
}