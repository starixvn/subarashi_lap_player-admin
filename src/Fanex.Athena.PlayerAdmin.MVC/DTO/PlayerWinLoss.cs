using System.Collections.Generic;

namespace Fanex.Athena.PlayerAdmin.MVC.DTO
{
    public class PlayerWinLoss
    {
        public IEnumerable<PlayerWinLossItem> DataList { get; set; }

        public PlayerWinLossItem Total { get; set; }

        public int TotalRows { get; set; }

        public string PromotionPrize { get; set; }
    }
}