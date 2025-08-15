namespace Fanex.Athena.PlayerAdmin.BetLimit.ViewModels
{
    using System.Collections.Generic;
    using DTO;
    public class PlayerBetLimitViewModel
    {
        public bool IsStatusEnable { get; set; }

        public bool StatusEnable { get; set; }

        public bool Editable { get; set; }

        public bool EnableCopy { get; set; }

        public string CopyId { get; set; }

        public int StatusSportType { get; set; }

        public int StatusBetType { get; set; }

        public IEnumerable<PlayerBetLimit> Data { get; set; }
    }
}