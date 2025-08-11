namespace Fanex.Athena.PlayerAdmin.API.ViewModels
{
    using System.Collections.Generic;
    using DTO;

    public class PlayerListResultViewModel
    {
        public IEnumerable<Player> DataList { get; set; }

        public int TotalItem { get; set; }
    }
}