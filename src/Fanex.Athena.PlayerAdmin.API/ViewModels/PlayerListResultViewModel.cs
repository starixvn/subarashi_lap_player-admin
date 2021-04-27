using Fanex.Athena.PlayerAdmin.API.DTO;
using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.API.ViewModels
{
    public class PlayerListResultViewModel
    {
        public IEnumerable<Player> DataList { get; set; }

        public int TotalItem { get; set; }
    }
}