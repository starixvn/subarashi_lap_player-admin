using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.MVC.DTO
{
    public class BetList
    {
        public IEnumerable<TicketText> Data { get; set; }
        public TicketTotalText Total { get; set; }
        public int TotalRows { get; set; }
    }
}