using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.MVC.DTO
{
    public class TicketText
    {
        public int Index { get; set; }

        public string ExternalId { get; set; }

        public string SystemId { get; set; }

        public string TransTime { get; set; }

        public string RefNo { get; set; }

        public string Choice { get; set; }

        public string Odds { get; set; }

        public string OddsType { get; set; }

        public string Stake { get; set; }

        public string WinLoss { get; set; }

        public string BetEffective { get; set; }

        public string LicenseeWL { get; set; }

        public string Status { get; set; }

        public string Platform { get; set; }

        public string Live { get; set; }

        public string LicenseeTxId { get; set; }
    }
}
