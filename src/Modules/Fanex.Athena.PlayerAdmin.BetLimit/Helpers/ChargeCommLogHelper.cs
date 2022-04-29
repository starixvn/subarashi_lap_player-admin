using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using Fanex.Athena.PlayerAdmin.BetLimit.Mediator.Events;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;

namespace Fanex.Athena.PlayerAdmin.BetLimit.Helpers
{
    public static class ChargeCommLogHelper
    {
        public const string Success = "Success";

        public static PlayerUpdatedChargeCommEvent CreateLogEvent(PlayerUpdateChargeCommMetaModel metaModel, int productId)
            => new PlayerUpdatedChargeCommEvent
            {
                CustID = metaModel.CustId,
                UserId = metaModel.UserId,
                SiteName = metaModel.SiteName,
                LicenseeCustId = metaModel.LicenseeCustId,
                Detail = new PlayerUpdatedProductDetailedLog
                {
                    Status = Success,
                    DetailedId = LogActionDetailId.UpdatingChargeComm,
                    ProductId = productId.ToString(),
                    Old = metaModel.OldComm + "%",
                    New = metaModel.Comm + "%"
                }
            };
    }
}
