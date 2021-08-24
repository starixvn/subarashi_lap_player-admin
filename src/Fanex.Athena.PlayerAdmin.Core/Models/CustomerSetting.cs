namespace Fanex.Athena.PlayerAdmin.Core.Models
{
    public class CustomerSetting
    {
        public bool Suspend { get; set; }

        public bool Closed { get; set; }

        public bool AutoSuspend { get; set; }

        public bool DisableAutoMpt { get; set; }

        public bool DisableHorseracing { get; set; }

        public bool DisableOpenTote { get; set; }

        public bool DisableFI { get; set; }

        public bool SyncCasino { get; set; }

        public bool DisableCasino { get; set; }

        public bool DisableOutright { get; set; }

        public bool IsInternal { get; set; }

        public int CurrencyID { get; set; }
    }
}