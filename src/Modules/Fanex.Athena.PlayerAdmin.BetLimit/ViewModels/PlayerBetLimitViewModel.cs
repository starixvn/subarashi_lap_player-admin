using System.Collections.Generic;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;

namespace Fanex.Athena.PlayerAdmin.BetLimit.ViewModels
{

    public class PlayerBetLimitViewModel
    {
        public bool IsStatusEnable { get; set; }

        public bool StatusEnable { get; set; }

        public bool Editable { get; set; }

        public bool EnableCopy { get; set; }

        public string CopyId { get; set; }

        public IEnumerable<PlayerBetLimit> Data { get; set; }
    }
}
