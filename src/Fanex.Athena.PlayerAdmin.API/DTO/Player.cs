namespace Fanex.Athena.PlayerAdmin.API.DTO
{
    public class Player
    {
        public int IbcCustId { get; set; }

        public string UserName { get; set; }

        public string SystemId { get; set; }

        public int CurrencyId { get; set; }

        public string Currency { get; set; }

        public decimal CurrentBalance { get; set; }

        public decimal OutstandingBalance { get; set; }

        public string OddsGroup { get; set; }

        public int SpecialLevel { get; set; }

        public string CommissionGroup { get; set; }

        public string CustomerClass { get; set; }

        public string RiskLevel { get; set; }

        public string RiskLevelKey { get; set; }

        public int? Status { get; set; }
    }
}